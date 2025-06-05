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

    public class TitleManager : MonoBehaviour
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
        }

        private void Update()
        {
            //서버에 접속했을때 실행
            if (GameManager.Instance.isOnline)
            {
                connectionInfoText.text = "Online";
                titleButtonGroup.SetActive(true);
            }
            else
            {
                connectionInfoText.text = "Connecting...";
            }
        }

        #region TitleButtons
        //방 만들기 버튼
        private void StartButton()
        {
            startButton.interactable = false;
            roomName = GenerateRoomCode(6);
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayerCount });
        }

        //설정 버튼
        private void SettingButton()
        {
            //기능
        }

        //나가기 버튼
        private void QuitButton()
        {
            Application.Quit();
        }
        #endregion

        #region JoinRoomPanelButtons

        //방 참가하는 패널 활성화
        private void JoinRoomButton()
        {
            inputRoomCodePanel.gameObject.SetActive(true);
        }

        //방에 참가하는 버튼
        private void JoinButton()
        {
            PhotonNetwork.JoinRoom(codeInputField.text.ToUpper());
        }

        //방 참가하는 패널의 뒤로가기 버튼
        private void BackButton()
        {
            inputRoomCodePanel.SetActive(false);
        }
        #endregion


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