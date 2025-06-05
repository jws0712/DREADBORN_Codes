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
            if (PhotonNetwork.IsConnected)
            {
                //호스트 일때
                if (PhotonNetwork.IsMasterClient)
                {
                    currentHp -= damage;

                    //호스트에서 변경된 값을 다른 클라이언트도 변경시킴
                    photonView.RPC("UpdateLifeStatus", RpcTarget.Others, currentHp, isDead);

                    //맞았다는 신호를 다른 클라이언트들 에게도 보냄
                    photonView.RPC("TakeDamage", RpcTarget.Others, damage);
                }

                //아직 죽은 상태가 아니고 체력이 0이나 0보다 아래라면 사망 처리
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

        //죽음 상태와 현재 체력을 업데이트함
        [PunRPC]
        public void UpdateLifeStatus(float hp, bool isDead)
        {
            currentHp = hp;
            this.isDead = isDead;
        }

        //사망
        public virtual void Die()
        {
            Debug.Log("사망");
            isDead = true;
        }
    }
}

