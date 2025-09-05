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

        //�ʱ�ȭ
        public void Initialize(Player player)
        {
            targetPlayer = player;

            //Ŀ���� ������Ƽ ����
            CP = targetPlayer.CustomProperties;

            //ĳ���� ������ ��ųʸ� �ʱ�ȭ
            foreach (var entry in characterIconEntries)
            {
                characterIcons[entry.type.ToString()] = entry.icon;
            }

            characterIcons.TryGetValue((CharacterType.None).ToString(), out Sprite iconSprite);
        }

        private void Update()
        {
            //Ŀ���� ������Ƽ�� ����Ǿ� ���� �ʴٸ� ������Ʈ ���� ����
            if (CP[PlayerType] == null || CP[CurrentHp] == null || CP[MaxHp] == null) return; 

            //�÷��̾� �̸� ǥ��
            nameText.text = (string)CP[PlayerType];

            //�÷��̾� ������ ǥ��
            if (characterIcons.TryGetValue((string)CP[PlayerType], out Sprite iconSprite))
            {
                icon.sprite = iconSprite;
            }

            //�÷��̾� ü�� ǥ��
            float currentHp = (float)CP[CurrentHp];
            float maxHp = (float)CP[MaxHp];
            hpBar.fillAmount = currentHp / maxHp;

            //�÷��̾� ������� ǥ��
            deadImage.SetActive((bool)CP[IsDead]);
        }


    }
}

