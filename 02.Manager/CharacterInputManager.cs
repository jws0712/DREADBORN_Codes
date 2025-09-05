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

        //������Ƽ
        public bool IsPressJump { get; set; }
        public bool IsPressSprint { get; set; }
        public bool IsPressInteraction { get; set; }
        public bool IsPressSKill { get; set; }
        public Vector2 MoveVec => moveVec;
        public Vector2 LookVec => lookVec;
        public bool IsPressAttack => isPressAttack;
        public bool IsPressDefend => isPressDefend;

        //�ʱ�ȭ
        public void Initialize()
        {
            manager = GetComponent<CharacterManager>();
        }

        //�̵� Ű �Է�
        private void OnMove(InputValue value)
        {
            if(!manager.IsDead) moveVec = value.Get<Vector2>();
        }

        //ī�޶� ȸ�� Ű �Է�
        private void OnLook(InputValue value)
        {
            lookVec = value.Get<Vector2>();
        }

        //���� Ű �Է�
        private void OnJump(InputValue value)
        {
            IsPressJump = value.isPressed;
        }

        //�޸��� Ű �Է�
        private void OnSprint(InputValue value)
        {
            IsPressSprint = value.isPressed;
        }

        //��ȣ�ۿ� Ű �Է�
        private void OnInteraction(InputValue value)
        {
            IsPressInteraction = value.isPressed;
        }

        //���� Ű �Է�
        private void OnAttack(InputValue value)
        {
            isPressAttack = value.isPressed;

        }

        //��� Ű �Է�
        private void OnDefend(InputValue value)
        {
            isPressDefend = value.isPressed;
        }

        //��ų Ű �Է�
        private void OnSkill(InputValue value)
        {
            IsPressSKill = value.isPressed;
        }

    }
}
