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

            //GameManager�� �÷��̾ ��ȯ�� ��ġ�� �Ѱ���
            GameManager.Instance.SetSpawnPosition(spawnPosition);
        }
    }
}


