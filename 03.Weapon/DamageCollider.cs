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
                    if (manager.photonView.IsMine)
                    {
                        SoundManager.instance.SFXPlay("Hit", hitSFX);
                    }

                    //공격에 따라 다른 카메라 쉐이크를 실행
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


                    //이펙트를 맞은 위치에 소환시킴
                    Vector3 effectPos = col.ClosestPoint(capsuleStartPosition.position);
                    ObjectPoolManager.Instance.SpawnPoolObject(hitEffect.name, effectPos, Quaternion.identity, 0.5f);

                    manager.UIController.EnableHitMarker();

                    //맞은 오브젝트에게 데미지를 입힘
                    target.TakeDamage(damage);

                    StartCoroutine(Co_Vibration(0.2f));

                    //맞은 오브젝트를 List에 추가
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