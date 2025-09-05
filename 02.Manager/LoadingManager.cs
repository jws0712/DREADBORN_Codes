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

            //ĳ���� ���� ��ųʸ� �ʱ�ȭ
            foreach (var entry in characterEntries)
            {
                characterDatas[entry.type.ToString()] = entry.model;
            }

            //���� ������ �÷��̾� ��� ��ŭ �ݺ�
            for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                //ó�� ��ġ���� LocalPlayer �� ����
                if(i == 0)
                {
                    Hashtable playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
                    SetCharacterModel(playerCP, i);
                }
                //�� �� ��ġ�� �ٸ� �÷��̾� �� ����
                else
                {
                    Hashtable playerCP = PhotonNetwork.PlayerListOthers[i-1].CustomProperties;
                    SetCharacterModel(playerCP, i);
                }
            }

            FadeManager.Instance.FadeIn();
            //�ε� ���μ��� ����
            StartCoroutine(Co_LoadSceneProgress());
        }

        //�ε����� ĳ���� ���� ��ȯ��Ŵ
        private void SetCharacterModel(Hashtable customProperties, int index)
        {
            characterDatas.TryGetValue((string)customProperties[CustomPropertyKey.PlayerType], out GameObject model);
            Instantiate(model, characterSetPosition[index].position, characterSetPosition[index].rotation);
        }

        private void Update()
        {
            //������ Ŭ���̾�Ʈ�̰� �ε� ī��Ʈ�� �濡 ���� �÷��̾� ��� �� ���ٸ� ���� �ε��Ŵ
            if (!once && PhotonNetwork.IsMasterClient && loadingfinshCount == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                photonView.RPC("LoadNextScene", RpcTarget.All);
                once = true;
            }
        }

        [PunRPC]
        public void LoadNextScene()
        {
            //���� �Ѿ����
            FadeManager.Instance.FadeOut(()=> { ao.allowSceneActivation = true; });
        }

        [PunRPC]
        public void OnReady()
        {
            //�ε��Ϸ� ī��Ʈ�� 1����
            loadingfinshCount++;
        }

        //�ε� ���μ���
        private IEnumerator Co_LoadSceneProgress()
        {
            //�񵿱�� �� �ε�
            ao = SceneManager.LoadSceneAsync(GameManager.Instance.NextScene);

            //���� �ٷ� �Ѿ�� �ʰ���
            ao.allowSceneActivation = false;

            //�� �񵿱Ⱑ �Ϸ� �ɶ����� �ݺ�
            while (ao.progress < 0.9f)
            {
                yield return null;
            }

            //3�ʰ� ���
            yield return new WaitForSeconds(3f);

            //�ε��� �Ϸ������ �˸�
            photonView.RPC("OnReady", RpcTarget.MasterClient);
        }
    }
}

