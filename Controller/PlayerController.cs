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

        //������Ƽ
        public bool IsCrouch => isCrouch;
        public bool IsGrounded => isGrounded;
        public bool IsDefend => isDefend;
        #endregion

        //�ʱ�ȭ
        public void Initialize()
        {
            manager = GetComponent<PlayerManager>();
            originCameraDamping = playerCamera.Damping.z;
            originYHeight = manager.CharacterController.height; //ĳ���� ���� �ʱ�ȭ
            speed = moveSpeed; //�ӵ� �ʱ�ȭ
        }

        #region Movement
        //�̵� �Լ�
        public void Movement()
        {
            if(manager.IsDead)
            {
                moveDir = (Vector3.right * 0) + (Vector3.left * 0);
            }
            else
            {
                //�̵� ���� ���͸� ���
                moveDir = (Vector3.right * manager.Input.MoveVec.x) + (Vector3.forward * manager.Input.MoveVec.y);

                Spin();
                Sprint();
            }

            //���� ���͸� ���� ���������� ��ȯ
            moveDir = transform.TransformDirection(moveDir);

            //���� ������ ���̸� �ӵ���ŭ �ø���
            moveDir *= speed;

            //���� ������ y���� �߷°��� �����Ѵ�
            moveDir.y = CalculatGravity();


            //����� �̵� ���� ���͸� �������� �̵�
            manager.CharacterController.Move(moveDir * Time.deltaTime);

        }

        //�÷��̾� �޸���
        private void Sprint()
        {
            if(manager.Input.IsPressSprint)
            {
                //Camera�� Damping.z �� ������ ������ ��ü
                playerCamera.Damping.z = cameraDamping;

                //spped �� �޸��� �ӵ��� ��ü
                speed = sprintSpeed;
            }
            else
            {
                //Camera�� Damping.z �ʱ�ȭ
                playerCamera.Damping.z = originCameraDamping;

                //�ӵ� �ʱ�ȭ
                speed = moveSpeed;
            }
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
        private float CalculatGravity()
        {
            //���� ������
            if(manager.CharacterController.isGrounded)
            {
                //�߷°��� ��� ������
                verticalVelocity = -5f;

                //�÷��̾� ���� �׼�
                if (manager.CharacterController.isGrounded && manager.Input.isPressJump && !manager.IsDead)
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
            }

            //���� ���� �ӵ��� ��ȯ
            return verticalVelocity;
        }

        //��ũ����
        public void Crouch()
        {

            if (manager.CharacterController.isGrounded && manager.Input.IsPressCrouch)
            {
                //�÷��̾� �ݶ��̴��� ���̸� ��ũ���� ������ ���̷� ��ü
                manager.CharacterController.height = crouchYHeight;
                //�ӵ��� ��ũ���� ���� �ӵ��� ��ü
                speed = crouchSpeed;
                isCrouch = true;
            }
            else
            {
                //�÷��̾� �ݶ��̴��� ���̸� ���� ���̷� ��ü
                manager.CharacterController.height = originYHeight;
                //�ӵ��� �ʱ�ȭ
                speed = moveSpeed;
                isCrouch = false;
            }
        }
        #endregion

        //���
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

        //��ȣ�ۿ� 
        public void Interaction()
        {
            if (manager.Input.isPressInteraction)
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hitData;

                Debug.Log("��ȣ�ۿ�");
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