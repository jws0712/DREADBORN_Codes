namespace DREADBORN
{
    //System
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Unity.Cinemachine;

    //OutLine
    using EPOOutline;

    //Photon
    using Photon.Pun;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    //Project
    using static AnimationClipName;
    using static ObjectPoolObjectName;
    using Sound;

    public class CharacterManager : Character, IPunObservable, IInteractable
    {
        #region 변수
        [Header("Player")]
        [SerializeField] private PlayerStat stat;
        [SerializeField] private GameObject spineProxy;
        [SerializeField] private float reviveTime;
        [SerializeField] private Color deadOutlineColor;
        [SerializeField] private float deadColliderRadius;
        [SerializeField] private Vector3 hitShakeVelocity;
        [SerializeField] private Vector3 defendHitShakeVelocity;

        [Header("Network")]
        [SerializeField] private float smoothPosSpeed;
        [SerializeField] private float smoothRotSpeed;
        [SerializeField] private float teleprotDistance;

        [Header("SFX")]
        [SerializeField] private AudioClip defendSFX;

        private float originColliderRadius;

        #region 플레이어
        private CharacterType characterType;
        private Sprite iconSprite;
        private float hp;
        private float moveSpeed;
        private float sprintSpeed;
        private float jumpHeight;
        private float maxDefensePoint;
        private Sprite skillIcon;
        private float skillDurationTime;
        private float maxSkillCoolTime;

        private float maxRecoverDefenseTime; //최대 방어력 회복 시간
        private float currentDefensePoint;
        private Coroutine recoverDefenseCoroutine; //방어력 회복 코루틴

        private float recoverDefenseDelay; //피격 후 방어력 회복 대기 시간

        private int level;

        private Hashtable playerCustomProperties = new Hashtable();
        #endregion

        #region 컴포넌트
        private CinemachineImpulseSource impulseSource;
        private CharacterController characterController;
        private Animator animator;
        private CameraController cameraController;
        private Outlinable outline;
        private BaseCharacterController controller;
        private CharacterAnimationController animationController;
        private CharacterInputManager inputManager;
        private CharacterUIController uiController;
        private CharacterEquipmentManager equipmentManager;
        private PlayerInput playerInput;
        #endregion

        #region 네트워크 변수
        private Vector3 networkPos;
        private Quaternion networkRot;
        private Color originOutlineColor;
        #endregion

        #region 프로퍼티
        public bool IsDefend { get; set; }
        public bool IsAction { get; set; }
        public bool IsPaused { get; set; }
        public bool CanDoCombo { get; set; }
        public float CurrentSkillCoolTime { get; set; }
        public float MaxSkillCoolTime => maxSkillCoolTime;
        public float SkillDurationTime => skillDurationTime;
        public int Level => level;
        public float MoveSpeed => moveSpeed;
        public float SprintSpeed => sprintSpeed;
        public float JumpHeight => jumpHeight;
        public float MaxDefensePoint => maxDefensePoint;
        public float CurrentDefensePoint => currentDefensePoint;
        public float InteractTime => reviveTime;
        public Sprite SkillIcon => skillIcon;
        public Sprite IconSprite => iconSprite;
        public CinemachineImpulseSource ImpulseSource => impulseSource;
        public CharacterUIController UIController => uiController;
        public BaseCharacterController Controller => controller;
        public CharacterController CharacterController => characterController;
        public CameraController CameraController => cameraController;
        public CharacterAnimationController AnimationController => animationController;
        public CharacterInputManager InputManager => inputManager;
        public CharacterEquipmentManager EquipmentManager => equipmentManager;
        public Animator Animator => animator;
        public CharacterType CharacterType => characterType;
        #endregion

        #endregion

        #region MonoBehaviour 콜백
        private void Awake()
        {
            Initialize();
            InitializePlayerStat(stat);
        }
        private void Start()
        {
            level = 30;

            if (photonView.IsMine)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                //커스텀 프로퍼티 플레이어 클래스 이름, 플레이어 현재 체력 설정
                UpdateCustomProperties();

                //아웃라인 비활성화
                outline.enabled = false;

                //스크립트 초기화
                inputManager.Initialize();


                //방어력 초기화
                currentDefensePoint = maxDefensePoint;
            }

            controller.Initialize();
            animationController.Initialize();
            equipmentManager.Initialize();
            uiController.Initialize();

            //처음 콜라이더 위치 및 반지름 저장
            originColliderRadius = characterController.radius;

            //처음 아웃라인 색 초기화
            originOutlineColor = outline.OutlineParameters.Color;
        }
        private void Update()
        {
            if(photonView.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    if(PhotonNetwork.IsConnected)
                    {
                        photonView.RPC("TakeDamage", RpcTarget.MasterClient, 10f);
                    }
                    else
                    {
                        TakeDamage(10f);
                    }
                }

                if (Input.GetKeyDown(KeyCode.F2))
                {
                    Interaction();
                }

                //이동및 중력 계산 메서드
                controller.Movement();

                //Plater UI Controller 메서드 업데이트
                uiController.UpdatePlayerUI();
                uiController.ToggleSettingPanel();

                //Player Animator Controller 메서드 업데이트
                animationController.UpdateAnimation(inputManager.MoveVec.x, inputManager.MoveVec.y);

                //플레이어가 사망하면 업데이트 하지 않음
                if (isDead) return;

                //PlayerController 메서드 업데이트
                controller.Interaction();
                controller.AttackControl();
                controller.Defend();
            }
        }
        //위치 동기화
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
        #endregion

        //컴포넌트 초기화
        private void Initialize()
        {
            playerInput = GetComponent<PlayerInput>();
            outline = GetComponent<Outlinable>();
            uiController = GetComponent<CharacterUIController>();
            impulseSource = GetComponent<CinemachineImpulseSource>();
            characterController = GetComponent<CharacterController>();
            controller = GetComponent<BaseCharacterController>();
            inputManager = GetComponent<CharacterInputManager>();
            cameraController = GetComponentInChildren<CameraController>();
            equipmentManager = GetComponentInChildren<CharacterEquipmentManager>();
            animationController = GetComponentInChildren<CharacterAnimationController>();
            animator = GetComponentInChildren<Animator>();

            if (!photonView.IsMine)
            {
                Destroy(playerInput);
                Destroy(cameraController.gameObject);
                Destroy(uiController.PlayerUICanvas.gameObject);
            }
        }

        //플레이어 스텟 초기화
        private void InitializePlayerStat(PlayerStat stat)
        {
            characterType = stat.CharacterType;
            iconSprite = stat.IconSprite;
            hp = stat.Hp;
            MaxHp = hp;
            moveSpeed = stat.MoveSpeed;
            sprintSpeed = stat.SprintSpeed;
            jumpHeight = stat.JumpHeight;
            maxDefensePoint = stat.MaxDefensePoint;
            maxRecoverDefenseTime = stat.MaxRecoverDefenseTime;
            recoverDefenseDelay = stat.RecoverDefenseDelay;
            skillIcon = stat.SkillIcon;
            maxSkillCoolTime = stat.SkillCoolTime;
            skillDurationTime = stat.SkillDurationTime;
        }

        //상호작용
        public void Interaction()
        {
            if(PhotonNetwork.IsConnected)
            photonView.RPC("Revive", RpcTarget.All);
        }

        //플레이어 부활
        [PunRPC]
        public void Revive()
        {
            GameManager.Instance.OnRevive();

            if(photonView.IsMine)
            {
                animationController.PlayAnimation(StandUp, true);
            }

            ObjectPoolManager.Instance.SpawnPoolObject(ReviveEffect, transform.position, Quaternion.identity);

            characterController.radius = originColliderRadius;
        }

        //플레이어의 상태를 사망 이전으로 돌림
        public void ResetDead()
        {
            if(photonView.IsMine)
            {
                isDead = false;
                IsAction = false;
                CanDoCombo = false;

                currentHp = MaxHp * 0.5f;

                currentDefensePoint = maxDefensePoint;

                photonView.RPC("UpdateLifeStatus", RpcTarget.All, currentHp, isDead);

                UpdateCustomProperties();
            }

            spineProxy.SetActive(true);

            outline.OutlineParameters.Color = originOutlineColor;
        }

        //사망
        protected override void Die()
        {
            base.Die();

            if(PhotonNetwork.IsConnected)
            {
                if(photonView.IsMine)
                {
                    UpdateCustomProperties();

                    photonView.RPC("OnDie", RpcTarget.All);
                }
            }
            else
            {
                OnDie();
            }
        }

        //사망 상태 동기화
        [PunRPC]
        public void OnDie()
        {
            animationController.PlayAnimation(AnimationClipName.Die, true);

            spineProxy.SetActive(false);

            outline.OutlineParameters.Color = deadOutlineColor;

            characterController.radius = deadColliderRadius;

            GameManager.Instance.OnDie();
        }

        //데미지
        [PunRPC]
        public override void TakeDamage(float damage)
        {
            if (isDead) return;

            //막고 있는 상태에서 맞았다면
            if (IsDefend && currentDefensePoint > 0)
            {
                Debug.Log("데미지 막음");
                if(photonView.IsMine)
                {
                    currentDefensePoint -= damage;

                    //기존 회복 중단
                    if (recoverDefenseCoroutine != null)
                    {
                        StopCoroutine(recoverDefenseCoroutine);
                    }

                    //회복 재시작
                    recoverDefenseCoroutine = StartCoroutine(Co_RecoverDefense());

                    if(inputManager.IsPressDefend)
                    {
                        SoundManager.instance.SFXPlay("Defend", defendSFX);

                        animationController.PlayAnimation(DefendHit, true);

                        //카메라 쉐이크 실행
                        impulseSource.GenerateImpulse(defendHitShakeVelocity);
                    }
                }
            }
            else
            {
                Debug.Log("데미지 들어옴");
                base.TakeDamage(damage);

                if(photonView.IsMine)
                {
                    animationController.PlayAnimation(Hit, true);

                    UpdateCustomProperties();

                    //카메라 쉐이크 실행
                    impulseSource.GenerateImpulse(hitShakeVelocity);
                }
            }
        }
        
        //커스텀 프로퍼티 업데이트
        private void UpdateCustomProperties()
        {
            playerCustomProperties[CustomPropertyKey.PlayerType] = characterType.ToString();
            playerCustomProperties[CustomPropertyKey.MaxHp] = MaxHp;
            playerCustomProperties[CustomPropertyKey.CurrentHp] = currentHp;
            playerCustomProperties[CustomPropertyKey.IsDead] = isDead;
            playerCustomProperties[CustomPropertyKey.Level] = level;

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
        }

        //방어력 회복
        private IEnumerator Co_RecoverDefense()
        {
            if (isDead) yield break;

            //딜레이 만큼 대기
            yield return new WaitForSeconds(recoverDefenseDelay);

            float lastDefensePoint = currentDefensePoint;

            //방어력이 얼마나 깎였는지 비율로 계산
            float currentDefensePersent = (maxDefensePoint - currentDefensePoint) / maxDefensePoint;

            //깎인 비율에 따라 회복에 걸릴 시간을 계산
            float recoverDefenseTime = maxRecoverDefenseTime * currentDefensePersent;

            float time = 0;

            while (time < recoverDefenseTime)
            {
                time += Time.deltaTime;

                //진행도 계산
                float progress = time / recoverDefenseTime;

                currentDefensePoint = Mathf.Lerp(lastDefensePoint, maxDefensePoint, progress);

                yield return null;
            }

            //현재 방어력을 최대 방어력으로 초기화
            currentDefensePoint = maxDefensePoint;
            recoverDefenseCoroutine = null;
        }

        //변수 동기화
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //현재 위치
                stream.SendNext(transform.position);

                //현재 각도
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
