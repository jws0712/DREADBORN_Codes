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
        #region ����
        [Header("Camera")]
        [SerializeField] private CinemachineThirdPersonFollow playerCamera;

        [Header("Movement")]
        [SerializeField] private float spinSpeed = 10f;
        [SerializeField] private float gravity = 9.81f;

        [Header("Interaction")]
        [SerializeField] private float interactDistance = 10;

        private float speed; //�÷��̾ ����ϴ� �ӵ�
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

        //�ʱ�ȭ
        public virtual void Initialize()
        {
            manager = GetComponent<CharacterManager>();

            //���� ����
            SetWeaponType();

            //�ӵ� �ʱ�ȭ
            speed = manager.MoveSpeed;

            //��ų�ʱ�ȭ
            canSkill = true;
        }

        #region �̵�
        //�̵� �Լ�
        public void Movement()
        {
            if(!manager.IsDead)
            {
                //�̵� Ű�� �޾ƿ�
                Vector2 moveInput = manager.InputManager.MoveVec;

                //�̵� ���� ���
                moveDir = new Vector3(moveInput.x, 0, moveInput.y);

                //�޸���
                Sprint();

                //ȸ��
                Spin();
            }

            //���� ���͸� ���� ���������� ��ȯ �ѵ� ������ ���̸� speed ��ŭ �ø�
            moveDir = transform.TransformDirection(moveDir) * speed;

            //���� ������ y���� �߷°��� �����Ѵ�
            moveDir.y = CalculateGravity();


            //����� �̵� ���� ���͸� �������� �̵�
            manager.CharacterController.Move(moveDir * Time.deltaTime);

        }

        //�÷��̾� �޸���
        private void Sprint()
        {
            //�޸��� Ű�� ������ �÷��̾ ���� ������
            if (manager.InputManager.IsPressSprint && manager.CharacterController.isGrounded)
            {
                //spped �� �޸��� �ӵ��� ��ü
                speed = manager.SprintSpeed;
            }

            //�̵����� �ʰų� ��, �翷 �̵��Ҷ� �޸��� ���� �ʱ�ȭ
            if (moveDir == Vector3.zero || moveDir.z < 0 || moveDir == Vector3.right || moveDir == Vector3.left)
            {
                //�ӵ� �ʱ�ȭ
                speed = manager.MoveSpeed;
                manager.InputManager.IsPressSprint = false;
            }
        }

        //�÷��̾� ȸ��
        private void Spin()
        {
            if (manager.IsPaused) return;

            //���� ������ ī�޶��� ���� �������� ����
            Vector3 lookDir = playerCamera.transform.forward;


            //���� ������ ���� ���� �������� ����
            lookDir.y = 0;

            //�ٶ� ���� ���
            Quaternion targetRot = Quaternion.LookRotation(lookDir);

            //�÷��̾��� ȸ������ Slerp �� �ٶ󺸰� �ִ� ���������� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * spinSpeed);
        }

        //�߷°� ���
        private float CalculateGravity()
        {
            //���� ������
            if(manager.CharacterController.isGrounded)
            {
                //�߷°��� ��� ������
                verticalVelocity = -5f;

                //�÷��̾� ���� �׼�
                if (manager.InputManager.IsPressJump && !manager.IsDead)
                {
                    //������
                    //v = Squrt(2gh)
                    //v = ���� Ƣ�� �������� �ӵ�
                    //g = �߷� ���ӵ�
                    //h = ��ǥ ����
                    verticalVelocity = Mathf.Sqrt(2 * gravity * manager.JumpHeight);

                    manager.InputManager.IsPressJump = false;
                }
            }
            //���߿� ������
            else
            {
                //���� �ӵ��� �߷�ũ�� ��ŭ ���� ���߿����� �߷��� ����Ѵ�
                verticalVelocity -= gravity * Time.deltaTime;

                //����Ű�� ������ ���ϰ���
                manager.InputManager.IsPressJump = false;
            }

            //���� ���� �ӵ��� ��ȯ
            return verticalVelocity;
        }
        #endregion

        #region ����

        public virtual void Defend()
        {
            manager.IsDefend = manager.InputManager.IsPressDefend;
        }

        public void SetWeaponType()
        {
            currentWeaponType = manager.EquipmentManager.RightWeapon.Type;
        }

        //���� ����
        public void AttackControl()
        {
            switch (currentWeaponType)
            {
                //���� ���� ����
                case WeaponType.MeleeWeapon:
                    {
                        MeleeWeaponAttack();
                        break;
                    }
                    //���� �ٸ� Ÿ���� ���� �߰�
            }

            Skill();
        }

        //���� ���� ���� ����
        private void MeleeWeaponAttack()
        {
            if (manager.InputManager.IsPressAttack && !manager.IsPaused)
            {
                //�޺� ��Ȳ�϶�
                if (manager.CanDoCombo)
                {
                    LightAttackCombo();
                }
                else
                {
                    //�޺� ���� �ƴϰ� �ִϸ��̼��� �������� �ƴҶ�
                    if (manager.IsAction) return;
                    if (manager.CanDoCombo) return;
                    Attack(Light_Attack1);
                }

            }
        }

        //���� �޺� ����
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

        //���� ����
        private void Attack(string currentAttack)
        {
            manager.AnimationController.PlayAnimation(currentAttack, true);
            lastAttack = currentAttack;
        }


        //��ų ����
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

        //��ų�� ��������� �Ҹ��� �ݹ�
        [PunRPC]
        public virtual void OnSkilled() { }

        //��ų�� �������� �Ҹ��� �ݹ�
        [PunRPC]
        public virtual void OnSkillEnded() { }


        #endregion

        #region ��ȣ�ۿ�

        //��ȣ�ۿ� 
        public void Interaction()
        {
            //ī�޶��� ���� �������� ���̸� �߻�
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                if (hit.transform.TryGetComponent<IInteractable>(out IInteractable target))
                {
                    //������ ������Ʈ�� �̼� ���̺��̰� �ڽ��� ������ Ŭ���̾�Ʈ�� �ƴ϶�� ��ȣ�ۿ� ���� ����
                    if (hit.transform.gameObject.CompareTag(TagName.MissionTable) && !PhotonNetwork.IsMasterClient) return;

                    //������ ������Ʈ�� �÷��̾� �̰� ������°� �ƴ϶�� ��ȣ�ۿ� ���� ����
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
                        //��ȣ�ۿ� ������ ��Ȱ��ȭ
                        manager.UIController.ToggleInteractionKeyIcon(false);
                        DisableOutline();
                        //�ƿ����� Ȱ��ȭ
                        targetObject.enabled = true;

                        //��ȣ�ۿ� ������ Ȱ��ȭ
                        manager.UIController.ToggleInteractionKeyIcon(true);
                        lastTargetOutline = targetObject;
                    }

                    DoInteraction(target);
                    return;
                }
            }
            //��ȣ�ۿ� ������ ��Ȱ��ȭ
            manager.UIController.ToggleInteractionKeyIcon(false);
            DisableOutline();

            interactHoldTime = 0;
            manager.UIController.UpdateInteractionGauge(interactHoldTime);
        }

        //�ƿ����� ��Ȱ��ȭ
        private void DisableOutline()
        {
            if (lastTargetOutline != null)
            {
                //�ƿ����� ��Ȱ��ȭ
                lastTargetOutline.enabled = false;
                lastTargetOutline = null;
            }
        }

        //��ȣ�ۿ� ����
        private void DoInteraction(IInteractable target)
        {
            //��ȣ�ۿ� Ű�� ��������
            if (manager.InputManager.IsPressInteraction)
            {
                //Ű�� ���� �ð��� ���
                interactHoldTime += Time.deltaTime;

                manager.UIController.UpdateInteractionGauge(interactHoldTime, target.InteractTime);

                //Ű�� ���� �ð��� ��ȣ�ۿ� 
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