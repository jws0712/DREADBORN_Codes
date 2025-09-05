namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class ResetDead : StateMachineBehaviour
    {
        //�ִϸ��̼� Ŭ������ �������ö�
        private CharacterManager manager;

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(manager == null)
            {
                manager = animator.GetComponentInParent<CharacterManager>();
            }

            manager.ResetDead();
        }
    }
}


