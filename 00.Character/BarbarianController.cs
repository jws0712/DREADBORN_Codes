namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project
    using static ObjectPoolObjectName;
    using Sound;

    public class BarbarianController : BaseCharacterController
    {
        [Header("Info")]
        [SerializeField] private float skillDamageMultiplier;
        [SerializeField] private Vector3 skillShakeVelocity;
        [SerializeField] private GameObject skillEffect;
        [Header("SFX")]
        [SerializeField] private AudioClip skillSFX;

        public override void Initialize()
        {
            base.Initialize();

            //이펙트 비활성화
            skillEffect.SetActive(false);
        }

        [PunRPC]
        public override void OnSkilled()
        {
            ObjectPoolManager.Instance.SpawnPoolObject(BerserkerRageEffect, transform.position, Quaternion.identity);

            if (manager.photonView.IsMine)
            {
                manager.ImpulseSource.GenerateImpulse(skillShakeVelocity);

                SoundManager.instance.SFXPlay("BarbarianSkill", skillSFX);

                //방어력을 모두 감소 시킴
                manager.IsDefend = true;
                manager.TakeDamage(manager.MaxDefensePoint);
                manager.IsDefend = false;
            }


            //이펙트 홞성화
            skillEffect.SetActive(true);

            //무기 데미지 셋팅
            manager.EquipmentManager.SetRightWeaponDamage(manager.EquipmentManager.RightWeapon.Damage * skillDamageMultiplier);
            manager.EquipmentManager.SetLeftWeaponDamage(manager.EquipmentManager.LeftWeapon.Damage * skillDamageMultiplier);
        }

        [PunRPC]
        public override void OnSkillEnded()
        {
            //이펙트 비활성화
            skillEffect.SetActive(false);

            //무기 데미지 초기화
            manager.EquipmentManager.SetRightWeaponDamage(manager.EquipmentManager.RightWeapon.Damage);
            manager.EquipmentManager.SetLeftWeaponDamage(manager.EquipmentManager.LeftWeapon.Damage);
        }
    }

}
