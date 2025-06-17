 namespace DREADBORN
{
    //System
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //Unity
    using UnityEngine;

    //Cinemachine
    using Unity.Cinemachine;

    //Pun
    using Photon.Pun;
    using EPOOutline;

    public class PlayerController : MonoBehaviourPun
    {
        #region variable
        [Header("Camera")]
        [SerializeField] private CinemachineThirdPersonFollow playerCamera;
        [SerializeField] private float cameraDamping;

        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float spinSpeed;
        [SerializeField] private float gravity;

        [Header("Jump")]
        [SerializeField] private float jumpHeight;

        [Header("Crouch")]
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float crouchYHeight;

        [Header("Interaction")]
        [SerializeField] private float interactionDistance;
        [SerializeField] private LayerMask interactionLayer;

        private bool isDefend;

        private float speed;
        private float originCameraDamping;
        private float verticalVelocity;

        private PlayerManager manager;

        private Outlinable lastTarget;

        private Vector3 moveDir;

        //프로퍼티
        public bool IsDefend => isDefend;
        #endregion

        //초기화
        public void Initialize()
        {
            manager = GetComponent<PlayerManager>();

            originCameraDamping = playerCamera.Damping.z; //Camera Daping Z 초깃값 설정
            speed = moveSpeed; //속도 초깃값 설정
        }

        #region Movement
        //이동 함수
        public void Movement()
        {
            //이동 키를 받아옴
            Vector2 moveInput = manager.Input.MoveVec;

            //이동 방향 계산
            moveDir = new Vector3(moveInput.x, 0, moveInput.y);

            Spin();
            Sprint();

            //방향 백터를 월드 포지션으로 변환 한뒤 백터의 길이를 speed 만큼 늘림
            moveDir = transform.TransformDirection(moveDir) * speed;

            //방향 백터의 y값에 중력값을 적용한다
            moveDir.y = CalculateGravity();

            //계산한 이동 방향 백터를 바탕으로 이동
            manager.CharacterController.Move(moveDir * Time.deltaTime);

        }

        //플레이어 달리기
        private void Sprint()
        {
            if(!manager.Input.isPressSprint)
            {
                ResetSprint();
                return;
            }

            //이동하지 않거나 뒤, 양옆 이동할때 달리기 상태 초기화
            if (moveDir == Vector3.zero || moveDir == Vector3.back || moveDir == Vector3.right || moveDir == Vector3.left)
            {
                ResetSprint();
            }
            else if(manager.CharacterController.isGrounded)
            {
                //Camera의 Damping.z 를 설정한 값으로 교체
                playerCamera.Damping.z = cameraDamping;

                //spped 를 달리기 속도로 교체
                speed = sprintSpeed;
            }
        }

        private void ResetSprint()
        {
            //Camera의 Damping.z 초기화
            playerCamera.Damping.z = originCameraDamping;

            //속도 초기화
            speed = moveSpeed;
            manager.Input.isPressSprint = false;
        }

        //플레이어 회전
        private void Spin()
        {
            if (manager.isPause) return;

            //보는 방향을 카메라의 앞쪽 방향으로 설정
            Vector3 lookDir = playerCamera.transform.forward;


            //보는 방향의 높이 값은 생각하지 않음
            lookDir.y = 0;

            //바라볼 방향 계산
            Quaternion targetRot = Quaternion.LookRotation(lookDir);

            //플레이어의 회전값을 Slerp 로 바라보고 있는 방향쪽으로 보간
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * spinSpeed);
        }

        //중력값 계산
        private float CalculateGravity()
        {
            //지상에 있을때
            if(manager.CharacterController.isGrounded)
            {
                //중력값을 계속 적용함
                verticalVelocity = -5f;

                //플레이어 점프 액션
                if (manager.Input.isPressJump && !manager.IsDead)
                {
                    //점프식
                    //v = Squrt(2gh)
                    //v = 위로 튀어 오를때의 속도
                    //g = 중력 가속도
                    //h = 목표 높이
                    verticalVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);

                    manager.Input.isPressJump = false;
                }
            }
            //공중에 있을때
            else
            {
                //수직 속도에 중력크기 만큼 빼서 공중에서의 중력을 계산한다
                verticalVelocity -= gravity * Time.deltaTime;

                //점프키를 누르지 못하게함
                manager.Input.isPressJump = false;
            }

            //계산된 수직 속도를 반환
            return verticalVelocity;
        }
        #endregion

        //방어
        public void Defend()
        {
            isDefend = manager.Input.IsPressDefend;
        }

        //상호작용 
        public void Interaction()
        {
            //카메라의 앞쪽 방향으로 레이를 발사
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                if (hit.transform.TryGetComponent<IInteractable>(out IInteractable target))
                {
                    hit.transform.TryGetComponent(out Outlinable targetObject);

                    if (lastTarget != targetObject)
                    {
                        DisableOutline();
                        //아웃라인 활성화
                        targetObject.enabled = true;

                        //상호작용 아이콘 활성화
                        manager.UIController.ToggleInteractionKeyIcon(true);
                        lastTarget = targetObject;
                    }

                    //상호작용 키를 눌렀을때
                    if (manager.Input.isPressInteraction)
                    {
                        target.Interaction();
                        manager.Input.isPressInteraction = false;
                    }
                    return;
                }
            }
            DisableOutline();
        }



        private void DisableOutline()
        {
            if(lastTarget != null)
            {
                //아웃라인 비활성화
                lastTarget.enabled = false;

                //상호작용 아이콘 비활성화
                manager.UIController.ToggleInteractionKeyIcon(false);
                lastTarget = null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * interactionDistance);
        }
    }
}