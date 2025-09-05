namespace DREADBORN
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;
    using Photon.Realtime;

    //Project
    using static SceneName;
    using UnityEngine.SceneManagement;
    using System.Linq;

    public class GameManager : Singleton<GameManager>
    {
        

        private Transform spawnTransform;
        private string nextScene;
        private GameResultType gameResultType;
        private int deadCount;

        public float PlayTime { get; set; }
        public string SelectClass { get; set; }
        public bool IsOnline { get; set; }
        public string NextScene => nextScene;
        public int DeadCount => deadCount;
        public GameResultType GameResult => gameResultType;


        //초기화
        public override void Awake()
        {
            base.Awake();
        }

        //선택된 캐릭터 이름을 설정함
        public void SetCharacter(CharacterType type)
        {
            FadeManager.Instance.FadeIn();

            SelectClass = type.ToString();

            //플레이어 소환
            PhotonNetwork.Instantiate(SelectClass, spawnTransform.position, spawnTransform.rotation);
        }

        //플레이어를 소환시킬 위치를 받아옴
        public void SetSpawnPosition(Transform transform)
        {
            spawnTransform = transform;
        }

        //플레이어를 게임월드에 소환시킴
        public void SpawnPlayer()
        {
            //테스트용
            //PhotonNetwork.Instantiate("Knight", spawnTransform.position, Quaternion.identity);

            PhotonNetwork.Instantiate(SelectClass, spawnTransform.position, spawnTransform.rotation);
        }

        //다음 불러올 씬의 이름을 저장함
        public void SetNextScene(string nextScene)
        {
            this.nextScene = nextScene;
        }

        //로딩씬을 통해 다음씬을 로드
        public void LoadScene()
        {
            PhotonNetwork.LoadLevel(LoadingScene);
        }

        public void OnRevive()
        {
            deadCount--;
        }

        public void OnDie()
        {
            deadCount++;


            if (PhotonNetwork.PlayerList.Length == 1)
            {
                LoadGameResult(GameResultType.Fail);
            }
            else if(PhotonNetwork.PlayerList.Length == deadCount)
            {
                LoadGameResult(GameResultType.Fail);
            }
        }

        //결과씬을 로드함
        public void LoadGameResult(GameResultType gameResult)
        {
            gameResultType = gameResult;
            FadeManager.Instance.FadeOut(() =>
            {
                SceneManager.LoadScene(ResultScene);
            });
        }

        //스테이지 잔류시간을 계산함
        public void UpdateStagePlayTime(float time)
        {
            PlayTime = time;
        }
    }
}