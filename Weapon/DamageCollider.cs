namespace DREADBORN
{
    //System
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    public class DamageCollider : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private Transform capsuleStartPosition;
        [SerializeField] private Transform capsuleEndPosition;
        [SerializeField] private float capsuleRadius;
        [SerializeField] private LayerMask hitLayer;
        [SerializeField] private GameObject hitEffect;

        private bool canCheck;
        private float damage;
        public float Damage { get { return damage; } set { damage = value; } }

        private List<IDamageable> damagedTargets = new List<IDamageable>();

        private void Update()
        {
            if(!canCheck) return;

            //오버렙 캡슐로 오브젝트를 검사함
            Collider[] hitColliders = Physics.OverlapCapsule(
                capsuleStartPosition.position,
                capsuleEndPosition.position,
                capsuleRadius,
                hitLayer);

            foreach (Collider col in hitColliders)
            {
                //맞은 오브젝트의 IDamageable 인터페이스를 가져옴                
                IDamageable target = col.GetComponentInParent<IDamageable>();

                //맞은 오브젝트가 IDamageable 오브젝트를 가지고 있고 맞은 오브젝트가 List 안에 없다면
                if (target != null && !damagedTargets.Contains(target))
                {
                    //이펙트를 맞은 위치에 소환시킴
                    Vector3 effectPos = col.ClosestPoint(capsuleStartPosition.position);
                    ObjectPoolManager.Instance.SpawnPoolObject(hitEffect.name, effectPos, Quaternion.identity);

                    //맞은 오브젝트에게 데미지를 입힘
                    target.TakeDamage(damage);

                    //맞은 오브젝트를 List에 추가
                    damagedTargets.Add(target);
                }
            }
        }

        //오버랩 캡슐의 충돌검사를 시작함
        public void EnableDamageCollider()
        {
            canCheck = true;

            //맞은 오브젝트가 담긴 List를 초기화 시킴
            damagedTargets.Clear();
        }

        //오버랩 캡슐의 충돌검사를 끝냄
        public void DisableDamageCollider()
        {
            canCheck = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            if(capsuleEndPosition || capsuleStartPosition)
            {
                Gizmos.DrawWireSphere(capsuleStartPosition.position, capsuleRadius);
                Gizmos.DrawWireSphere(capsuleEndPosition.position, capsuleRadius);
            }
        }
    }
}