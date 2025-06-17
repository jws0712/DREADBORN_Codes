namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class ResetDead : StateMachineBehaviour
    {
        //애니매이션 클립에서 빠져나올때
        private PlayerManager manager;

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(manager == null)
            {
                manager = animator.GetComponentInParent<PlayerManager>();
            }

            manager.ResetDead();
        }
    }
}


