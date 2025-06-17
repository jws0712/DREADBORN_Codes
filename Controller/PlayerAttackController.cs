namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;
    using UnityEngine.EventSystems;

    //Project
    using static AnimationClipName;

    public class PlayerAttackController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private float chargeAttackTime;

        private float currentMouseHoldTime;

        private string lastAttack;

        private WeaponType currentWeaponType;

        private PlayerManager manager;

        //�ʱ�ȭ
        public void Initialize()
        {
            manager = GetComponent<PlayerManager>();
            SetWeaponType();
        }

        public void SetWeaponType()
        {
            currentWeaponType = manager.Inventroy.RightWeapon.Type;
        }

        //���� ����
        public void AttackControl()
        {
            switch(currentWeaponType)
            {
                //���� ���� ����
                case WeaponType.MeleeWeapon:
                    {
                        MeleeWeaponAttack();
                        break;
                    }
                //���� �ٸ� Ÿ���� ���� �߰�

            }
        }

        //���� ���� ���� ����
        private void MeleeWeaponAttack()
        {
            if (manager.Input.IsPressAttack && !manager.PlayerController.IsDefend && !manager.isPause)
            {
                Debug.Log("����Ű ����");
                //����Ű�� ������ �ִ� �ð��� ���
                currentMouseHoldTime += Time.deltaTime;
            }
            else
            {
                //����Ű�� �ѹ��̶� ��������
                if (currentMouseHoldTime == 0) return;

                //������ �ð��� ��¡ ���� �ð����� ������
                if (currentMouseHoldTime < chargeAttackTime)
                {
                    //�ð� �ʱ�ȭ
                    currentMouseHoldTime = 0;

                    //�޺� ��Ȳ�϶�
                    if (manager.canCombo)
                    {
                        LightAttackCombo();
                    }
                    else
                    {
                        //�޺� ���� �ƴϰ� �ִϸ��̼��� �������� �ƴҶ�
                        if (manager.isAction) return;
                        if (manager.canCombo) return;
                        Attack(Light_Attack1);
                    }
                }
                //��¡ ����
                else
                {
                    //�ð� �ʱ�ȭ
                    currentMouseHoldTime = 0;
                }
            }
        }

        //���� �޺� ����
        private void LightAttackCombo()
        {
            manager.canCombo = false;

            switch(lastAttack)
            {
                case Light_Attack1:
                    {
                        Attack(Light_Attack2);
                        break;
                    }
                case Light_Attack2:
                    {
                        Attack(Light_Attack3);
                        break;
                    }
                case Light_Attack3:
                    {
                        Attack(Light_Attack1);
                        break;
                    }
            }
        }

        //���� ����
        private void Attack(string currentAttack)
        {
            manager.AnimController.PlayAnimation(currentAttack, true);
            lastAttack = currentAttack;
        }
    }
}