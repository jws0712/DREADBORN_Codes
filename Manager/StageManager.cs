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
                //GameManger 에 플레이어가 소환될 위치를 보냄
                GameManager.Instance.SetSpawnPosition(spawnPosition);

                //플레이어를 소환시킴
                GameManager.Instance.SpawnPlayer();
            }
        }
    }
}



