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
            //각 스크립트 초기화
            thirdPersonEquipmentManager.Initialize();
            animController.Initialize();
            ui.Initialize();
            
            //로컬 플레이어 일때
            if (photonView.IsMine)
            {
                playerController.Initialize();
                attackController.Initialize();
            }
        }

        private void Update()
        {
            //리모트 플레이어인 경우 실행하지 않음
            if (!photonView.IsMine) return;

            if(UnityEngine.Input.GetKeyDown(KeyCode.I))
            {
                Revive();
            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.O))
            {
                TakeDamage(100f);
            }

            //이동및 중력 계산 메서드
            playerController.Movement();

            //플레이어가 사망하면 업데이트 하지 않음
            if (isDead) return;

            //PlayerController 메서드 업데이트
            playerController.Defend();
            playerController.Crouch();
            playerController.Interaction();

            //Player Attack Controller 메서드 업데이트
            attackController.AttackControl();

            //Player Animator Controller 메서드 업데이트
            animController.UpdateAnimation(inputManager.MoveVec.x, inputManager.MoveVec.y);

            //Plater UI Controller 메서드 업데이트
            ui.UpdateUIValue();
            ui.ToggleSettingPanel();
        }

        private void FixedUpdate()
        {
            //리모트 플레이어 일때
            if (!photonView.IsMine)
            {
                //현재 값을 서버에서 받은값으로 보간
                transform.position = Vector3.Lerp(transform.position, networkPos, smoothPosSpeed * Time.fixedDeltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, networkRot, smoothRotSpeed * Time.fixedDeltaTime);

                //현재 위치와 서버에서 받은 위치가 많은 차이가 난다면 플레이어를 서버에서 받은 위치로 강제 이동 시킴 
                if (Vector3.Distance(transform.position, networkPos) > teleprotDistance)
                {
                    transform.position = networkPos;
                }
            }
        }

        //초기화
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


            //로컬 플레이어 일때
            if (photonView.IsMine)
            {
                source = GetComponent<CinemachineImpulseSource>();
                attackController = GetComponent<PlayerAttackController>();
                characterController = GetComponent<CharacterController>();
                playerController = GetComponent<PlayerController>();
                inputManager = GetComponent<PlayerInputManager>();

            }
            //리모트 플레이어 일때
            else
            {
                Destroy(aim.gameObject);
                Destroy(playerInput);
                Destroy(playerCamera);
            }
        }
        
        //플레이어 부활
        public void Revive()
        {
            animController.SetTriggerAnimation(StandUp);

        }

        //플레이어의 상태를 사망 이전으로 돌림
        public void ResetDead()
        {
            isDead = false;
            currentHp = maxHp;
            currentDefendPoint = maxDefendPoint;
        }

        //데미지
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

        //사망
        [PunRPC]
        public override void Die()
        {
            animController.SetTriggerAnimation(Dead);
            base.Die();
        }

        //위치 동기화
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
