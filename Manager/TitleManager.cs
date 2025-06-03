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

        //�濡 ������ �� �ִ� ��� ��
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

            //���� ����
            if(!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();

                connectionInfoText.text = "Connecting...";
            }
        }

        //���� ���� ������ ����
        public override void OnConnectedToMaster()
        {
            titleButtonGroup.SetActive(true);
            connectionInfoText.text = "Online";
        }

        #region RoomButton
        //�� ����� ��ư �Լ�
        private void StartButton()
        {
            startButton.interactable = false;
            roomName = GenerateRoomCode(6);
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayerCount});
        }

        //������ �� �ڵ带 �����ϴ� �Լ�
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

        //�� ���忡 ���������� �����ϴ� �Լ�
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(LobbyScene);
        }

        //�� �����ϴ� �г��� Ȱ��ȭ �ϴ� ��ư �Լ�
        private void JoinRoomButton()
        {
            //���
            inputRoomCodePanel.gameObject.SetActive(true);
        }

        //������ �濡 �����ϴ� �Լ�
        private void JoinButton()
        {
            PhotonNetwork.JoinRoom(codeInputField.text.ToUpper());
        }

        //�� �����ϴ� �г��� �ڷΰ��� ��ư �Լ�
        private void BackButton()
        {
            inputRoomCodePanel.SetActive(false);
        }

        //�� ���忡 ���������� ����
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(cause);
        }
        #endregion

        #region TitleButton

        //���ù�ư �Լ�
        private void SettingButton()
        {
            //���
            Debug.Log("SettingButton");
        }

        //������ ��ư �Լ�
        private void QuitButton()
        {
            Application.Quit();
        }
        #endregion
    }
}