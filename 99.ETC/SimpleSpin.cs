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
            //ȸ���� �ʱ�ȭ
            transform.rotation = Quaternion.identity;
        }

        void Update()
        {
            //ȸ��
            transform.Rotate(spinDir * speed * Time.deltaTime);
        }
    }
}

