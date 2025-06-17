namespace DREADBORN
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project
    using static SceneName;

    public class GameManager : Singleton<GameManager>
    {
        private Transform spawnTransform;

        [HideInInspector] public string selectClass;

        [HideInInspector] public bool isOnline;

        //�ʱ�ȭ
        public override void Awake()
        {
            base.Awake();

            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;

            //��� �÷��̾ ���� ���� �Ѿ�� ��
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        //���õ� ĳ���� �̸��� ������
        public void SetCharacter(CharacterType type)
        {
            FadeManager.Instance.FadeIn();

            selectClass = type.ToString();

            //�÷��̾� ��ȯ
            PhotonNetwork.Instantiate(selectClass, spawnTransform.position, spawnTransform.rotation);
        }

        //�÷��̾ ��ȯ��ų ��ġ�� �޾ƿ�
        public void SetSpawnPosition(Transform transform)
        {
            spawnTransform = transform;
        }

        //�÷��̾ ���ӿ��忡 ��ȯ��Ŵ
        public void SpawnPlayer()
        {
            PhotonNetwork.Instantiate(selectClass, spawnTransform.position, spawnTransform.rotation);
        }

        //�ε����� ���� �������� �ε�
        public void LoadScene()
        {
            PhotonNetwork.LoadLevel(StageLoadingScene);
        }
    }
}