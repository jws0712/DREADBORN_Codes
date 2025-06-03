namespace DREADBORN
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    public class GameManager : Singleton<GameManager>
    {
        private Transform spawnTransform = null;
        private string selectClass = null;

        //�ʱ�ȭ
        public override void Awake()
        {
            base.Awake();
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;
        }

        //��� �÷��̾��� �� �̵� ���θ� ������
        [PunRPC]
        public void SetAutomaticallySyncScene(bool trigger)
        {
            PhotonNetwork.AutomaticallySyncScene = trigger;

            if(PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SetAutomaticallySyncScene", RpcTarget.Others, trigger);
            }
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