namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project
    using static AnimatorParameter;
    using Sound;

    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private float trasitionDuration;

        [Header("Sound")]
        [SerializeField] private AudioClip footStep;

        private PlayerManager manager;
        private PhotonView pv;

        private int animHorizontalID;
        private int animVerticalID;
        private int animIsJumpID;
        private int animIsGroundID;
        private int animIsSprintID;
        private int animIsDefendID;

        //초기화 함수
        public void Initialize()
        {
            manager = GetComponentInParent<PlayerManager>();
            pv = GetComponentInParent<PhotonView>();

            animHorizontalID = Animator.StringToHash(Horizontal);
            animVerticalID = Animator.StringToHash(Vertical);
            animIsJumpID = Animator.StringToHash(IsJump);
            animIsGroundID = Animator.StringToHash(IsGround);
            animIsSprintID = Animator.StringToHash(IsSprint);
            animIsDefendID = Animator.StringToHash(IsDefend);
        }

        //플레이어 이동 애니매이션을 업데이트하는 함수
        public void UpdateAnimation(float horizontal, float vertical)
        {
            //걷는 애니매이션 업데이트
            manager.ThirdPersonAnim.SetFloat(animHorizontalID, horizontal);
            manager.ThirdPersonAnim.SetFloat(animVerticalID, vertical);

            //점프 애니매이션 업데이트
            manager.ThirdPersonAnim.SetBool(animIsJumpID, manager.Input.isPressJump);
            manager.ThirdPersonAnim.SetBool(animIsGroundID, manager.CharacterController.isGrounded);

            //달리기 애니매이션 업데이트
            manager.ThirdPersonAnim.SetBool(animIsSprintID, manager.Input.isPressSprint);

            //방어 애니매이션 업데이트
            manager.ThirdPersonAnim.SetBool(animIsDefendID, manager.Input.IsPressDefend);
        }

        public void OnStep()
        {
            SoundManager.instance.SFXPlay("FootStep", footStep);
        }

        [PunRPC]
        //애니매이션 실행
        public void PlayAnimation(string targetAnimation, bool isAction, bool isRootMotion = false)
        {
            if (manager == null) return;

            manager.ThirdPersonAnim.applyRootMotion = isRootMotion;

            manager.ThirdPersonAnim.CrossFade(targetAnimation, trasitionDuration);

            manager.isAction = isAction;

            if (PhotonNetwork.IsConnected)
            {
                if(manager.photonView.IsMine)
                pv.RPC("PlayAnimation", RpcTarget.Others, targetAnimation, isAction, isRootMotion);
            }
        }
    }
}
