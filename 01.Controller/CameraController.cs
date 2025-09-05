namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] [Range(0.1f, 1f)] private float sensitivity;
        [SerializeField] private float cameraDirectionLimit;
        [SerializeField] private Transform moveDirection;

        private float mouseX;
        private float mouseY;

        private CharacterManager manager;

        public Transform MoveDirection => moveDirection;

        //�ʱ�ȭ
        private void Start()
        {
            manager = GetComponentInParent<CharacterManager>();
        }


        private void LateUpdate()
        {
            if (manager.IsPaused) return;

            //ī�޶��� ������ ���콺�� ���� �����Ŵ
            mouseY += manager.InputManager.LookVec.x * sensitivity;

            mouseX -= manager.InputManager.LookVec.y * sensitivity;

            //ī�޶��� ������ ���� ��Ŵ
            mouseX = Mathf.Clamp(mouseX, -cameraDirectionLimit, cameraDirectionLimit);

            transform.rotation = Quaternion.Euler(mouseX, mouseY, 0f);
        }
    }
}


