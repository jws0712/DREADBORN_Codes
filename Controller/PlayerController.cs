namespace DREADBORN
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //Unity
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.Animations.Rigging;

    //Cinemachine
    using Unity.Cinemachine;


    //Pun
    using Photon.Pun;

    public class PlayerController : MonoBehaviourPun
    {
        #region variable
        [Header("Camera")]
        [SerializeField] private CinemachineThirdPersonFollow playerCamera = null;
        [SerializeField] private float cameraDamping = default;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = default;
        [SerializeField] private float sprintSpeed = default;
        [SerializeField] private float spinSpeed = default;
        [SerializeField] private float gravity = 9.81f;

        [Header("Jump")]
        [SerializeField] private float jumpHeight = default;

        [Header("Crouch")]
        [SerializeField] private float crouchSpeed = default;
        [SerializeField] private float crouchYHeight = default;

        [Header("Interaction")]
        [SerializeField] private float interactionDistance = default;

        private float speed = default;
        private float originYHeight = default;
        private float originCameraDamping = default;
        private float verticalVelocity = default;

        private bool isCrouch = default;
        private bool isGrounded = default;
        private bool isDefend = default;

        private IInteractable interactableObject = null;

        private PlayerManager manager = null;

        private Vector3 moveDir = default;

        //프로퍼티
        public bool IsCrouch => isCrouch;
        public bool IsGrounded => isGrounded;
        public bool IsDefend => isDefend;
        #endregion

        //초기화
        public void Initialize()
        {
            manager = GetComponent<PlayerManager>();
            originCameraDamping = playerCamera.Damping.z;
            originYHeight = manager.CharacterController.height; //캐릭터 높이 초기화
            speed = moveSpeed; //속도 초기화
        }

        #region Movement
        //이동 함수
        public void Movement()
        {
            if(manager.IsDead)
            {
                moveDir = (Vector3.right * 0) + (Vector3.left * 0);
            }
            else
            {
                //이동 방향 백터를 계산
                moveDir = (Vector3.right * manager.Input.MoveVec.x) + (Vector3.forward * manager.Input.MoveVec.y);

                Spin();
                Sprint();
            }

            //방향 백터를 월드 포지션으로 변환
            moveDir = transform.TransformDirection(moveDir);

            //방향 벡터의 길이를 속도만큼 늘린다
            moveDir *= speed;

            //방향 백터의 y값에 중력값을 적용한다
            moveDir.y = CalculatGravity();


            //계산한 이동 방향 백터를 바탕으로 이동
            manager.CharacterController.Move(moveDir * Time.deltaTime);

        }

        //플레이어 달리기
        private void Sprint()
        {
            if(manager.Input.IsPressSprint)
            {
                //Camera의 Damping.z 를 설정한 값으로 교체
                playerCamera.Damping.z = cameraDamping;

                //spped 를 달리기 속도로 교체
                speed = sprintSpeed;
            }
            else
            {
                //Camera의 Damping.z 초기화
                playerCamera.Damping.z = originCameraDamping;

                //속도 초기화
                speed = moveSpeed;
            }
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
        private float CalculatGravity()
        {
            //지상에 있을때
            if(manager.CharacterController.isGrounded)
            {
                //중력값을 계속 적용함
                verticalVelocity = -5f;

                //플레이어 점프 액션
                if (manager.CharacterController.isGrounded && manager.Input.isPressJump && !manager.IsDead)
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
            }

            //계산된 수직 속도를 반환
            return verticalVelocity;
        }

        //웅크리기
        public void Crouch()
        {

            if (manager.CharacterController.isGrounded && manager.Input.IsPressCrouch)
            {
                //플레이어 콜라이더의 높이를 웅크리기 상태의 높이로 교체
                manager.CharacterController.height = crouchYHeight;
                //속도를 웅크리기 상태 속도로 교체
                speed = crouchSpeed;
                isCrouch = true;
            }
            else
            {
                //플레이어 콜라이더의 높이를 원래 높이로 교체
                manager.CharacterController.height = originYHeight;
                //속도를 초기화
                speed = moveSpeed;
                isCrouch = false;
            }
        }
        #endregion

        //방어
        public void Defend()
        {
            if(manager.Input.IsPressDefend)
            {
                manager.isDefend = true;
            }
            else
            {
                manager.isDefend = false;
            }
        }

        //상호작용 
        public void Interaction()
        {
            if (manager.Input.isPressInteraction)
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hitData;

                Debug.Log("상호작용");
                if(Physics.Raycast(ray, out hitData, interactionDistance))
                {
                    if (hitData.transform.TryGetComponent<IInteractable>(out interactableObject))
                    {
                        interactableObject.Interaction();
                    }
                }
            }

            manager.Input.isPressInteraction = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * interactionDistance);
        }
    }
}