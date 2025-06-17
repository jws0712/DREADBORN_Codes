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

            //������ ĸ���� ������Ʈ�� �˻���
            Collider[] hitColliders = Physics.OverlapCapsule(
                capsuleStartPosition.position,
                capsuleEndPosition.position,
                capsuleRadius,
                hitLayer);

            foreach (Collider col in hitColliders)
            {
                //���� ������Ʈ�� IDamageable �������̽��� ������                
                IDamageable target = col.GetComponentInParent<IDamageable>();

                //���� ������Ʈ�� IDamageable ������Ʈ�� ������ �ְ� ���� ������Ʈ�� List �ȿ� ���ٸ�
                if (target != null && !damagedTargets.Contains(target))
                {
                    //����Ʈ�� ���� ��ġ�� ��ȯ��Ŵ
                    Vector3 effectPos = col.ClosestPoint(capsuleStartPosition.position);
                    ObjectPoolManager.Instance.SpawnPoolObject(hitEffect.name, effectPos, Quaternion.identity);

                    //���� ������Ʈ���� �������� ����
                    target.TakeDamage(damage);

                    //���� ������Ʈ�� List�� �߰�
                    damagedTargets.Add(target);
                }
            }
        }

        //������ ĸ���� �浹�˻縦 ������
        public void EnableDamageCollider()
        {
            canCheck = true;

            //���� ������Ʈ�� ��� List�� �ʱ�ȭ ��Ŵ
            damagedTargets.Clear();
        }

        //������ ĸ���� �浹�˻縦 ����
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