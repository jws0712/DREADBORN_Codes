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

            //����Ʈ ��Ȱ��ȭ
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

                //������ ��� ���� ��Ŵ
                manager.IsDefend = true;
                manager.TakeDamage(manager.MaxDefensePoint);
                manager.IsDefend = false;
            }


            //����Ʈ �W��ȭ
            skillEffect.SetActive(true);

            //���� ������ ����
            manager.EquipmentManager.SetRightWeaponDamage(manager.EquipmentManager.RightWeapon.Damage * skillDamageMultiplier);
            manager.EquipmentManager.SetLeftWeaponDamage(manager.EquipmentManager.LeftWeapon.Damage * skillDamageMultiplier);
        }

        [PunRPC]
        public override void OnSkillEnded()
        {
            //����Ʈ ��Ȱ��ȭ
            skillEffect.SetActive(false);

            //���� ������ �ʱ�ȭ
            manager.EquipmentManager.SetRightWeaponDamage(manager.EquipmentManager.RightWeapon.Damage);
            manager.EquipmentManager.SetLeftWeaponDamage(manager.EquipmentManager.LeftWeapon.Damage);
        }
    }

}
