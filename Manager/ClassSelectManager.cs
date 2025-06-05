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

            //��� Ŭ������ ó���� ���̰���
            ChooseClass(Knight);

            knightButton.onClick.AddListener(() => { ChooseClass(Knight); });
            barbarianButton.onClick.AddListener( () => { ChooseClass(Barbarian); } );
            vanguardButton.onClick.AddListener( () => { ChooseClass(Vanguard); } );
            selectButton.onClick.AddListener(() => { Select(selectedClass); });

        }

        //Ŭ���� ����
        private void ChooseClass(string className)
        {
            //���� ���õǾ��ִ� Ŭ���� ���� ������ �ʰ���
            if (currentSelectClassModel != null) currentSelectClassModel.SetActive(false);

            //������ Ŭ������ �̸����� ��ųʸ��� Ž���� ������ Ŭ������ ���� �������
            classDatas.TryGetValue(className, out currentSelectClassModel);

            //������ ���� ���̰� ��
            currentSelectClassModel.SetActive(true);

            //���õ� Ŭ������ �̸��� �Ҵ��Ŵ
            selectedClass = className;
        }

        //Ŭ���� ���� Ȯ��
        private void Select(string className)
        {
            //Ŭ���� ���ø� ������ �ʰ���
            classSelectCanvas.gameObject.SetActive(false);

            //������ Ŭ������ �̸��� GameManager�� �Ѱ��ְ� ������ Ŭ������ ���� ���ӿ��忡 ��ȯ��Ŵ
            GameManager.Instance.SetClass(className);
            GameManager.Instance.SpawnPlayer(className);
        }
    }
}





