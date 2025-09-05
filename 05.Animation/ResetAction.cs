namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class ResetAction : StateMachineBehaviour
    {
        private CharacterManager manager;

        //�ִϸ��̼� Ŭ���� ����ɶ�
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(manager == null)
            {
                manager = animator.GetComponentInParent<CharacterManager>();
            }

            //������ �ൿ���¿� �޺����¸� �ʱ�ȭ��
            manager.CanDoCombo = false;
            manager.IsAction = false;
        }
    }
}