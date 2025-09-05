namespace DREADBORN
{
    //System
    using System;
    using System.Collections;

    //UnityEngine
    using UnityEngine;

    //Project
    using static AnimatorParameter;

    public class FadeManager : Singleton<FadeManager>
    {
        [Header("Info")]
        [SerializeField] private float fadeTime;

        private Animator anim;

        private int animFadeInID;
        private int animFadeOutID;

        public override void Awake()
        {
            base.Awake();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            animFadeInID = Animator.StringToHash(In);
            animFadeOutID =  Animator.StringToHash(Out);
        }

        //페이드 인
        public void FadeIn(Action action = null)
        {
            StartCoroutine(Co_Fade(FadeType.In, action));
        }

        //페이드 아웃
        public void FadeOut(Action action = null)
        {
            StartCoroutine(Co_Fade(FadeType.Out, action));
        }

        IEnumerator Co_Fade(FadeType type, Action action)
        {
            //페이드 인 실행
            if(type == FadeType.In)
            {
                anim.SetTrigger(animFadeInID);
            }

            //페이드 아웃 실행
            else
            {
                anim.SetTrigger(animFadeOutID);
            }

            if(action != null)
            {
                //페이드 시간 만큼 대기
                yield return new WaitForSeconds(fadeTime);

                //페이드가 끝난후 받은 action 실행
                action();
            }

        }
    }
}


