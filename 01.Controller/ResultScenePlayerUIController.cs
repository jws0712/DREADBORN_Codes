namespace DREADBORN
{
    //System
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;

    //TMP
    using TMPro;

    //Photon
    using Photon.Realtime;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    //Project
    using static CustomPropertyKey;


    public class ResultScenePlayerUIController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private List<CharacterIconEntry> characterIconEntries = new List<CharacterIconEntry>();
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image icon;

        private Hashtable CP;

        private Player targetPlayer;

        private Dictionary<string, Sprite> characterIcons = new();

        //초기화
        public void Initialize(Player player)
        {
            targetPlayer = player;

            //커스텀 프로퍼티 설정
            CP = targetPlayer.CustomProperties;

            //캐릭터 아이콘 딕셔너리 초기화
            foreach (var entry in characterIconEntries)
            {
                characterIcons[entry.type.ToString()] = entry.icon;
            }
        }

        private void Update()
        {
            //커스텀 프로퍼티가 저장되어 있지 않다면 업데이트 하지 않음
            if (CP[PlayerType] == null ||
                CP[Level] == null) return;

            //플레이어 레벨 표시
            levelText.text = ((int)CP[Level]).ToString();

            //플레이어 아이콘 표시
            if (characterIcons.TryGetValue((string)CP[PlayerType], out Sprite iconSprite))
            {
                icon.sprite = iconSprite;
            }
        }
    }
}

