namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Project
    using static AnimatorParameter;

    public class ResetFade : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger(In);
            animator.ResetTrigger(Out);
        }
    }
}
