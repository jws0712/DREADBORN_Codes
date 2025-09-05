namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project
    using static ObjectPoolObjectName;
    using Sound;

    public class KnightController : BaseCharacterController
    {
        [Header("SFX")]
        [SerializeField] private AudioClip skillSFX;

        private bool isSkill;

        public override void Defend()
        {
            if(isSkill)
            {
                return;
            }

            base.Defend();
        }

        //��ų�� �����Ҷ�
        [PunRPC]
        public override void OnSkilled()
        {
            isSkill = true;
            GameObject effect = ObjectPoolManager.Instance.SpawnPoolObject(DefendEffect, transform.position + Vector3.up, Quaternion.identity, 1f, transform);
            manager.IsDefend = true;

            if (manager.photonView.IsMine)
            {
                SoundManager.instance.SFXPlay("KnightSkill", skillSFX);
            }

        }

        //��ų�� ��������
        [PunRPC]
        public override void OnSkillEnded()
        {
            isSkill = false;
            manager.IsDefend = false;
        }
    }

}
