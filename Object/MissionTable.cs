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
        //��ȣ�ۿ�
        public void Interaction()
        {
            if(PhotonNetwork.IsMasterClient)
            {


                //���� �ε���
                PhotonNetwork.LoadLevel(InGameScene);
            }
        }
    }
}

