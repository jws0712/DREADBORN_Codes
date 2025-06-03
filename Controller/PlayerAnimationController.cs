namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    //Project
    using static AnimatorParameter;

    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private float trasitionDuration = default;

        private PlayerManager manager = null;

        private int animHorizontalID = default;
        private int animVerticalID = default;
        private int animIsCrouchID = default;
        private int animIsJumpID = default;
        private int animIsGroundID = default;
        private int animIsSprintID = default;
        private int animIsDefendID = default;

        //초기화 함수
        public void Initialize()
        {
            manager = GetComponent<PlayerManager>();

            animHorizontalID = Animator.StringToHash(Horizontal);
            animVerticalID = Animator.StringToHash(Vertical);
            animIsCrouchID = Animator.StringToHash(IsCrouch);
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

            //웅크리기 애니매이션 업데이트
            manager.ThirdPersonAnim.SetBool(animIsCrouchID, manager.PlayerController.IsCrouch);

            //점프 애니매이션 업데이트
            manager.ThirdPersonAnim.SetBool(animIsJumpID, manager.Input.isPressJump);
            manager.ThirdPersonAnim.SetBool(animIsGroundID, manager.CharacterController.isGrounded);

            //달리기 애니매이션 업데이트
            manager.ThirdPersonAnim.SetBool(animIsSprintID, manager.Input.IsPressSprint);

            //방어 애니매이션 업데이트
            manager.ThirdPersonAnim.SetBool(animIsDefendID, manager.Input.IsPressDefend);
        }

        public void SetTriggerAnimation(string parameterName)
        {
            manager.ThirdPersonAnim.SetTrigger(parameterName);
        }

        [PunRPC]
        //애니매이션을 실행 동기화
        public void TriggerAnimation(string targetAnimation, bool isAction, bool isRootMotion = false)
        {
            if (manager == null) return;
            manager.ThirdPersonAnim.applyRootMotion = isRootMotion;

            manager.ThirdPersonAnim.CrossFade(targetAnimation, trasitionDuration);

            manager.isAction = isAction;
        }

        //애니매이션 실행
        public void PlayAnimation(string targetAnimation, bool isAction, bool isRootMotion = false)
        {
            if(PhotonNetwork.IsConnected)
            {
                manager.photonView.RPC("TriggerAnimation", RpcTarget.All, targetAnimation, isAction, isRootMotion);
            }
            else
            {
                TriggerAnimation(targetAnimation, isAction, isRootMotion);
            }
        }
        
    }
}
