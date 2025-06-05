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
                    //인스턴스가 없다면 인스턴스를 찾음
                    instance = FindFirstObjectByType<T>();

                    //인스턴스가 찾아도 없다면 인스턴스를 새로 생성함
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();
                    }
                }

                //인스턴스를 반환
                return instance;
            }
        }

        //초기화
        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;

                //파괴되지 않고 계속 유지시킴
                DontDestroyOnLoad(gameObject);
            }
            //다른 인스턴스가 이미 있다면 현재 오브젝트를 파괴시킴
            else
            {
                Destroy(gameObject);
            }
        }
    }
}


