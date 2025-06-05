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

    public class NetworkManager : MonoBehaviourPunCallbacks
    {

        private void Awake()
        {
            //���ȿ� �ٸ� NetworkManager �� �ִٸ� �ڽ��� �ı���Ŵ
            if(FindFirstObjectByType<NetworkManager>() != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);

                if (!PhotonNetwork.IsConnected)
                {
                    Debug.Log("���� ���� �õ�");
                    //���� ���� �õ�
                    PhotonNetwork.ConnectUsingSettings();
                }
            }
        }

        //���� ���ӿ� ���� ������
        public override void OnConnectedToMaster()
        {
            GameManager.Instance.isOnline = true;
        }
        
        //�� ������ ���� ������
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(LobbyScene);
        }

        //���� ��������
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(TitleScene);
        }

        //�� ������ ���� ������
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(cause);
        }
    }
}


