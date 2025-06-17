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

        //������Ƽ
        public bool IsDefend => isDefend;
        #endregion

        //�ʱ�ȭ
        public void Initialize()
        {
            manager = GetComponent<PlayerManager>();

            originCameraDamping = playerCamera.Damping.z; //Camera Daping Z �ʱ갪 ����
            speed = moveSpeed; //�ӵ� �ʱ갪 ����
        }

        #region Movement
        //�̵� �Լ�
        public void Movement()
        {
            //�̵� Ű�� �޾ƿ�
            Vector2 moveInput = manager.Input.MoveVec;

            //�̵� ���� ���
            moveDir = new Vector3(moveInput.x, 0, moveInput.y);

            Spin();
            Sprint();

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
            if(!manager.Input.isPressSprint)
            {
                ResetSprint();
                return;
            }

            //�̵����� �ʰų� ��, �翷 �̵��Ҷ� �޸��� ���� �ʱ�ȭ
            if (moveDir == Vector3.zero || moveDir == Vector3.back || moveDir == Vector3.right || moveDir == Vector3.left)
            {
                ResetSprint();
            }
            else if(manager.CharacterController.isGrounded)
            {
                //Camera�� Damping.z �� ������ ������ ��ü
                playerCamera.Damping.z = cameraDamping;

                //spped �� �޸��� �ӵ��� ��ü
                speed = sprintSpeed;
            }
        }

        private void ResetSprint()
        {
            //Camera�� Damping.z �ʱ�ȭ
            playerCamera.Damping.z = originCameraDamping;

            //�ӵ� �ʱ�ȭ
            speed = moveSpeed;
            manager.Input.isPressSprint = false;
        }

        //�÷��̾� ȸ��
        private void Spin()
        {
            if (manager.isPause) return;

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
                if (manager.Input.isPressJump && !manager.IsDead)
                {
                    //������
                    //v = Squrt(2gh)
                    //v = ���� Ƣ�� �������� �ӵ�
                    //g = �߷� ���ӵ�
                    //h = ��ǥ ����
                    verticalVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);

                    manager.Input.isPressJump = false;
                }
            }
            //���߿� ������
            else
            {
                //���� �ӵ��� �߷�ũ�� ��ŭ ���� ���߿����� �߷��� ����Ѵ�
                verticalVelocity -= gravity * Time.deltaTime;

                //����Ű�� ������ ���ϰ���
                manager.Input.isPressJump = false;
            }

            //���� ���� �ӵ��� ��ȯ
            return verticalVelocity;
        }
        #endregion

        //���
        public void Defend()
        {
            isDefend = manager.Input.IsPressDefend;
        }

        //��ȣ�ۿ� 
        public void Interaction()
        {
            //ī�޶��� ���� �������� ���̸� �߻�
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                if (hit.transform.TryGetComponent<IInteractable>(out IInteractable target))
                {
                    hit.transform.TryGetComponent(out Outlinable targetObject);

                    if (lastTarget != targetObject)
                    {
                        DisableOutline();
                        //�ƿ����� Ȱ��ȭ
                        targetObject.enabled = true;

                        //��ȣ�ۿ� ������ Ȱ��ȭ
                        manager.UIController.ToggleInteractionKeyIcon(true);
                        lastTarget = targetObject;
                    }

                    //��ȣ�ۿ� Ű�� ��������
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
                //�ƿ����� ��Ȱ��ȭ
                lastTarget.enabled = false;

                //��ȣ�ۿ� ������ ��Ȱ��ȭ
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