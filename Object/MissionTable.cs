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
                //모든 플레이어가 씬을 이동하게함
                GameManager.Instance.SetAutomaticallySyncScene(true);
                //씬을 로드함
                PhotonNetwork.LoadLevel(Stage1);
            }
        }
    }
}

