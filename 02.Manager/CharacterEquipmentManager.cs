namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class CharacterEquipmentManager : MonoBehaviour
    {
        [Header("Weapon")]
        [SerializeField] private WeaponItemData rightWeapon;
        [SerializeField] private WeaponItemData leftWeapon;

        [Header("Setting")]
        [SerializeField] private EquipmentHandler rightEquipmentHandler;
        [SerializeField] private EquipmentHandler leftEquipmentHandler;

        private GameObject rightWeaponObject;
        private GameObject leftWeaponObject;

        private CharacterManager manager;

        private DamageCollider rightWeaponCollider;
        private DamageCollider leftWeaponCollider;


        public WeaponItemData RightWeapon => rightWeapon;
        public WeaponItemData LeftWeapon => leftWeapon;

        //�ʱ�ȭ
        public void Initialize()
        {
            manager = GetComponentInParent<CharacterManager>();

            SetAllWeapon();
            GetAllWeaponCollider();
        }

        public void ChangeLightWeapon()
        {
            Destroy(rightWeaponObject);
            SetAllWeapon();
            GetAllWeaponCollider();
        }

        public void ChangeLeftWeapon()
        {
            Destroy(leftWeaponObject);
            SetAllWeapon();
            GetAllWeaponCollider();
        }

        //������ ������ ����
        #region SetWeaponDamage
        public void SetRightWeaponDamage(float damage)
        {
            if(rightWeaponCollider != null)
            {
                rightWeaponCollider.Damage = damage;
            }
        }

        public void SetLeftWeaponDamage(float damage)
        {
            if (leftWeaponCollider != null)
            {
                leftWeaponCollider.Damage = damage;
            }
        }
        #endregion

        //���� ����
        #region SetWeapon
        private void SetAllWeapon()
        {
            SetRightWeapon();
            SetLeftWeapon();
        }

        private void SetRightWeapon()
        {
            if (manager.EquipmentManager.RightWeapon != null)
            {
                rightWeaponObject = Instantiate(manager.EquipmentManager.RightWeapon.WeaponPrefab);
                rightEquipmentHandler.SetWeapon(rightWeaponObject);

                manager.Animator.runtimeAnimatorController = manager.EquipmentManager.RightWeapon.ThirdPersonWeaponAnimator;
            }
        }

        private void SetLeftWeapon()
        {
            if (manager.EquipmentManager.LeftWeapon != null)
            {
                leftWeaponObject = Instantiate(manager.EquipmentManager.LeftWeapon.WeaponPrefab);
                leftEquipmentHandler.SetWeapon(leftWeaponObject);
            }
        }
        #endregion

        //������ DamageCollider�� ������
        #region GetWeaponCollider
        private void GetAllWeaponCollider()
        {
            GetRightWeaponCollider();
            GetLeftWeaponCollider();
        }

        private void GetRightWeaponCollider()
        {
            if(rightWeaponObject != null)
            {
                rightWeaponCollider = rightWeaponObject.GetComponentInChildren<DamageCollider>();
                SetRightWeaponDamage(manager.EquipmentManager.RightWeapon.Damage);
            }
        }

        private void GetLeftWeaponCollider()
        {
            if (leftWeaponObject != null)
            {
                leftWeaponCollider = leftWeaponObject.GetComponentInChildren<DamageCollider>();
                SetLeftWeaponDamage(manager.EquipmentManager.LeftWeapon.Damage);
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
            manager.CanDoCombo = true;
        }

        public void DisableCombo()
        {
            manager.CanDoCombo = false;
        }
        #endregion
    }
}