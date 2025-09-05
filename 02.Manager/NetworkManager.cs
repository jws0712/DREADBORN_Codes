namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;


    //Photon
    using Photon.Pun;
    using Photon.Realtime;

    //Project
    using static SceneName;

    public class NetworkManager : Singleton<NetworkManager>
    {
        public override void Awake()
        {
            base.Awake();

            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;

            if (!PhotonNetwork.IsConnected)
            {
                //���� ���� �õ�
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        private void Start()
        {
            SetNewAutomaticallySyncSceneState(false);
        }


        //���� ���� �Ѿ�� ���θ� ����
        public void SetNewAutomaticallySyncSceneState(bool state)
        {
            PhotonNetwork.AutomaticallySyncScene = state;
        }

        //���� ���ӿ� ���� ������
        public override void OnConnectedToMaster()
        {
            //�¶��� ���¸� �˷���
            GameManager.Instance.IsOnline = true;
        }

        //�� ������ ���� ������
        public override void OnJoinedRoom()
        {


            //�κ���� �ε���
            PhotonNetwork.LoadLevel(LobbyScene);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            //�� ���� ����
            FadeManager.Instance.FadeIn();
        }

        //���� ��������
        public override void OnLeftRoom()
        {
            //Ÿ��Ʋ���� �ε���
            SceneManager.LoadScene(TitleScene);

        }

        //�� ������ ���� ������
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(cause);
        }
    }
}


