namespace DREADBORN
{
    //UnityENgine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project
    using static SceneName;

    public class MissionTable : MonoBehaviourPun, IInteractable
    {
        [SerializeField] private float interactTime;

        public float InteractTime => interactTime;

        //상호작용
        public void Interaction()
        {
            if(PhotonNetwork.IsMasterClient) photonView.RPC("LoadStage", RpcTarget.All);
        }

        //다음 스테이지 로드
        [PunRPC]
        public void LoadStage()
        {
            GameManager.Instance.SetNextScene(Stage1);
            NetworkManager.Instance.SetNewAutomaticallySyncSceneState(true);

            if(PhotonNetwork.IsMasterClient)
            {
                FadeManager.Instance.FadeOut(() =>
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.Instance.LoadScene();
                    }
                });
            }
            else
            {
                FadeManager.Instance.FadeOut();
            }

        }
    }
}

