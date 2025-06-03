namespace DREADBORN
{
    using Photon.Pun;
    //UnityEngine
    using UnityEngine;

    public class StageManager : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private Transform spawnPosition = null;

        private void Start()
        {
            if(PhotonNetwork.IsConnected)
            {
                //GameManger �� �÷��̾ ��ȯ�� ��ġ�� ����
                GameManager.Instance.SetSpawnPosition(spawnPosition);

                //�÷��̾ ��ȯ��Ŵ
                GameManager.Instance.SpawnPlayer();
            }
        }
    }
}



