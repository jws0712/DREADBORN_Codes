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
            
            //캐릭터 정보 딕셔너리 초기화
            foreach(var entry in characterEntries)
            {
                characterDatas[entry.type] = entry.model;
            }

            //기사 캐릭터를 처음에 보이게함
            ChooseCharacter(CharacterType.Knight);

            //버튼 리슨너 초기화
            knightButton.onClick.AddListener(() => { ChooseCharacter(CharacterType.Knight); });
            barbarianButton.onClick.AddListener( () => { ChooseCharacter(CharacterType.Barbarian); } );
            vanguardButton.onClick.AddListener( () => { ChooseCharacter(CharacterType.Vanguard); } );
            selectButton.onClick.AddListener(() => { Select(selectedCharacter); } );
        }

        //캐릭터 선택
        private void ChooseCharacter(CharacterType type)
        {
            //현재 선택되어있는 캐릭터 모델을 보이지 않게함
            if (currentSelectCharacterModel != null) currentSelectCharacterModel.SetActive(false);

            //선택한 캐릭터 타입으로 딕셔너리를 탐색해 선택한 캐릭터 모델을 가지고옴
            characterDatas.TryGetValue(type, out currentSelectCharacterModel);

            //가저온 모델을 보이게 함
            currentSelectCharacterModel.SetActive(true);

            //선택된 캐릭터의 이름을 할당시킴
            selectedCharacter = type;
        }

        //클래스 선택 확정
        private void Select(CharacterType className)
        {
            FadeManager.Instance.FadeOut(() => {
                //캐릭터 선택 화면를 보이지 않게함
                characterSelectCanvas.gameObject.SetActive(false);

                //선택한 캐릭터 이름을 GameManager에 넘겨주고 선택한 캐릭터 모델을 게임월드에 소환시킴
                GameManager.Instance.SetCharacter(className);
            });

        }
    }
}





