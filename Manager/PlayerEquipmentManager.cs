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

        //초기화
        public void Initialize()
        {
            manager = GetComponentInParent<PlayerManager>();

            SetAllWeapon();
            GetAllWeaponCollider();
        }

        //무기를 사용하는 애니매이션을 캐릭터 애니매이터에 셋팅
        private void SetWeaponAnimator()
        {
            if(playerAnimatorOverride != null)
            {
                manager.ThirdPersonAnim.runtimeAnimatorController = playerAnimatorOverride;
            }
        }

        //무기의 데미지 셋팅
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

        //무기 셋팅
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

        //무기의 DamageCollider를 가져옴
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

        //무기의 Collider를 끄고 킴
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

        //무기의 콤보여부를 체크
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