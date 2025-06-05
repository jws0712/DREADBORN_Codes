namespace DREADBORN
{
    using Photon.Pun;
    //UnityEngine
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;
        
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //�ν��Ͻ��� ���ٸ� �ν��Ͻ��� ã��
                    instance = FindFirstObjectByType<T>();

                    //�ν��Ͻ��� ã�Ƶ� ���ٸ� �ν��Ͻ��� ���� ������
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();
                    }
                }

                //�ν��Ͻ��� ��ȯ
                return instance;
            }
        }

        //�ʱ�ȭ
        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;

                //�ı����� �ʰ� ��� ������Ŵ
                DontDestroyOnLoad(gameObject);
            }
            //�ٸ� �ν��Ͻ��� �̹� �ִٸ� ���� ������Ʈ�� �ı���Ŵ
            else
            {
                Destroy(gameObject);
            }
        }
    }
}


