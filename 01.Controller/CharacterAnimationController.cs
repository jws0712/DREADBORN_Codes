namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project
    using static AnimatorParameter;
    using static ObjectPoolObjectName;
    using Sound;

    public class CharacterAnimationController : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private float trasitionDuration;
        [Header("SFX")]
        [SerializeField] private AudioClip[] stepSFX;

        private CharacterManager manager;
        private PhotonView pv;

        private int animHorizontalID;
        private int animVerticalID;
        private int animIsGroundID;
        private int animIsSprintID;
        private int animIsDefendID;

        //초기화 함수
        public void Initialize()
        {
            manager = GetComponentInParent<CharacterManager>();
            pv = GetComponentInParent<PhotonView>();

            animHorizontalID = Animator.StringToHash(Horizontal);
            animVerticalID = Animator.StringToHash(Vertical);
            animIsGroundID = Animator.StringToHash(IsGround);
            animIsSprintID = Animator.StringToHash(IsSprint);
            animIsDefendID = Animator.StringToHash(IsDefend);
        }

        public void OnStep()
        {
            if(manager.photonView.IsMine)
            {
                SoundManager.instance.SFXPlay("Step", stepSFX[Random.Range(0, stepSFX.Length)]);
            }

            ObjectPoolManager.Instance.SpawnPoolObject(FootStepEffect, transform.position, Quaternion.identity);
        }

        //플레이어 이동 애니매이션을 업데이트하는 함수
        public void UpdateAnimation(float horizontal, float vertical)
        {
            //걷는 애니매이션 업데이트
            manager.Animator.SetFloat(animHorizontalID, horizontal);
            manager.Animator.SetFloat(animVerticalID, vertical);

            //점프 애니매이션 업데이트
            manager.Animator.SetBool(animIsGroundID, manager.CharacterController.isGrounded);

            //달리기 애니매이션 업데이트
            manager.Animator.SetBool(animIsSprintID, manager.InputManager.IsPressSprint);

            //방어 애니매이션 업데이트
            manager.Animator.SetBool(animIsDefendID, manager.InputManager.IsPressDefend);
        }

        [PunRPC]
        //애니매이션 실행
        public void PlayAnimation(string targetAnimation, bool isAction, bool isRootMotion = false)
        {
            if (manager == null) return;

            manager.Animator.applyRootMotion = isRootMotion;

            manager.Animator.CrossFade(targetAnimation, trasitionDuration);

            manager.IsAction = isAction;

            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            {
                if(manager.photonView.IsMine)
                pv.RPC("PlayAnimation", RpcTarget.Others, targetAnimation, isAction, isRootMotion);
            }
        }
    }
}
