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
        //상호작용
        public void Interaction()
        {
            if(PhotonNetwork.IsMasterClient) photonView.RPC("LoadStage", RpcTarget.All);
        }

        [PunRPC]
        //다음 스테이지 로드
        public void LoadStage()
        {
            FadeManager.Instance.FadeOut(() => {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameManager.Instance.LoadScene();
                }
            });
        }
    }
}

