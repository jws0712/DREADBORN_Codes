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

        //�ʱ�ȭ �Լ�
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

        //�÷��̾� �̵� �ִϸ��̼��� ������Ʈ�ϴ� �Լ�
        public void UpdateAnimation(float horizontal, float vertical)
        {
            //�ȴ� �ִϸ��̼� ������Ʈ
            manager.ThirdPersonAnim.SetFloat(animHorizontalID, horizontal);
            manager.ThirdPersonAnim.SetFloat(animVerticalID, vertical);

            //��ũ���� �ִϸ��̼� ������Ʈ
            manager.ThirdPersonAnim.SetBool(animIsCrouchID, manager.PlayerController.IsCrouch);

            //���� �ִϸ��̼� ������Ʈ
            manager.ThirdPersonAnim.SetBool(animIsJumpID, manager.Input.isPressJump);
            manager.ThirdPersonAnim.SetBool(animIsGroundID, manager.CharacterController.isGrounded);

            //�޸��� �ִϸ��̼� ������Ʈ
            manager.ThirdPersonAnim.SetBool(animIsSprintID, manager.Input.IsPressSprint);

            //��� �ִϸ��̼� ������Ʈ
            manager.ThirdPersonAnim.SetBool(animIsDefendID, manager.Input.IsPressDefend);
        }

        public void SetTriggerAnimation(string parameterName)
        {
            manager.ThirdPersonAnim.SetTrigger(parameterName);
        }

        [PunRPC]
        //�ִϸ��̼��� ���� ����ȭ
        public void TriggerAnimation(string targetAnimation, bool isAction, bool isRootMotion = false)
        {
            if (manager == null) return;
            manager.ThirdPersonAnim.applyRootMotion = isRootMotion;

            manager.ThirdPersonAnim.CrossFade(targetAnimation, trasitionDuration);

            manager.isAction = isAction;
        }

        //�ִϸ��̼� ����
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
