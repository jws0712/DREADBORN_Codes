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
                //��� �÷��̾ ���� �̵��ϰ���
                GameManager.Instance.SetAutomaticallySyncScene(true);
                //���� �ε���
                PhotonNetwork.LoadLevel(Stage1);
            }
        }
    }
}

