namespace DREADBORN
{

    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;

    //Photon
    using Photon.Pun;
    using Photon.Realtime;

    //TMP
    using TMPro;

    //Project
    using static AnimatorParameter;
    using Sound;

    public class TitleManager : MonoBehaviourPunCallbacks
    {
        [Header("Info")]
        [SerializeField] private GameObject character;
        [Space(10)]
        [SerializeField] private GameObject buttonGroup;
        [SerializeField] private Button startButton;
        [SerializeField] private Button joinRoomButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button quitButton;
        [Space(10)]
        [SerializeField] private TMP_InputField codeInputField;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button backButton;
        [Header("SFX")]
        [SerializeField] private AudioClip buttonClickSFX;
        [SerializeField] private AudioClip bg;


        //�濡 ������ �� �ִ� ��� ��
        private const int maxPlayerCount = 4;

        //���ڵ� ����
        private const int roomCodeLength = 6;

        private string roomName;

        private readonly char[] chars = "23456789ABCDEFGHIJKLMNOPQRXTUVWXYZ".ToCharArray();

        private Animator anim;

        private Animator characterAnimator;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            characterAnimator = character.GetComponent<Animator>();

        }

        private void Start()
        {
            buttonGroup.SetActive(false);

            SoundManager.instance.BgSoundPlay(bg);

            FadeManager.Instance.FadeIn();

            NetworkManager.Instance.SetNewAutomaticallySyncSceneState(false);

            startButton.onClick.AddListener(StartButton);

            joinRoomButton.onClick.AddListener(JoinRoomButton);
            joinButton.onClick.AddListener(JoinButton);
            backButton.onClick.AddListener(BackButton);
            
            settingButton.onClick.AddListener(SettingButton);
            quitButton.onClick.AddListener(QuitButton);
        }

        private void Update()
        {
            if(PhotonNetwork.IsConnectedAndReady)
            {
                buttonGroup.SetActive(true);
            }
        }

        //�� ����� ��ư
        private void StartButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);
            characterAnimator.SetTrigger(GameStart);
            startButton.interactable = false;

            FadeManager.Instance.FadeOut(() =>
            {
                roomName = GenerateRoomCode(roomCodeLength);
                PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayerCount });
            });
        }

        //���� ��ư
        private void SettingButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            //���
        }

        //������ ��ư
        private void QuitButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            Application.Quit();
        }

        //�� �����ϴ� �г� Ȱ��ȭ
        private void JoinRoomButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            anim.SetTrigger(EnableJoinRoomPanel);
        }

        //�濡 �����ϴ� ��ư
        private void JoinButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            if (codeInputField.text == "") return;

            FadeManager.Instance.FadeOut(() => {
                PhotonNetwork.JoinRoom(codeInputField.text.ToUpper());
            });
        }

        //�� �����ϴ� �г��� �ڷΰ��� ��ư
        private void BackButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            anim.SetTrigger(DisableJoinRoomPanel);
        }

        //������ �� �ڵ带 ����
        public string GenerateRoomCode(int codeLength)
        {
            string code = null;

            for (int i = 0; i < codeLength; i++)
            {
                int index = Random.Range(0, chars.Length);
                code += chars[index];
            }

            return code;
        }
    }
}