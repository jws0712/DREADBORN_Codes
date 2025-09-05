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


        //방에 참가할 수 있는 사람 수
        private const int maxPlayerCount = 4;

        //방코드 길이
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

        //방 만들기 버튼
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

        //설정 버튼
        private void SettingButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            //기능
        }

        //나가기 버튼
        private void QuitButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            Application.Quit();
        }

        //방 참가하는 패널 활성화
        private void JoinRoomButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            anim.SetTrigger(EnableJoinRoomPanel);
        }

        //방에 참가하는 버튼
        private void JoinButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            if (codeInputField.text == "") return;

            FadeManager.Instance.FadeOut(() => {
                PhotonNetwork.JoinRoom(codeInputField.text.ToUpper());
            });
        }

        //방 참가하는 패널의 뒤로가기 버튼
        private void BackButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            anim.SetTrigger(DisableJoinRoomPanel);
        }

        //랜덤한 방 코드를 생성
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