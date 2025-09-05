namespace DREADBORN
{
    //System
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    //Project
    using static CustomPropertyKey;
    using Sound;

    public class ResultManager : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private List<CharacterModelEntry> characterEntries = new List<CharacterModelEntry>();
        [SerializeField] private Transform[] characterSetPosition;
        [SerializeField] private AudioClip bg;


        private Dictionary<string, GameObject> characterDatas = new();

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            SoundManager.instance.BgSoundPlay(bg);
            NetworkManager.Instance.SetNewAutomaticallySyncSceneState(false);
            FadeManager.Instance.FadeIn();

            //딕셔너리 초기화
            foreach (var entry in characterEntries)
            {
                characterDatas[entry.type.ToString()] = entry.model;
            }

            //현재 입장한 플레이어 명수 만큼 반복
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                Hashtable playerCP = PhotonNetwork.PlayerList[i].CustomProperties;
                SetCharacterModel(playerCP, i);
            }
        }

        //플레이어 모델 소환
        private void SetCharacterModel(Hashtable customProperties, int index)
        {
            characterDatas.TryGetValue((string)customProperties[CustomPropertyKey.PlayerType], out GameObject model);
            Instantiate(model, characterSetPosition[index].position, characterSetPosition[index].rotation);
        }
    }
}

