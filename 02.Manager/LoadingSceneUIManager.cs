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
            SetNewLoadingText("게임 불러오는 중...");
            StartCoroutine(Co_UpdateLoadingText());
        }

        //로딩 텍스트를 변경함
        public void SetNewLoadingText(string text)
        {
            loadingText = text;
        }

        //일정 시간마다 한 글자씩 출력함
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


