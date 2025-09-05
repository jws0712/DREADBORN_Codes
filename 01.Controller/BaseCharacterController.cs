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

    //Project
    using static LayerName;
    using static AnimationClipName;

    public class BaseCharacterController : MonoBehaviourPun
    {
        #region 변수
        [Header("Camera")]
        [SerializeField] private CinemachineThirdPersonFollow playerCamera;

        [Header("Movement")]
        [SerializeField] private float spinSpeed = 10f;
        [SerializeField] private float gravity = 9.81f;

        [Header("Interaction")]
        [SerializeField] private float interactDistance = 10;

        private float speed; //플레이어가 사용하는 속도
        private float verticalVelocity;
        private float interactHoldTime;

        public bool canSkill;

        private string lastAttack;
        private Outlinable lastTargetOutline;
        private Vector3 moveDir;
        private WeaponType currentWeaponType;

        protected CharacterManager manager;

        public string LastAttack => lastAttack;
        #endregion

        //초기화
        public virtual void Initialize()
        {
            manager = GetComponent<CharacterManager>();

            //무기 설정
            SetWeaponType();

            //속도 초기화
            speed = manager.MoveSpeed;

            //스킬초기화
            canSkill = true;
        }

        #region 이동
        //이동 함수
        public void Movement()
        {
            if(!manager.IsDead)
            {
                //이동 키를 받아옴
                Vector2 moveInput = manager.InputManager.MoveVec;

                //이동 방향 계산
                moveDir = new Vector3(moveInput.x, 0, moveInput.y);

                //달리기
                Sprint();

                //회전
                Spin();
            }

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
            //달리기 키를 눌렀고 플레이어가 땅에 있을때
            if (manager.InputManager.IsPressSprint && manager.CharacterController.isGrounded)
            {
                //spped 를 달리기 속도로 교체
                speed = manager.SprintSpeed;
            }

            //이동하지 않거나 뒤, 양옆 이동할때 달리기 상태 초기화
            if (moveDir == Vector3.zero || moveDir.z < 0 || moveDir == Vector3.right || moveDir == Vector3.left)
            {
                //속도 초기화
                speed = manager.MoveSpeed;
                manager.InputManager.IsPressSprint = false;
            }
        }

        //플레이어 회전
        private void Spin()
        {
            if (manager.IsPaused) return;

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
                if (manager.InputManager.IsPressJump && !manager.IsDead)
                {
                    //점프식
                    //v = Squrt(2gh)
                    //v = 위로 튀어 오를때의 속도
                    //g = 중력 가속도
                    //h = 목표 높이
                    verticalVelocity = Mathf.Sqrt(2 * gravity * manager.JumpHeight);

                    manager.InputManager.IsPressJump = false;
                }
            }
            //공중에 있을때
            else
            {
                //수직 속도에 중력크기 만큼 빼서 공중에서의 중력을 계산한다
                verticalVelocity -= gravity * Time.deltaTime;

                //점프키를 누르지 못하게함
                manager.InputManager.IsPressJump = false;
            }

            //계산된 수직 속도를 반환
            return verticalVelocity;
        }
        #endregion

        #region 전투

        public virtual void Defend()
        {
            manager.IsDefend = manager.InputManager.IsPressDefend;
        }

        public void SetWeaponType()
        {
            currentWeaponType = manager.EquipmentManager.RightWeapon.Type;
        }

        //공격 관리
        public void AttackControl()
        {
            switch (currentWeaponType)
            {
                //근접 무기 공격
                case WeaponType.MeleeWeapon:
                    {
                        MeleeWeaponAttack();
                        break;
                    }
                    //추후 다른 타입의 무기 추가
            }

            Skill();
        }

        //근접 무기 공격 관리
        private void MeleeWeaponAttack()
        {
            if (manager.InputManager.IsPressAttack && !manager.IsPaused)
            {
                //콤보 상황일때
                if (manager.CanDoCombo)
                {
                    LightAttackCombo();
                }
                else
                {
                    //콤보 중이 아니고 애니매이션이 실행중이 아닐때
                    if (manager.IsAction) return;
                    if (manager.CanDoCombo) return;
                    Attack(Light_Attack1);
                }

            }
        }

        //공격 콤보 실행
        private void LightAttackCombo()
        {
            manager.CanDoCombo = false;

            switch (lastAttack)
            {
                case Light_Attack1:
                    {
                        Attack(Light_Attack2);
                        break;
                    }
                case Light_Attack2:
                    {
                        Attack(Light_Attack3);
                        break;
                    }
                case Light_Attack3:
                    {
                        Attack(Light_Attack1);
                        break;
                    }
            }
        }

        //공격 실행
        private void Attack(string currentAttack)
        {
            manager.AnimationController.PlayAnimation(currentAttack, true);
            lastAttack = currentAttack;
        }


        //스킬 실행
        private void Skill()
        {
            if (canSkill && manager.InputManager.IsPressSKill)
            {
                manager.InputManager.IsPressSKill = false;

                if (PhotonNetwork.IsConnected)
                {
                    manager.photonView.RPC("OnSkilled", RpcTarget.All);
                }
                else
                {
                    OnSkilled();
                }

                StartCoroutine(Co_DrainSkillCooldown());
            }


            manager.InputManager.IsPressSKill = false;
        }

        private IEnumerator Co_FillSkillCooldown()
        {
            float time = 0;


            if (PhotonNetwork.IsConnected)
            {
                manager.photonView.RPC("OnSkillEnded", RpcTarget.All);
            }
            else
            {
                OnSkillEnded();
            }


            while (time < manager.MaxSkillCoolTime)
            {
                time += Time.deltaTime;
                manager.CurrentSkillCoolTime = time;
                manager.UIController.UpdateSkillUI(manager.CurrentSkillCoolTime, manager.MaxSkillCoolTime);
                yield return null;
            }


            canSkill = true;
        }

        private IEnumerator Co_DrainSkillCooldown()
        {
            canSkill = false;

            float time = manager.SkillDurationTime;

            while (time > 0)
            {
                time -= Time.deltaTime;
                manager.CurrentSkillCoolTime = time;
                manager.UIController.UpdateSkillUI(manager.CurrentSkillCoolTime, manager.SkillDurationTime);
                yield return null;
            }

            StartCoroutine(Co_FillSkillCooldown());
        }

        //스킬이 실행됐을때 불리는 콜백
        [PunRPC]
        public virtual void OnSkilled() { }

        //스킬이 끝났을때 불리는 콜백
        [PunRPC]
        public virtual void OnSkillEnded() { }


        #endregion

        #region 상호작용

        //상호작용 
        public void Interaction()
        {
            //카메라의 앞쪽 방향으로 레이를 발사
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                if (hit.transform.TryGetComponent<IInteractable>(out IInteractable target))
                {
                    //감지한 오브젝트가 미션 테이블이고 자신이 마스터 클라이언트가 아니라면 상호작용 하지 않음
                    if (hit.transform.gameObject.CompareTag(TagName.MissionTable) && !PhotonNetwork.IsMasterClient) return;

                    //감지한 오브젝트가 플레이어 이고 사망상태가 아니라면 상호작용 하지 않음
                    if(hit.transform.gameObject.layer == LayerMask.NameToLayer(Player))
                    {
                        if(!hit.transform.gameObject.GetComponent<CharacterManager>().IsDead)
                        {
                            return;
                        }
                        else
                        {
                            manager.UIController.ToggleInteractionKeyIcon(true);
                            DoInteraction(target);
                            return;
                        }
                    }

                    hit.transform.TryGetComponent(out Outlinable targetObject);

                    if (lastTargetOutline != targetObject)
                    {
                        //상호작용 아이콘 비활성화
                        manager.UIController.ToggleInteractionKeyIcon(false);
                        DisableOutline();
                        //아웃라인 활성화
                        targetObject.enabled = true;

                        //상호작용 아이콘 활성화
                        manager.UIController.ToggleInteractionKeyIcon(true);
                        lastTargetOutline = targetObject;
                    }

                    DoInteraction(target);
                    return;
                }
            }
            //상호작용 아이콘 비활성화
            manager.UIController.ToggleInteractionKeyIcon(false);
            DisableOutline();

            interactHoldTime = 0;
            manager.UIController.UpdateInteractionGauge(interactHoldTime);
        }

        //아웃라인 비활성화
        private void DisableOutline()
        {
            if (lastTargetOutline != null)
            {
                //아웃라인 비활성화
                lastTargetOutline.enabled = false;
                lastTargetOutline = null;
            }
        }

        //상호작용 실행
        private void DoInteraction(IInteractable target)
        {
            //상호작용 키를 눌렀을때
            if (manager.InputManager.IsPressInteraction)
            {
                //키를 누른 시간을 계산
                interactHoldTime += Time.deltaTime;

                manager.UIController.UpdateInteractionGauge(interactHoldTime, target.InteractTime);

                //키를 누른 시간이 상호작용 
                if (interactHoldTime >= target.InteractTime)
                {
                    target.Interaction();
                    interactHoldTime = 0;
                    manager.InputManager.IsPressInteraction = false;
                }
            }
            else
            {
                interactHoldTime = 0;
                manager.UIController.UpdateInteractionGauge(interactHoldTime);
            }
        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * interactDistance);
        }
    }
}