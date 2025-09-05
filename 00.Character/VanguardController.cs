namespace DREADBORN
{
    //System
    using System.Collections;

    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project
    using static ObjectPoolObjectName;
    using Sound;

    public class VanguardController : BaseCharacterController
    {
        [Header("Info")]
        [SerializeField] private float skillDamage;
        [SerializeField] private Vector3 skillShakeVelocity;
        [Header("SFX")]
        [SerializeField] private AudioClip skillSFX;

        private DamageCollider[] skillColliders;

        //스킬 시작할때
        [PunRPC]
        public override void OnSkilled()
        {

            if (manager.photonView.IsMine)
            {
                SoundManager.instance.SFXPlay("VanguardSkill", skillSFX);
                manager.ImpulseSource.GenerateImpulse(skillShakeVelocity);
            }

            StartCoroutine(Co_Skill());
        }
        
        private IEnumerator Co_Skill()
        {
            GameObject effect = ObjectPoolManager.Instance.SpawnPoolObject(VanguardSkill, transform.position, transform.rotation, 1f, transform);
            skillColliders = effect.GetComponentsInChildren<DamageCollider>();

            foreach(var collider in skillColliders)
            {
                collider.EnableDamageCollider();
                collider.Damage = skillDamage;
            }

            yield return new WaitForSeconds(0.3f);

            foreach (var collider in skillColliders)
            {
                collider.DisableDamageCollider();
            }
        }
    }
}

