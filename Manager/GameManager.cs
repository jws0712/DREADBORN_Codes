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

        //초기화
        public override void Awake()
        {
            base.Awake();

            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;

            //모든 플레이어가 씬을 같이 넘어가게 함
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        //선택된 캐릭터 이름을 설정함
        public void SetCharacter(CharacterType type)
        {
            FadeManager.Instance.FadeIn();

            selectClass = type.ToString();

            //플레이어 소환
            PhotonNetwork.Instantiate(selectClass, spawnTransform.position, spawnTransform.rotation);
        }

        //플레이어를 소환시킬 위치를 받아옴
        public void SetSpawnPosition(Transform transform)
        {
            spawnTransform = transform;
        }

        //플레이어를 게임월드에 소환시킴
        public void SpawnPlayer()
        {
            PhotonNetwork.Instantiate(selectClass, spawnTransform.position, spawnTransform.rotation);
        }

        //로딩씬을 통해 다음씬을 로드
        public void LoadScene()
        {
            PhotonNetwork.LoadLevel(StageLoadingScene);
        }
    }
}