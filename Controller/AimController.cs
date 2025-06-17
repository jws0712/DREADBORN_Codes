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

        //초기화
        private void Start()
        {
            manager = GetComponentInParent<PlayerManager>();
        }


        private void LateUpdate()
        {
            if (manager.isPause) return;

            //카메라의 각도를 마우스에 맞춰 변경시킴
            mouseY += manager.Input.LookVec.x * sensitivity;

            mouseX -= manager.Input.LookVec.y * sensitivity;

            //카메라의 각도를 제한 시킴
            mouseX = Mathf.Clamp(mouseX, -cameraDirectionLimit, cameraDirectionLimit);

            transform.rotation = Quaternion.Euler(mouseX, mouseY, 0f);
        }
    }
}


