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
    using Photon.Pun;
    using Photon.Realtime;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    //Project
    using static CustomPropertyKey;

    public class OtherPlayerUIController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private List<CharacterIconEntry> characterIconEntries = new List<CharacterIconEntry>();
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image icon;
        [SerializeField] private Image hpBar;
        [SerializeField] private GameObject deadImage;

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

            characterIcons.TryGetValue((CharacterType.None).ToString(), out Sprite iconSprite);
        }

        private void Update()
        {
            //커스텀 프로퍼티가 저장되어 있지 않다면 업데이트 하지 않음
            if (CP[PlayerType] == null || CP[CurrentHp] == null || CP[MaxHp] == null) return; 

            //플레이어 이름 표시
            nameText.text = (string)CP[PlayerType];

            //플레이어 아이콘 표시
            if (characterIcons.TryGetValue((string)CP[PlayerType], out Sprite iconSprite))
            {
                icon.sprite = iconSprite;
            }

            //플레이어 체력 표시
            float currentHp = (float)CP[CurrentHp];
            float maxHp = (float)CP[MaxHp];
            hpBar.fillAmount = currentHp / maxHp;

            //플레이어 사망상태 표시
            deadImage.SetActive((bool)CP[IsDead]);
        }


    }
}

