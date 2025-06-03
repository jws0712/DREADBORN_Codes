namespace DREADBORN
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerInputManager : MonoBehaviour
    {
        //Action Type : Value
        private Vector2 moveVec = default;
        private Vector2 lookVec = default;

        //Action Type : Button
        [HideInInspector] public bool isPressJump = default;
        [HideInInspector] public bool isPressInteraction = default;
        

        //Action Type : Pass Through
        private bool isPressAttack = default;
        private bool isPressDefend = default;
        private bool isPressCrouch = default;
        private bool isPressSprint = default;

        //프로퍼티
        public Vector2 MoveVec => moveVec;
        public Vector2 LookVec => lookVec;
        public bool IsPressAttack => isPressAttack;
        public bool IsPressDefend => isPressDefend;
        public bool IsPressCrouch => isPressCrouch;
        public bool IsPressSprint => isPressSprint;

        //이동 키 입력
        //Action Type : Value
        private void OnMove(InputValue value)
        {
            moveVec = value.Get<Vector2>();
        }

        //카메라 회전 키 입력
        //Action Type : Value
        private void OnLook(InputValue value)
        {
            lookVec = value.Get<Vector2>();
        }

        //점프 키 입력
        //Action Type : Button
        private void OnJump(InputValue value)
        {
            isPressJump = value.isPressed;
        }

        //상호작용 키 입력
        //Action Type : Button
        private void OnInteraction(InputValue value)
        {
            isPressInteraction = value.isPressed;

        }

        //달리기 키 입력
        //Action Type : Pass Through
        private void OnSprint(InputValue value)
        {
            isPressSprint = value.isPressed;
        }

        //공격 키 입력
        //Action Type : Pass Through
        private void OnAttack(InputValue value)
        {
            isPressAttack = value.isPressed;

        }

        //방어 키 입력
        //Action Type : Pass Through
        private void OnDefend(InputValue value)
        {
            isPressDefend = value.isPressed;
        }

        //웅크리기 키 입력
        //Action Type : Pass Through
        private void OnCrouch(InputValue value)
        {
            isPressCrouch = value.isPressed;
        }
    }
}
