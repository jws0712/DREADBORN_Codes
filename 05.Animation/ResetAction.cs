namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class ResetAction : StateMachineBehaviour
    {
        private CharacterManager manager;

        //애니매이션 클립이 실행될때
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(manager == null)
            {
                manager = animator.GetComponentInParent<CharacterManager>();
            }

            //무기의 행동상태와 콤보상태를 초기화함
            manager.CanDoCombo = false;
            manager.IsAction = false;
        }
    }
}