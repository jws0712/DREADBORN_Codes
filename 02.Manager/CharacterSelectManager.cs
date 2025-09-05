namespace DREADBORN
{
    using Sound;
    //System
    using System;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;

    public class CharacterSelectManager : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private List<CharacterModelEntry> characterEntries = new List<CharacterModelEntry>();


        [Space(10)]
        [Header("UI")]
        [SerializeField] private Button barbarianButton;
        [SerializeField] private Button knightButton;
        [SerializeField] private Button vanguardButton;
        [SerializeField] private Button CannondButton;
        [SerializeField] private Button selectButton;
        [Space(10)]
        [SerializeField] private Canvas characterSelectCanvas;

        [Header("SFX")]
        [SerializeField] private AudioClip buttonClickSFX;


        private CharacterType selectedCharacter;
        private GameObject currentSelectCharacterModel;
        private GameObject lastSelectCharacterMode;
        private Dictionary<string, GameObject> characterModels = new Dictionary<string, GameObject>();

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            FadeManager.Instance.FadeIn();
            
            //ĳ���� ���� ��ųʸ� �ʱ�ȭ
            foreach(var entry in characterEntries)
            {
                characterModels[entry.type.ToString()] = entry.model;
            }

            //��� ĳ���ͷ� �ʱ�ȭ
            ChooseCharacter(CharacterType.Knight);

            //��ư �ʱ�ȭ
            knightButton.onClick.AddListener(() => { ChooseCharacter(CharacterType.Knight); });
            barbarianButton.onClick.AddListener( () => { ChooseCharacter(CharacterType.Barbarian); } );
            vanguardButton.onClick.AddListener( () => { ChooseCharacter(CharacterType.Vanguard); } );
            CannondButton.onClick.AddListener( () => { ChooseCharacter(CharacterType.Hammer); } );
            selectButton.onClick.AddListener(() => { Select(selectedCharacter); } );
        }

        //ĳ���� ����
        private void ChooseCharacter(CharacterType type)
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            //������ ĳ���� Ÿ������ ��ųʸ��� Ž���� ������ ĳ���� ���� �������
            if(characterModels.TryGetValue(type.ToString(), out currentSelectCharacterModel))
            {
                //���� ���õǾ��ִ� ĳ���� ���� ������ �ʰ���
                if (lastSelectCharacterMode != null && lastSelectCharacterMode != currentSelectCharacterModel) lastSelectCharacterMode.SetActive(false);

                //������ ���� ���̰� ��
                currentSelectCharacterModel.SetActive(true);

                lastSelectCharacterMode = currentSelectCharacterModel;

                //���õ� ĳ������ Ÿ���� �Ҵ�
                selectedCharacter = type;
            }
        }

        //Ŭ���� ���� Ȯ��
        private void Select(CharacterType className)
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            FadeManager.Instance.FadeOut(() => {

                //���� UI�� ������ �ʰ���
                lastSelectCharacterMode.SetActive(false);

                //ĳ���� ���� ȭ�鸦 ������ �ʰ���
                characterSelectCanvas.gameObject.SetActive(false);

                //������ ĳ���� �̸��� GameManager�� �Ѱ��ְ� ������ ĳ���� ���� ���ӿ��忡 ��ȯ��Ŵ
                GameManager.Instance.SetCharacter(className);
            });
        }
    }
}





