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
            //씬안에 다른 NetworkManager 가 있다면 자신을 파괴시킴
            if(FindFirstObjectByType<NetworkManager>() != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);

                if (!PhotonNetwork.IsConnected)
                {
                    //서버 접속 시도
                    PhotonNetwork.ConnectUsingSettings();
                }
            }
        }

        //서버 접속에 성공 했을때
        public override void OnConnectedToMaster()
        {
            //온라인 상태를 알려줌
            GameManager.Instance.isOnline = true;
        }
        
        //방 참가에 성공 했을때
        public override void OnJoinedRoom()
        {


            //로비씬을 로드함
            PhotonNetwork.LoadLevel(LobbyScene);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            //방 참가 실패
            FadeManager.Instance.FadeIn();
        }

        //방을 나갔을때
        public override void OnLeftRoom()
        {
            //타이틀씬을 로드함
            FadeManager.Instance.FadeOut(() => { SceneManager.LoadScene(TitleScene); });

        }

        //방 참가에 실패 했을때
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(cause);
        }
    }
}


