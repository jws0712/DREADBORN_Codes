namespace DREADBORN
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //Unity
    using UnityEngine;

    //Photon
    using Photon.Pun;

    public abstract class Character : MonoBehaviourPun, IDamageable
    {
        [Header("Info")]
        [SerializeField] protected float maxHp = default;

        [SerializeField] protected float currentHp = default;

        protected bool isDead = default;

        //프로퍼티
        public bool IsDead => isDead;
        public float CurrentHp => currentHp;
        public float MaxHp => maxHp;


        protected virtual void OnEnable()
        {
            currentHp = maxHp;
        }

        //데미지
        [PunRPC]
        public virtual void TakeDamage(float damage)
        {
            currentHp -= damage;

            if (currentHp <= 0)
            {
                Die();
            }

            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("UpdateLifeStatus", RpcTarget.Others, currentHp, isDead);
                }
                else
                {
                    photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
                }
            }
        }

        //변경된 값을 업데이트함
        [PunRPC]
        public void UpdateLifeStatus(float hp, bool isDead)
        {
            currentHp = hp;
            this.isDead = isDead;
        }

        //사망
        public virtual void Die()
        {
            isDead = true;

            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("UpdateLifeStatus", RpcTarget.Others, currentHp, isDead);
            }
        }
    }
}

