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
                //서버 접속 시도
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        private void Start()
        {
            SetNewAutomaticallySyncSceneState(false);
        }


        //씬을 같이 넘어갈지 여부를 설정
        public void SetNewAutomaticallySyncSceneState(bool state)
        {
            PhotonNetwork.AutomaticallySyncScene = state;
        }

        //서버 접속에 성공 했을때
        public override void OnConnectedToMaster()
        {
            //온라인 상태를 알려줌
            GameManager.Instance.IsOnline = true;
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
            SceneManager.LoadScene(TitleScene);

        }

        //방 참가에 실패 했을때
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(cause);
        }
    }
}


