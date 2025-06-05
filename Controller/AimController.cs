namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class AimController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] [Range(0.1f, 1f)] private float sensitivity = default;
        [SerializeField] private float cameraDirectionLimit = default;
        [SerializeField] private Transform moveDirection = null;

        private float mouseX = default;
        private float mouseY = default;

        private PlayerManager manager = null;

        public Transform MoveDirection => moveDirection;

        //�ʱ�ȭ
        private void Start()
        {
            manager = GetComponentInParent<PlayerManager>();
        }


        private void LateUpdate()
        {
            if (manager.isPause) return;

            //ī�޶��� ������ ���콺�� ���� �����Ŵ
            mouseY += manager.Input.LookVec.x * sensitivity;

            mouseX -= manager.Input.LookVec.y * sensitivity;

            //ī�޶��� ������ ���� ��Ŵ
            mouseX = Mathf.Clamp(mouseX, -cameraDirectionLimit, cameraDirectionLimit);

            transform.rotation = Quaternion.Euler(mouseX, mouseY, 0f);
        }
    }
}


