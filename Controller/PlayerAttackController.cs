namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;
    using UnityEngine.EventSystems;

    //Project
    using static AnimationClipName;

    public class PlayerAttackController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private float chargeAttackTime = default;

        private float currentMouseHoldTime = default;

        private string lastAttack = default;

        private WeaponType currentWeaponType = default;

        private PlayerManager manager = null;

        //초기화
        public void Initialize()
        {
            manager = GetComponent<PlayerManager>();
            SetWeaponType();
        }

        public void SetWeaponType()
        {
            currentWeaponType = manager.Inventroy.RightWeapon.Type;
        }

        //공격 관리
        public void AttackControl()
        {
            switch(currentWeaponType)
            {
                //근접 무기 공격
                case WeaponType.MeleeWeapon:
                    {
                        MeleeWeaponAttack();
                        break;
                    }
                //추후 다른 타입의 무기 추가

            }
        }

        //근접 무기 공격 관리
        private void MeleeWeaponAttack()
        {
            if (manager.Input.IsPressAttack && !manager.isPause)
            {
                //공격키를 누르고 있는 시간을 계산
                currentMouseHoldTime += Time.deltaTime;
            }
            else
            {
                //공격키를 한번이라도 눌렀을때
                if (currentMouseHoldTime == 0) return;

                //눌려진 시간이 차징 공격 시간보다 작을때
                if (currentMouseHoldTime < chargeAttackTime)
                {
                    //시간 초기화
                    currentMouseHoldTime = 0;

                    //콤보 상황일때
                    if (manager.canCombo)
                    {
                        LightAttackCombo();
                    }
                    else
                    {
                        //콤보 중이 아니고 애니매이션이 실행중이 아닐때
                        if (manager.isAction) return;
                        if (manager.canCombo) return;
                        Attack(Light_Attack1);
                    }
                }
                //차징 공격
                else
                {
                    //시간 초기화
                    currentMouseHoldTime = 0;
                }
            }
        }

        //공격 콤보 실행
        private void LightAttackCombo()
        {
            manager.canCombo = false;

            switch(lastAttack)
            {
                case Light_Attack1:
                    {
                        Attack(Light_Attack2);
                        break;
                    }
                case Light_Attack2:
                    {
                        Attack(Light_Attack3);
                        break;
                    }
                case Light_Attack3:
                    {
                        Attack(Light_Attack1);
                        break;
                    }
            }
        }

        //공격 실행
        private void Attack(string currentAttack)
        {
            manager.AnimController.PlayAnimation(currentAttack, true);
            lastAttack = currentAttack;
        }
    }
}