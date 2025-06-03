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

        //������Ƽ
        public bool IsDead => isDead;
        public float CurrentHp => currentHp;
        public float MaxHp => maxHp;


        protected virtual void OnEnable()
        {
            currentHp = maxHp;
        }

        //������
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

        //����� ���� ������Ʈ��
        [PunRPC]
        public void UpdateLifeStatus(float hp, bool isDead)
        {
            currentHp = hp;
            this.isDead = isDead;
        }

        //���
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

