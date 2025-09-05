namespace DREADBORN
{
    using Sound;
    //System
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.InputSystem;

    //Project
    using static AnimationClipName;

    public class DamageCollider : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private Transform capsuleStartPosition;
        [SerializeField] private Transform capsuleEndPosition;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private float capsuleRadius;
        [SerializeField] private LayerMask hitLayer;
        [Header("Attack Shake")]
        [SerializeField] private Vector3 firstAttackShakeVelocity;
        [SerializeField] private Vector3 secondAttackShakeVelocity;
        [SerializeField] private Vector3 thirdAttackShakeVelocity;
        [Header("SFX")]
        [SerializeField] private AudioClip hitSFX;

        private bool canCheck;
        private float damage;

        private CharacterManager manager;

        public float Damage { get { return damage; } set { damage = value; } }

        private List<IDamageable> damagedTargets = new List<IDamageable>();

        private void Start()
        {
            manager = transform.root.GetComponent<CharacterManager>();
        }

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
                    if (manager.photonView.IsMine)
                    {
                        SoundManager.instance.SFXPlay("Hit", hitSFX);
                    }

                    //���ݿ� ���� �ٸ� ī�޶� ����ũ�� ����
                    switch (manager.Controller.LastAttack)
                    {
                        case Light_Attack1:
                            {
                                manager.ImpulseSource.GenerateImpulse(firstAttackShakeVelocity);

                                break;
                            }
                        case Light_Attack2:
                            {
                                manager.ImpulseSource.GenerateImpulse(secondAttackShakeVelocity);

                                break;
                            }
                        case Light_Attack3:
                            {
                                manager.ImpulseSource.GenerateImpulse(thirdAttackShakeVelocity);

                                break;
                            }
                    }


                    //����Ʈ�� ���� ��ġ�� ��ȯ��Ŵ
                    Vector3 effectPos = col.ClosestPoint(capsuleStartPosition.position);
                    ObjectPoolManager.Instance.SpawnPoolObject(hitEffect.name, effectPos, Quaternion.identity, 0.5f);

                    manager.UIController.EnableHitMarker();

                    //���� ������Ʈ���� �������� ����
                    target.TakeDamage(damage);

                    StartCoroutine(Co_Vibration(0.2f));

                    //���� ������Ʈ�� List�� �߰�
                    damagedTargets.Add(target);
                }
            }
        }

        private IEnumerator Co_Vibration(float time)
        {
            if (Gamepad.current == null) yield break;

            Gamepad.current.SetMotorSpeeds(0.3f, 0.6f);
            yield return new WaitForSeconds(time);
            Gamepad.current.SetMotorSpeeds(0f, 0f);
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