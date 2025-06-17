namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class EquipmentHandler : MonoBehaviour
    {
        private GameObject currentWeapon;

        //현재 들고 있는 무기를 셋팅
        public void SetWeapon(GameObject weaponObject)
        {
            currentWeapon = weaponObject;
            weaponObject.transform.parent = transform;

            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;

        }
    }
}