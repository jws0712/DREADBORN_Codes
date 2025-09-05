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
        #region ����
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

        #region �÷��̾�
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

        private float maxRecoverDefenseTime; //�ִ� ���� ȸ�� �ð�
        private float currentDefensePoint;
        private Coroutine recoverDefenseCoroutine; //���� ȸ�� �ڷ�ƾ

        private float recoverDefenseDelay; //�ǰ� �� ���� ȸ�� ��� �ð�

        private int level;

        private Hashtable playerCustomProperties = new Hashtable();
        #endregion

        #region ������Ʈ
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

        #region ��Ʈ��ũ ����
        private Vector3 networkPos;
        private Quaternion networkRot;
        private Color originOutlineColor;
        #endregion

        #region ������Ƽ
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

        #region MonoBehaviour �ݹ�
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

                //Ŀ���� ������Ƽ �÷��̾� Ŭ���� �̸�, �÷��̾� ���� ü�� ����
                UpdateCustomProperties();

                //�ƿ����� ��Ȱ��ȭ
                outline.enabled = false;

                //��ũ��Ʈ �ʱ�ȭ
                inputManager.Initialize();


                //���� �ʱ�ȭ
                currentDefensePoint = maxDefensePoint;
            }

            controller.Initialize();
            animationController.Initialize();
            equipmentManager.Initialize();
            uiController.Initialize();

            //ó�� �ݶ��̴� ��ġ �� ������ ����
            originColliderRadius = characterController.radius;

            //ó�� �ƿ����� �� �ʱ�ȭ
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

                //�̵��� �߷� ��� �޼���
                controller.Movement();

                //Plater UI Controller �޼��� ������Ʈ
                uiController.UpdatePlayerUI();
                uiController.ToggleSettingPanel();

                //Player Animator Controller �޼��� ������Ʈ
                animationController.UpdateAnimation(inputManager.MoveVec.x, inputManager.MoveVec.y);

                //�÷��̾ ����ϸ� ������Ʈ ���� ����
                if (isDead) return;

                //PlayerController �޼��� ������Ʈ
                controller.Interaction();
                controller.AttackControl();
                controller.Defend();
            }
        }
        //��ġ ����ȭ
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
        #endregion

        //������Ʈ �ʱ�ȭ
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

        //�÷��̾� ���� �ʱ�ȭ
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

        //��ȣ�ۿ�
        public void Interaction()
        {
            if(PhotonNetwork.IsConnected)
            photonView.RPC("Revive", RpcTarget.All);
        }

        //�÷��̾� ��Ȱ
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

        //�÷��̾��� ���¸� ��� �������� ����
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

        //���
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

        //��� ���� ����ȭ
        [PunRPC]
        public void OnDie()
        {
            animationController.PlayAnimation(AnimationClipName.Die, true);

            spineProxy.SetActive(false);

            outline.OutlineParameters.Color = deadOutlineColor;

            characterController.radius = deadColliderRadius;

            GameManager.Instance.OnDie();
        }

        //������
        [PunRPC]
        public override void TakeDamage(float damage)
        {
            if (isDead) return;

            //���� �ִ� ���¿��� �¾Ҵٸ�
            if (IsDefend && currentDefensePoint > 0)
            {
                Debug.Log("������ ����");
                if(photonView.IsMine)
                {
                    currentDefensePoint -= damage;

                    //���� ȸ�� �ߴ�
                    if (recoverDefenseCoroutine != null)
                    {
                        StopCoroutine(recoverDefenseCoroutine);
                    }

                    //ȸ�� �����
                    recoverDefenseCoroutine = StartCoroutine(Co_RecoverDefense());

                    if(inputManager.IsPressDefend)
                    {
                        SoundManager.instance.SFXPlay("Defend", defendSFX);

                        animationController.PlayAnimation(DefendHit, true);

                        //ī�޶� ����ũ ����
                        impulseSource.GenerateImpulse(defendHitShakeVelocity);
                    }
                }
            }
            else
            {
                Debug.Log("������ ����");
                base.TakeDamage(damage);

                if(photonView.IsMine)
                {
                    animationController.PlayAnimation(Hit, true);

                    UpdateCustomProperties();

                    //ī�޶� ����ũ ����
                    impulseSource.GenerateImpulse(hitShakeVelocity);
                }
            }
        }
        
        //Ŀ���� ������Ƽ ������Ʈ
        private void UpdateCustomProperties()
        {
            playerCustomProperties[CustomPropertyKey.PlayerType] = characterType.ToString();
            playerCustomProperties[CustomPropertyKey.MaxHp] = MaxHp;
            playerCustomProperties[CustomPropertyKey.CurrentHp] = currentHp;
            playerCustomProperties[CustomPropertyKey.IsDead] = isDead;
            playerCustomProperties[CustomPropertyKey.Level] = level;

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
        }

        //���� ȸ��
        private IEnumerator Co_RecoverDefense()
        {
            if (isDead) yield break;

            //������ ��ŭ ���
            yield return new WaitForSeconds(recoverDefenseDelay);

            float lastDefensePoint = currentDefensePoint;

            //������ �󸶳� �𿴴��� ������ ���
            float currentDefensePersent = (maxDefensePoint - currentDefensePoint) / maxDefensePoint;

            //���� ������ ���� ȸ���� �ɸ� �ð��� ���
            float recoverDefenseTime = maxRecoverDefenseTime * currentDefensePersent;

            float time = 0;

            while (time < recoverDefenseTime)
            {
                time += Time.deltaTime;

                //���൵ ���
                float progress = time / recoverDefenseTime;

                currentDefensePoint = Mathf.Lerp(lastDefensePoint, maxDefensePoint, progress);

                yield return null;
            }

            //���� ������ �ִ� �������� �ʱ�ȭ
            currentDefensePoint = maxDefensePoint;
            recoverDefenseCoroutine = null;
        }

        //���� ����ȭ
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //���� ��ġ
                stream.SendNext(transform.position);

                //���� ����
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
