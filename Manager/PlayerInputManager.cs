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

        //������Ƽ
        public Vector2 MoveVec => moveVec;
        public Vector2 LookVec => lookVec;
        public bool IsPressAttack => isPressAttack;
        public bool IsPressDefend => isPressDefend;
        public bool IsPressCrouch => isPressCrouch;
        public bool IsPressSprint => isPressSprint;

        //�̵� Ű �Է�
        //Action Type : Value
        private void OnMove(InputValue value)
        {
            moveVec = value.Get<Vector2>();
        }

        //ī�޶� ȸ�� Ű �Է�
        //Action Type : Value
        private void OnLook(InputValue value)
        {
            lookVec = value.Get<Vector2>();
        }

        //���� Ű �Է�
        //Action Type : Button
        private void OnJump(InputValue value)
        {
            isPressJump = value.isPressed;
        }

        //��ȣ�ۿ� Ű �Է�
        //Action Type : Button
        private void OnInteraction(InputValue value)
        {
            isPressInteraction = value.isPressed;

        }

        //�޸��� Ű �Է�
        //Action Type : Pass Through
        private void OnSprint(InputValue value)
        {
            isPressSprint = value.isPressed;
        }

        //���� Ű �Է�
        //Action Type : Pass Through
        private void OnAttack(InputValue value)
        {
            isPressAttack = value.isPressed;

        }

        //��� Ű �Է�
        //Action Type : Pass Through
        private void OnDefend(InputValue value)
        {
            isPressDefend = value.isPressed;
        }

        //��ũ���� Ű �Է�
        //Action Type : Pass Through
        private void OnCrouch(InputValue value)
        {
            isPressCrouch = value.isPressed;
        }
    }
}
