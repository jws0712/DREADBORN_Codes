namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;


    [CreateAssetMenu(fileName = "WeaponItemData", menuName = "Inventory/WeaponItemData")]
    public class WeaponItemData : ItemData
    {
        [Header("WeaponItem Information")]
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private AnimatorOverrideController thirdPersonWeaponAnimator;
        [SerializeField] private GameObject weaponPrefab;
        [Space(5)]
        [SerializeField] private float damage;
        //[SerializeField] private float attackSpeed = default;
        

        public float Damage => damage;

        public GameObject WeaponPrefab => weaponPrefab;
        public AnimatorOverrideController ThirdPersonWeaponAnimator => thirdPersonWeaponAnimator;

        public WeaponType Type => weaponType;

    }
}