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
                //GameManger �� �÷��̾ ��ȯ�� ��ġ�� ����
                GameManager.Instance.SetSpawnPosition(spawnPosition);

                //�÷��̾ ��ȯ��Ŵ
                GameManager.Instance.SpawnPlayer();
            }
        }

        private void Update()
        {
            GameManager.Instance.UpdateStagePlayTime(Time.time - startTime);
        }
    }
}



