namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class ResetAction : StateMachineBehaviour
    {
        private PlayerManager manager;

        //�ִϸ��̼� Ŭ���� ����ɶ�
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(manager == null)
            {
                manager = animator.GetComponentInParent<PlayerManager>();
            }

            //������ �ൿ���¿� �޺����¸� �ʱ�ȭ��
            manager.canCombo = false;
            manager.isAction = false;
        }
    }
}