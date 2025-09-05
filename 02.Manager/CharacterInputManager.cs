namespace DREADBORN
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CharacterInputManager : MonoBehaviour
    {
        private Vector2 moveVec;
        private Vector2 lookVec;

        private bool isPressAttack;
        private bool isPressDefend;

        private CharacterManager manager;

        //프로퍼티
        public bool IsPressJump { get; set; }
        public bool IsPressSprint { get; set; }
        public bool IsPressInteraction { get; set; }
        public bool IsPressSKill { get; set; }
        public Vector2 MoveVec => moveVec;
        public Vector2 LookVec => lookVec;
        public bool IsPressAttack => isPressAttack;
        public bool IsPressDefend => isPressDefend;

        //초기화
        public void Initialize()
        {
            manager = GetComponent<CharacterManager>();
        }

        //이동 키 입력
        private void OnMove(InputValue value)
        {
            if(!manager.IsDead) moveVec = value.Get<Vector2>();
        }

        //카메라 회전 키 입력
        private void OnLook(InputValue value)
        {
            lookVec = value.Get<Vector2>();
        }

        //점프 키 입력
        private void OnJump(InputValue value)
        {
            IsPressJump = value.isPressed;
        }

        //달리기 키 입력
        private void OnSprint(InputValue value)
        {
            IsPressSprint = value.isPressed;
        }

        //상호작용 키 입력
        private void OnInteraction(InputValue value)
        {
            IsPressInteraction = value.isPressed;
        }

        //공격 키 입력
        private void OnAttack(InputValue value)
        {
            isPressAttack = value.isPressed;

        }

        //방어 키 입력
        private void OnDefend(InputValue value)
        {
            isPressDefend = value.isPressed;
        }

        //스킬 키 입력
        private void OnSkill(InputValue value)
        {
            IsPressSKill = value.isPressed;
        }

    }
}
