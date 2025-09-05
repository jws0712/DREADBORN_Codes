namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class AttackDummy : MonoBehaviour, IDamageable
    {
        private Animator anim;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
        }

        public void TakeDamage(float damage)
        {
            anim.SetTrigger("Hit");
            GameObject damageTextEffect = ObjectPoolManager.Instance.SpawnPoolObject(ObjectPoolObjectName.DamageTextEffect, transform.position + Vector3.up, Quaternion.identity, 0.01f);
            damageTextEffect.GetComponent<DamageTextEffect>().SetDamageText(damage);
        }
    }
}


