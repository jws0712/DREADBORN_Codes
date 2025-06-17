namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class AimController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] [Range(0.1f, 1f)] private float sensitivity;
        [SerializeField] private float cameraDirectionLimit;
        [SerializeField] private Transform moveDirection;

        private float mouseX;
        private float mouseY;

        private PlayerManager manager;

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


