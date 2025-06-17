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
                    //���� ���� �õ�
                    PhotonNetwork.ConnectUsingSettings();
                }
            }
        }

        //���� ���ӿ� ���� ������
        public override void OnConnectedToMaster()
        {
            //�¶��� ���¸� �˷���
            GameManager.Instance.isOnline = true;
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
            FadeManager.Instance.FadeOut(() => { SceneManager.LoadScene(TitleScene); });

        }

        //�� ������ ���� ������
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(cause);
        }
    }
}


