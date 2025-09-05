namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    public class SimpleSpin : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private Vector3 spinDir;
        [SerializeField] private float speed;

        private void OnEnable()
        {
            //회전값 초기화
            transform.rotation = Quaternion.identity;
        }

        void Update()
        {
            //회전
            transform.Rotate(spinDir * speed * Time.deltaTime);
        }
    }
}

