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

        //초기화
        public override void Awake()
        {
            base.Awake();
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;
        }

        //모든 플레이어의 씬 이동 여부를 설정함
        [PunRPC]
        public void SetAutomaticallySyncScene(bool trigger)
        {
            PhotonNetwork.AutomaticallySyncScene = trigger;

            if(PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SetAutomaticallySyncScene", RpcTarget.Others, trigger);
            }
        }

        //선택된 클래스의 이름을 받아옴
        public void SetClass(string className)
        {
            selectClass = className;
        }

        //플레이어를 소환시킬 위치를 받아옴
        public void SetSpawnPosition(Transform transform)
        {
            spawnTransform = transform;
        }

        //플레이어를 게임월드에 소환시킴
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