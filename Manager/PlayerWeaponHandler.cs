namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class PlayerWeaponHandler : MonoBehaviour
    {
        //�÷��̾ ���� ��� �ִ� ������ ������ ����
        [Header("Info")]
        [SerializeField] private WeaponItemData leftWeapon = null;
        [SerializeField] private WeaponItemData rightWeapon = null;

        public WeaponItemData LeftWeapon => leftWeapon;
        public WeaponItemData RightWeapon => rightWeapon;
    }
}

