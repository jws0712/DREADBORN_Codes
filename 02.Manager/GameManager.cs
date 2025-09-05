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


        //�ʱ�ȭ
        public override void Awake()
        {
            base.Awake();
        }

        //���õ� ĳ���� �̸��� ������
        public void SetCharacter(CharacterType type)
        {
            FadeManager.Instance.FadeIn();

            SelectClass = type.ToString();

            //�÷��̾� ��ȯ
            PhotonNetwork.Instantiate(SelectClass, spawnTransform.position, spawnTransform.rotation);
        }

        //�÷��̾ ��ȯ��ų ��ġ�� �޾ƿ�
        public void SetSpawnPosition(Transform transform)
        {
            spawnTransform = transform;
        }

        //�÷��̾ ���ӿ��忡 ��ȯ��Ŵ
        public void SpawnPlayer()
        {
            //�׽�Ʈ��
            //PhotonNetwork.Instantiate("Knight", spawnTransform.position, Quaternion.identity);

            PhotonNetwork.Instantiate(SelectClass, spawnTransform.position, spawnTransform.rotation);
        }

        //���� �ҷ��� ���� �̸��� ������
        public void SetNextScene(string nextScene)
        {
            this.nextScene = nextScene;
        }

        //�ε����� ���� �������� �ε�
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

        //������� �ε���
        public void LoadGameResult(GameResultType gameResult)
        {
            gameResultType = gameResult;
            FadeManager.Instance.FadeOut(() =>
            {
                SceneManager.LoadScene(ResultScene);
            });
        }

        //�������� �ܷ��ð��� �����
        public void UpdateStagePlayTime(float time)
        {
            PlayTime = time;
        }
    }
}