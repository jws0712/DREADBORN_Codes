namespace DREADBORN
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //Unity
    using UnityEngine;

    //Photon
    using Photon.Pun;

    public class Character : MonoBehaviourPun, IDamageable
    {
        protected float currentHp;
        protected bool isDead;

        //������Ƽ
        public bool IsDead => isDead;
        public float CurrentHp => currentHp;
        public float MaxHp { get; set; }

        private void OnEnable()
        {
            currentHp = MaxHp;
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
        protected virtual void Die()
        {
            isDead = true;

            if(PhotonNetwork.IsConnected)
            photonView.RPC("UpdateLifeStatus", RpcTarget.Others, currentHp, isDead);
        }
    }
}

