namespace DREADBORN
{

    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;

    //Photon
    using Photon.Pun;
    using Photon.Realtime;

    //Project
    using static SceneName;
    using TMPro;

    public class TitleManager : MonoBehaviourPunCallbacks
    {
        [Header("Info")]
        [SerializeField] private GameObject titleButtonGroup = null;
        [SerializeField] private Button startButton = null;
        [SerializeField] private Button settingButton = null;
        [SerializeField] private Button quitButton = null;
        [Space(10)]
        [SerializeField] private Button joinRoomButton = null;
        [SerializeField] private GameObject inputRoomCodePanel = null;
        [SerializeField] private TMP_InputField codeInputField = null;
        [SerializeField] private Button joinButton = null;
        [SerializeField] private Button backButton = null;
        [Space(10)]
        [SerializeField] private Text connectionInfoText = null;

        //방에 참가할 수 있는 사람 수
        private const int maxPlayerCount = 3;

        private string roomName = null;

        private readonly char[] chars = "0123456789ABCDEFGHIJKLMNOPQRXTUVWXYZ".ToCharArray();

        private void Start()
        {
            startButton.onClick.AddListener(StartButton);

            joinRoomButton.onClick.AddListener(JoinRoomButton);
            joinButton.onClick.AddListener(JoinButton);
            backButton.onClick.AddListener(BackButton);
            
            settingButton.onClick.AddListener(SettingButton);
            quitButton.onClick.AddListener(QuitButton);

            //서버 접속
            if(!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();

                connectionInfoText.text = "Connecting...";
            }
        }

        //서버 접속 성공시 실행
        public override void OnConnectedToMaster()
        {
            titleButtonGroup.SetActive(true);
            connectionInfoText.text = "Online";
        }

        #region RoomButton
        //방 만들기 버튼 함수
        private void StartButton()
        {
            startButton.interactable = false;
            roomName = GenerateRoomCode(6);
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayerCount});
        }

        //랜덤한 방 코드를 생성하는 함수
        public string GenerateRoomCode(int codeLength)
        {
            string code = null;

            for(int i = 0; i < codeLength; i++)
            {
                int index = Random.Range(0, chars.Length);
                code += chars[index];
            }

            return code;
        }

        //방 입장에 성공했을때 실행하는 함수
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(LobbyScene);
        }

        //방 참가하는 패널을 활성화 하는 버튼 함수
        private void JoinRoomButton()
        {
            //기능
            inputRoomCodePanel.gameObject.SetActive(true);
        }

        //실제로 방에 참가하는 함수
        private void JoinButton()
        {
            PhotonNetwork.JoinRoom(codeInputField.text.ToUpper());
        }

        //방 참가하는 패널의 뒤로가기 버튼 함수
        private void BackButton()
        {
            inputRoomCodePanel.SetActive(false);
        }

        //방 입장에 실패했을때 실행
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(cause);
        }
        #endregion

        #region TitleButton

        //셋팅버튼 함수
        private void SettingButton()
        {
            //기능
            Debug.Log("SettingButton");
        }

        //나가기 버튼 함수
        private void QuitButton()
        {
            Application.Quit();
        }
        #endregion
    }
}