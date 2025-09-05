namespace DREADBORN
{
    //System
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.SceneManagement;

    //Photon
    using Photon.Pun;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    //Project
    using static SceneName;
    using static CustomPropertyKey;
    using Sound;

    public class LoadingManager : MonoBehaviourPun
    {
        [Header("Info")]
        [SerializeField] private List<CharacterModelEntry> characterEntries = new List<CharacterModelEntry>();
        [SerializeField] private Transform[] characterSetPosition;
        [SerializeField] private AudioClip bg;
        private int loadingfinshCount;

        private AsyncOperation ao;

        private bool once;

        private Dictionary<string, GameObject> characterDatas = new();

        private void Start()
        {
            SoundManager.instance.BgSoundPlay(bg);

            NetworkManager.Instance.SetNewAutomaticallySyncSceneState(false);

            //캐릭터 정보 딕셔너리 초기화
            foreach (var entry in characterEntries)
            {
                characterDatas[entry.type.ToString()] = entry.model;
            }

            //현재 입장한 플레이어 명수 만큼 반복
            for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                //처음 위치에는 LocalPlayer 모델 셋팅
                if(i == 0)
                {
                    Hashtable playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
                    SetCharacterModel(playerCP, i);
                }
                //그 외 위치는 다른 플레이어 모델 셋팅
                else
                {
                    Hashtable playerCP = PhotonNetwork.PlayerListOthers[i-1].CustomProperties;
                    SetCharacterModel(playerCP, i);
                }
            }

            FadeManager.Instance.FadeIn();
            //로딩 프로세스 시작
            StartCoroutine(Co_LoadSceneProgress());
        }

        //로딩씬에 캐릭터 모델을 소환시킴
        private void SetCharacterModel(Hashtable customProperties, int index)
        {
            characterDatas.TryGetValue((string)customProperties[CustomPropertyKey.PlayerType], out GameObject model);
            Instantiate(model, characterSetPosition[index].position, characterSetPosition[index].rotation);
        }

        private void Update()
        {
            //마스터 클라이언트이고 로딩 카운트가 방에 들어온 플레이어 명수 와 같다면 씬을 로드시킴
            if (!once && PhotonNetwork.IsMasterClient && loadingfinshCount == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                photonView.RPC("LoadNextScene", RpcTarget.All);
                once = true;
            }
        }

        [PunRPC]
        public void LoadNextScene()
        {
            //씬을 넘어가게함
            FadeManager.Instance.FadeOut(()=> { ao.allowSceneActivation = true; });
        }

        [PunRPC]
        public void OnReady()
        {
            //로딩완료 카운트를 1더함
            loadingfinshCount++;
        }

        //로딩 프로세스
        private IEnumerator Co_LoadSceneProgress()
        {
            //비동기로 씬 로드
            ao = SceneManager.LoadSceneAsync(GameManager.Instance.NextScene);

            //씬을 바로 넘어가지 않게함
            ao.allowSceneActivation = false;

            //씬 비동기가 완료 될때까지 반복
            while (ao.progress < 0.9f)
            {
                yield return null;
            }

            //3초간 대기
            yield return new WaitForSeconds(3f);

            //로딩이 완료됐음을 알림
            photonView.RPC("OnReady", RpcTarget.MasterClient);
        }
    }
}

