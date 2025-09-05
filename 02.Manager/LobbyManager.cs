namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;
    using Sound;

    //Project

    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private AudioClip bg;

        private void Start()
        {
            SoundManager.instance.BgSoundPlay(bg);

            //GameManager에 플레이어가 소환될 위치를 넘겨줌
            GameManager.Instance.SetSpawnPosition(spawnPosition);
        }
    }
}


