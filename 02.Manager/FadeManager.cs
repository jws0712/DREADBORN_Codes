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

        //���̵� ��
        public void FadeIn(Action action = null)
        {
            StartCoroutine(Co_Fade(FadeType.In, action));
        }

        //���̵� �ƿ�
        public void FadeOut(Action action = null)
        {
            StartCoroutine(Co_Fade(FadeType.Out, action));
        }

        IEnumerator Co_Fade(FadeType type, Action action)
        {
            //���̵� �� ����
            if(type == FadeType.In)
            {
                anim.SetTrigger(animFadeInID);
            }

            //���̵� �ƿ� ����
            else
            {
                anim.SetTrigger(animFadeOutID);
            }

            if(action != null)
            {
                //���̵� �ð� ��ŭ ���
                yield return new WaitForSeconds(fadeTime);

                //���̵尡 ������ ���� action ����
                action();
            }

        }
    }
}


