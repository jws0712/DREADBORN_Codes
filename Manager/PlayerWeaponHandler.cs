namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class PlayerWeaponHandler : MonoBehaviour
    {
        //플레이어가 현재 들고 있는 무기의 정보를 저장
        [Header("Info")]
        [SerializeField] private WeaponItemData leftWeapon = null;
        [SerializeField] private WeaponItemData rightWeapon = null;

        public WeaponItemData LeftWeapon => leftWeapon;
        public WeaponItemData RightWeapon => rightWeapon;
    }
}

