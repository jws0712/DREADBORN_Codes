namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class PlayerEquipmentManager : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private EquipmentHandler rightEquipmentHandler = null;
        [SerializeField] private EquipmentHandler leftEquipmentHandler = null;

        private GameObject rightWeaponObject = null;
        private GameObject leftWeaponObject = null;

        private PlayerManager manager = null;

        private DamageCollider rightWeaponCollider = null;
        private DamageCollider leftWeaponCollider = null;

        private AnimatorOverrideController playerAnimatorOverride = null;

        //�ʱ�ȭ
        public void Initialize()
        {
            manager = GetComponentInParent<PlayerManager>();

            SetAllWeapon();
            GetAllWeaponCollider();
        }

        //���⸦ ����ϴ� �ִϸ��̼��� ĳ���� �ִϸ����Ϳ� ����
        private void SetWeaponAnimator()
        {
            if(playerAnimatorOverride != null)
            {
                manager.ThirdPersonAnim.runtimeAnimatorController = playerAnimatorOverride;
            }
        }

        //������ ������ ����
        #region SetWeaponDamage
        private void SetRightWeaponDamage()
        {
            if(rightWeaponCollider != null)
            {
                rightWeaponCollider.Damage = manager.Inventroy.RightWeapon.Damage;
            }
        }

        private void SetLeftWeaponDamage()
        {
            if (leftWeaponCollider != null)
            {
                leftWeaponCollider.Damage = manager.Inventroy.LeftWeapon.Damage;
            }
        }
        #endregion

        //���� ����
        #region SetWeapon
        private void SetAllWeapon()
        {
            SetRightWeapon();
            SetLeftWeapon();
            SetWeaponAnimator();
        }

        private void SetRightWeapon()
        {
            if (manager.Inventroy.RightWeapon != null)
            {
                rightWeaponObject = Instantiate(manager.Inventroy.RightWeapon.WeaponPrefab);
                rightEquipmentHandler.SetWeapon(rightWeaponObject);

                if (playerAnimatorOverride == null)
                {
                    playerAnimatorOverride = manager.Inventroy.RightWeapon.ThirdPersonWeaponAnimator;
                }
            }
        }

        private void SetLeftWeapon()
        {
            if (manager.Inventroy.LeftWeapon != null)
            {
                leftWeaponObject = Instantiate(manager.Inventroy.LeftWeapon.WeaponPrefab);
                leftEquipmentHandler.SetWeapon(leftWeaponObject);

                if (playerAnimatorOverride == null)
                {
                    playerAnimatorOverride = manager.Inventroy.LeftWeapon.ThirdPersonWeaponAnimator;
                }
            }
        }
        #endregion

        //������ DamageCollider�� ������
        #region GetWeaponCollider
        public void GetAllWeaponCollider()
        {
            GetRightWeaponCollider();
            GetLeftWeaponCollider();
        }

        private void GetRightWeaponCollider()
        {
            if(rightWeaponObject != null)
            {
                rightWeaponCollider = rightWeaponObject.GetComponentInChildren<DamageCollider>();
                SetRightWeaponDamage();
            }
        }

        private void GetLeftWeaponCollider()
        {
            if (leftWeaponObject != null)
            {
                leftWeaponCollider = leftWeaponObject.GetComponentInChildren<DamageCollider>();
                SetLeftWeaponDamage();
            }
        }
        #endregion

        //������ Collider�� ���� Ŵ
        #region CheckWeaponAttack
        public void StartRightWeaponAttack()
        {
            if (rightWeaponCollider == null) return;
            rightWeaponCollider.EnableDamageCollider();
        }

        public void EndRightWeaponAttack()
        {
            if (rightWeaponCollider == null) return;
            rightWeaponCollider.DisableDamageCollider();
        }

        public void StartLeftWeaponAttack()
        {
            if (leftWeaponCollider == null) return;
            leftWeaponCollider.EnableDamageCollider();
        }

        public void EndLeftWeaponAttack()
        {
            if (leftWeaponCollider == null) return;
            leftWeaponCollider.DisableDamageCollider();
        }
        #endregion

        //������ �޺����θ� üũ
        #region CheckCombo
        public void EnableCombo()
        {
            manager.canCombo = true;
        }

        public void DisableCombo()
        {
            manager.canCombo = false;
        }
        #endregion
    }
}