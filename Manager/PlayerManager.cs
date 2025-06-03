namespace DREADBORN
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.Animations.Rigging;
    using Unity.Cinemachine;


    //Photon
    using Photon.Pun;

    //Project
    using static AnimatorParameter;

    public class PlayerManager : Character, IPunObservable
    {
        #region variable
        [Header("Player")]
        [SerializeField] private float maxDefendPoint = default;
        [SerializeField] private Sprite playerIconSprite = null;
        [SerializeField] private GameObject playerCamera = null;

        [Header("Network")]
        [SerializeField] private float smoothPosSpeed = default;
        [SerializeField] private float smoothRotSpeed = default;
        [SerializeField] private float teleprotDistance = default;

        private float currentDefendPoint = default;

        [HideInInspector] public bool isDefend = default;
        [HideInInspector] public bool isFall = default;
        [HideInInspector] public bool isAction = default;
        [HideInInspector] public bool canCombo = default;

        private Vector3 networkPos = default;

        private Quaternion networkRot = default;

        private CinemachineImpulseSource source = null;
        private CharacterController characterController = null;
        private Animator thirdPersonAnim = null;
        private AimController aim = null;

        private PlayerController playerController = null;
        private PlayerAttackController attackController = null;
        private PlayerAnimationController animController = null;
        private PlayerInputManager inputManager = null;
        private PlayerWeaponHandler inventory = null;
        private PlayerUIController ui = null;
        private PlayerEquipmentManager thirdPersonEquipmentManager = null;
        private PlayerInput playerInput = null;


        //property
        public float MaxDefendPorint => maxDefendPoint;
        public float CurrentDefendPoint => currentDefendPoint;
        public Sprite PlayerIconSprite => playerIconSprite;
        public PlayerUIController UIController => ui;
        public PlayerController PlayerController => playerController;
        public PlayerAttackController AttackContorller => attackController;
        public CharacterController CharacterController => characterController;
        public AimController Aim => aim;
        public PlayerAnimationController AnimController => animController;
        public PlayerInputManager Input => inputManager;
        public PlayerEquipmentManager ThirdPersonEquipmentManager => thirdPersonEquipmentManager;
        public PlayerWeaponHandler Inventroy => inventory;
        public Animator ThirdPersonAnim => thirdPersonAnim;
        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            //�� ��ũ��Ʈ �ʱ�ȭ
            thirdPersonEquipmentManager.Initialize();
            animController.Initialize();
            ui.Initialize();
            
            //���� �÷��̾� �϶�
            if (photonView.IsMine)
            {
                playerController.Initialize();
                attackController.Initialize();
            }
        }

        private void Update()
        {
            //����Ʈ �÷��̾��� ��� �������� ����
            if (!photonView.IsMine) return;

            if(UnityEngine.Input.GetKeyDown(KeyCode.I))
            {
                Revive();
            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.O))
            {
                TakeDamage(100f);
            }

            //�̵��� �߷� ��� �޼���
            playerController.Movement();

            //�÷��̾ ����ϸ� ������Ʈ ���� ����
            if (isDead) return;

            //PlayerController �޼��� ������Ʈ
            playerController.Defend();
            playerController.Crouch();
            playerController.Interaction();

            //Player Attack Controller �޼��� ������Ʈ
            attackController.AttackControl();

            //Player Animator Controller �޼��� ������Ʈ
            animController.UpdateAnimation(inputManager.MoveVec.x, inputManager.MoveVec.y);

            //Plater UI Controller �޼��� ������Ʈ
            ui.UpdateUIValue();
            ui.ToggleSettingPanel();
        }

        private void FixedUpdate()
        {
            //����Ʈ �÷��̾� �϶�
            if (!photonView.IsMine)
            {
                //���� ���� �������� ���������� ����
                transform.position = Vector3.Lerp(transform.position, networkPos, smoothPosSpeed * Time.fixedDeltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, networkRot, smoothRotSpeed * Time.fixedDeltaTime);

                //���� ��ġ�� �������� ���� ��ġ�� ���� ���̰� ���ٸ� �÷��̾ �������� ���� ��ġ�� ���� �̵� ��Ŵ 
                if (Vector3.Distance(transform.position, networkPos) > teleprotDistance)
                {
                    transform.position = networkPos;
                }
            }
        }

        //�ʱ�ȭ
        private void Initialize()
        {
            currentDefendPoint = maxDefendPoint;

            ui = GetComponent<PlayerUIController>();
            playerInput = GetComponent<PlayerInput>();
            aim = GetComponentInChildren<AimController>();
            inventory = GetComponent<PlayerWeaponHandler>();
            animController = GetComponent<PlayerAnimationController>();
            thirdPersonAnim = GetComponentInChildren<Animator>();
            thirdPersonEquipmentManager = GetComponentInChildren<PlayerEquipmentManager>();


            //���� �÷��̾� �϶�
            if (photonView.IsMine)
            {
                source = GetComponent<CinemachineImpulseSource>();
                attackController = GetComponent<PlayerAttackController>();
                characterController = GetComponent<CharacterController>();
                playerController = GetComponent<PlayerController>();
                inputManager = GetComponent<PlayerInputManager>();

            }
            //����Ʈ �÷��̾� �϶�
            else
            {
                Destroy(aim.gameObject);
                Destroy(playerInput);
                Destroy(playerCamera);
            }
        }
        
        //�÷��̾� ��Ȱ
        public void Revive()
        {
            animController.SetTriggerAnimation(StandUp);

        }

        //�÷��̾��� ���¸� ��� �������� ����
        public void ResetDead()
        {
            isDead = false;
            currentHp = maxHp;
            currentDefendPoint = maxDefendPoint;
        }

        //������
        [PunRPC]
        public override void TakeDamage(float damage)
        {
            if (isDead) return;

            if (isDefend && currentDefendPoint > 0)
            {
                animController.SetTriggerAnimation(DefendHit);
                currentDefendPoint -= damage;
            }
            else
            {
                animController.SetTriggerAnimation(Hit);
                base.TakeDamage(damage);
            }

            source.GenerateImpulse();

            ui.UpdateUIValue();
        }

        //���
        [PunRPC]
        public override void Die()
        {
            animController.SetTriggerAnimation(Dead);
            base.Die();
        }

        //��ġ ����ȭ
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                networkPos = (Vector3)stream.ReceiveNext();
                networkRot = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
