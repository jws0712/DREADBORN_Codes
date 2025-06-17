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

    public class LoadingManager : MonoBehaviourPun
    {
        [Header("Info")]
        [SerializeField] private List<ClassEntry> characterEntries = new List<ClassEntry>();
        [SerializeField] private Transform[] characterSetPosition;

        private int loadingfinshCount;

        private AsyncOperation ao;

        private bool once;

        private Dictionary<string, GameObject> characterDatas = new Dictionary<string, GameObject>();

        private void Start()
        {

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
                    characterDatas.TryGetValue((string)playerCP[SelectCharacterType], out GameObject model);
                    Instantiate(model, characterSetPosition[i].position, characterSetPosition[i].rotation);
                    Debug.Log("���� �÷��̾� ����");
                }
                //�� �� ��ġ�� �ٸ� �÷��̾� �� ����
                else
                {
                    Hashtable playerCP = PhotonNetwork.PlayerListOthers[i-1].CustomProperties;
                    characterDatas.TryGetValue((string)playerCP[SelectCharacterType], out GameObject model);
                    Instantiate(model, characterSetPosition[i].position, characterSetPosition[i].rotation);
                    Debug.Log("�ٸ� �÷��̾� ����");
                }
            }

            FadeManager.Instance.FadeIn();
            //�ε� ���μ��� ����
            StartCoroutine(Co_LoadSceneProgress());
        }

        private void Update()
        {
            //������ Ŭ���̾�Ʈ�̰� �ε� ī��Ʈ�� �濡 ���� �÷��̾� ��� �� ���ٸ� ���� �ѹ� �ε��Ŵ
            if (!once && PhotonNetwork.IsMasterClient && loadingfinshCount == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                photonView.RPC("LoadNextScene", RpcTarget.All);
                once = true;
            }
        }

        [PunRPC]
        private void LoadNextScene()
        {
            //���� �Ѿ����
            FadeManager.Instance.FadeOut(()=> { ao.allowSceneActivation = true; });
        }

        [PunRPC]
        private void OnReady()
        {
            //�ε��Ϸ� ī��Ʈ�� 1����
            loadingfinshCount++;
        }

        //�ε� ���μ���
        private IEnumerator Co_LoadSceneProgress()
        {
            //�񵿱�� �� �ε�
            ao = SceneManager.LoadSceneAsync(Stage1);

            //���� �ٷ� �Ѿ�� �ʰ���
            ao.allowSceneActivation = false;

            //�� �񵿱Ⱑ �Ϸ� �ɶ����� �ݺ�
            while(ao.progress < 0.9f)
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

