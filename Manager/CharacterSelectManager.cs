namespace DREADBORN
{
    //System
    using System;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.Rendering.Universal;
    using UnityEngine.UI;

    public class CharacterSelectManager : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private List<ClassEntry> characterEntries = new List<ClassEntry>();

        [Space(10)]
        [Header("UI")]
        [SerializeField] private Button barbarianButton;
        [SerializeField] private Button knightButton;
        [SerializeField] private Button vanguardButton;
        [SerializeField] private Button selectButton;
        [Space(10)]
        [SerializeField] private Canvas characterSelectCanvas;


        private CharacterType selectedCharacter;
        private GameObject currentSelectCharacterModel;
        private Dictionary<CharacterType, GameObject> characterDatas = new Dictionary<CharacterType, GameObject>();

        private void Start()
        {
            FadeManager.Instance.FadeIn();
            
            //ĳ���� ���� ��ųʸ� �ʱ�ȭ
            foreach(var entry in characterEntries)
            {
                characterDatas[entry.type] = entry.model;
            }

            //��� ĳ���͸� ó���� ���̰���
            ChooseCharacter(CharacterType.Knight);

            //��ư ������ �ʱ�ȭ
            knightButton.onClick.AddListener(() => { ChooseCharacter(CharacterType.Knight); });
            barbarianButton.onClick.AddListener( () => { ChooseCharacter(CharacterType.Barbarian); } );
            vanguardButton.onClick.AddListener( () => { ChooseCharacter(CharacterType.Vanguard); } );
            selectButton.onClick.AddListener(() => { Select(selectedCharacter); } );
        }

        //ĳ���� ����
        private void ChooseCharacter(CharacterType type)
        {
            //���� ���õǾ��ִ� ĳ���� ���� ������ �ʰ���
            if (currentSelectCharacterModel != null) currentSelectCharacterModel.SetActive(false);

            //������ ĳ���� Ÿ������ ��ųʸ��� Ž���� ������ ĳ���� ���� �������
            characterDatas.TryGetValue(type, out currentSelectCharacterModel);

            //������ ���� ���̰� ��
            currentSelectCharacterModel.SetActive(true);

            //���õ� ĳ������ �̸��� �Ҵ��Ŵ
            selectedCharacter = type;
        }

        //Ŭ���� ���� Ȯ��
        private void Select(CharacterType className)
        {
            FadeManager.Instance.FadeOut(() => {
                //ĳ���� ���� ȭ�鸦 ������ �ʰ���
                characterSelectCanvas.gameObject.SetActive(false);

                //������ ĳ���� �̸��� GameManager�� �Ѱ��ְ� ������ ĳ���� ���� ���ӿ��忡 ��ȯ��Ŵ
                GameManager.Instance.SetCharacter(className);
            });

        }
    }
}





