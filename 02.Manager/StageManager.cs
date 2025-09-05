namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;
    using Sound;

    public class StageManager : MonoBehaviourPun
    {
        [Header("Info")]
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private AudioClip bg;

        private float startTime;

        private void Start()
        {
            SoundManager.instance.BgSoundPlay(bg);

            FadeManager.Instance.FadeIn();

            startTime = Time.time;

            if (PhotonNetwork.IsConnected)
            {
                //GameManger 에 플레이어가 소환될 위치를 보냄
                GameManager.Instance.SetSpawnPosition(spawnPosition);

                //플레이어를 소환시킴
                GameManager.Instance.SpawnPlayer();
            }
        }

        private void Update()
        {
            GameManager.Instance.UpdateStagePlayTime(Time.time - startTime);
        }
    }
}



