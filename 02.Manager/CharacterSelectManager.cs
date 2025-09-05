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
            
            //캐릭터 정보 딕셔너리 초기화
            foreach(var entry in characterEntries)
            {
                characterModels[entry.type.ToString()] = entry.model;
            }

            //기사 캐릭터로 초기화
            ChooseCharacter(CharacterType.Knight);

            //버튼 초기화
            knightButton.onClick.AddListener(() => { ChooseCharacter(CharacterType.Knight); });
            barbarianButton.onClick.AddListener( () => { ChooseCharacter(CharacterType.Barbarian); } );
            vanguardButton.onClick.AddListener( () => { ChooseCharacter(CharacterType.Vanguard); } );
            CannondButton.onClick.AddListener( () => { ChooseCharacter(CharacterType.Hammer); } );
            selectButton.onClick.AddListener(() => { Select(selectedCharacter); } );
        }

        //캐릭터 선택
        private void ChooseCharacter(CharacterType type)
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            //선택한 캐릭터 타입으로 딕셔너리를 탐색해 선택한 캐릭터 모델을 가지고옴
            if(characterModels.TryGetValue(type.ToString(), out currentSelectCharacterModel))
            {
                //현재 선택되어있는 캐릭터 모델을 보이지 않게함
                if (lastSelectCharacterMode != null && lastSelectCharacterMode != currentSelectCharacterModel) lastSelectCharacterMode.SetActive(false);

                //가저온 모델을 보이게 함
                currentSelectCharacterModel.SetActive(true);

                lastSelectCharacterMode = currentSelectCharacterModel;

                //선택된 캐릭터의 타입을 할당
                selectedCharacter = type;
            }
        }

        //클래스 선택 확정
        private void Select(CharacterType className)
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            FadeManager.Instance.FadeOut(() => {

                //설명 UI를 보이지 않게함
                lastSelectCharacterMode.SetActive(false);

                //캐릭터 선택 화면를 보이지 않게함
                characterSelectCanvas.gameObject.SetActive(false);

                //선택한 캐릭터 이름을 GameManager에 넘겨주고 선택한 캐릭터 모델을 게임월드에 소환시킴
                GameManager.Instance.SetCharacter(className);
            });
        }
    }
}





