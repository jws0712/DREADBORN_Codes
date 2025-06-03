namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project

    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform spawnPosition = null;

        private void Start()
        {
            //GameManager�� �÷��̾ ��ȯ�� ��ġ�� �Ѱ���
            GameManager.Instance.SetSpawnPosition(spawnPosition);
        }
    }
}


