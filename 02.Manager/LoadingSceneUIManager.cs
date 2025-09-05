namespace DREADBORN
{
    //System
    using System;
    using System.Collections;
    using System.Collections.Generic;
    
    //UnityEngine
    using UnityEngine;

    //TMP
    using TMPro;

    public class LoadingSceneUIManager : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float typeSpeed;

        private string loadingText;

        private void Start()
        {
            SetNewLoadingText("���� �ҷ����� ��...");
            StartCoroutine(Co_UpdateLoadingText());
        }

        //�ε� �ؽ�Ʈ�� ������
        public void SetNewLoadingText(string text)
        {
            loadingText = text;
        }

        //���� �ð����� �� ���ھ� �����
        private IEnumerator Co_UpdateLoadingText()
        {
            while(true)
            {
                text.text = "";

                for (int i = 0; i < loadingText.Length; i++)
                {
                    text.text = loadingText.Substring(0, i);
                    yield return new WaitForSeconds(typeSpeed);
                }
            }
        }
    }
}


