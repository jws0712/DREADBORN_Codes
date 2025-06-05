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
            if (PhotonNetwork.IsConnected)
            {
                //ȣ��Ʈ �϶�
                if (PhotonNetwork.IsMasterClient)
                {
                    currentHp -= damage;

                    //ȣ��Ʈ���� ����� ���� �ٸ� Ŭ���̾�Ʈ�� �����Ŵ
                    photonView.RPC("UpdateLifeStatus", RpcTarget.Others, currentHp, isDead);

                    //�¾Ҵٴ� ��ȣ�� �ٸ� Ŭ���̾�Ʈ�� ���Ե� ����
                    photonView.RPC("TakeDamage", RpcTarget.Others, damage);
                }

                //���� ���� ���°� �ƴϰ� ü���� 0�̳� 0���� �Ʒ���� ��� ó��
                if (currentHp <= 0 && !isDead)
                {
                    Die();
                }
            }
            else
            {
                currentHp -= damage;

                if (currentHp <= 0 && !isDead)
                {
                    Die();
                }
            }
        }

        //���� ���¿� ���� ü���� ������Ʈ��
        [PunRPC]
        public void UpdateLifeStatus(float hp, bool isDead)
        {
            currentHp = hp;
            this.isDead = isDead;
        }

        //���
        public virtual void Die()
        {
            Debug.Log("���");
            isDead = true;
        }
    }
}

