namespace DREADBORN
{
    using System;
    using System.Collections.Generic;
    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;

    //Project
    using static ClassName;

    public class ClassSelectManager : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private GameObject knightModel = null;
        [SerializeField] private GameObject barbarianModel = null;
        [SerializeField] private GameObject vanguardModel = null;

        [Space(10)]
        [Header("UI")]
        [SerializeField] private Button barbarianButton = null;
        [SerializeField] private Button knightButton = null;
        [SerializeField] private Button vanguardButton = null;
        [SerializeField] private Button selectButton = null;
        [SerializeField] private Canvas classSelectCanvas = null;

        private string selectedClass = null;
        private GameObject currentSelectClassModel = null;
        private Dictionary<string, GameObject> classDatas = new Dictionary<string, GameObject>();

        private void Start()
        {
            classDatas.Add(Knight, knightModel);
            classDatas.Add(Vanguard, barbarianModel);
            classDatas.Add(Barbarian, vanguardModel);

            //기사 클래스를 처음에 보이게함
            ChooseClass(Knight);

            knightButton.onClick.AddListener(() => { ChooseClass(Knight); });
            barbarianButton.onClick.AddListener( () => { ChooseClass(Barbarian); } );
            vanguardButton.onClick.AddListener( () => { ChooseClass(Vanguard); } );
            selectButton.onClick.AddListener(() => { Select(selectedClass); });

        }

        //클래스 선택
        private void ChooseClass(string className)
        {
            //현재 선택되어있는 클래스 모델을 보이지 않게함
            if (currentSelectClassModel != null) currentSelectClassModel.SetActive(false);

            //선택한 클래스의 이름으로 딕셔너리를 탐색해 선택한 클래스의 모델을 가지고옴
            classDatas.TryGetValue(className, out currentSelectClassModel);

            //가저온 모델을 보이게 함
            currentSelectClassModel.SetActive(true);

            //선택된 클래스의 이름을 할당시킴
            selectedClass = className;
        }

        //클래스 선택 확정
        private void Select(string className)
        {
            //클래스 선택를 보이지 않게함
            classSelectCanvas.gameObject.SetActive(false);

            //선택한 클래스의 이름을 GameManager에 넘겨주고 선택한 클래스의 모델을 게임월드에 소환시킴
            GameManager.Instance.SetClass(className);
            GameManager.Instance.SpawnPlayer(className);
        }
    }
}





