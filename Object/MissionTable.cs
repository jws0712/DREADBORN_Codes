namespace DREADBORN
{
    //UnityENgine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project
    using static SceneName;



    public class MissionTable : MonoBehaviour, IInteractable
    {
        //상호작용
        public void Interaction()
        {
            if(PhotonNetwork.IsMasterClient)
            {


                //씬을 로드함
                PhotonNetwork.LoadLevel(InGameScene);
            }
        }
    }
}

