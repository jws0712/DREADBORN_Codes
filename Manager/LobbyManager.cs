namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project

    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform spawnPosition;

        private void Start()
        {
            //GameManager에 플레이어가 소환될 위치를 넘겨줌
            GameManager.Instance.SetSpawnPosition(spawnPosition);
        }
    }
}


