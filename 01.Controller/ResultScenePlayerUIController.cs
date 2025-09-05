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
        }

        private void Update()
        {
            //Ŀ���� ������Ƽ�� ����Ǿ� ���� �ʴٸ� ������Ʈ ���� ����
            if (CP[PlayerType] == null ||
                CP[Level] == null) return;

            //�÷��̾� ���� ǥ��
            levelText.text = ((int)CP[Level]).ToString();

            //�÷��̾� ������ ǥ��
            if (characterIcons.TryGetValue((string)CP[PlayerType], out Sprite iconSprite))
            {
                icon.sprite = iconSprite;
            }
        }
    }
}

