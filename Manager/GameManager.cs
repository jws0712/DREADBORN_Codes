namespace DREADBORN
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.SceneManagement;

    //Photon
    using Photon.Pun;

    //Project
    using static SceneName;

    public class GameManager : Singleton<GameManager>
    {
        private Transform spawnTransform = null;
        private string selectClass = null;

        public bool isOnline = default;

        //�ʱ�ȭ
        public override void Awake()
        {
            base.Awake();

            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;

            //��� �÷��̾ ���� ���� �Ѿ�� ��
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        //���õ� Ŭ������ �̸��� �޾ƿ�
        public void SetClass(string className)
        {
            selectClass = className;
        }

        //�÷��̾ ��ȯ��ų ��ġ�� �޾ƿ�
        public void SetSpawnPosition(Transform transform)
        {
            spawnTransform = transform;
        }

        //�÷��̾ ���ӿ��忡 ��ȯ��Ŵ
        public void SpawnPlayer(string className = null)
        {

            if (className == null)
            {
                PhotonNetwork.Instantiate(selectClass, spawnTransform.position, spawnTransform.rotation);
            }
            else
            {
                PhotonNetwork.Instantiate(className, spawnTransform.position, spawnTransform.rotation);
            }

        }
    }
}