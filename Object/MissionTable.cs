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
        //��ȣ�ۿ�
        public void Interaction()
        {
            if(PhotonNetwork.IsMasterClient) photonView.RPC("LoadStage", RpcTarget.All);
        }

        [PunRPC]
        //���� �������� �ε�
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

