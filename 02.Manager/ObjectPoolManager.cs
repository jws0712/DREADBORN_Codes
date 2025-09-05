namespace DREADBORN
{
    //System
    using System;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        [SerializeField] protected PoolObjectData[] poolObjectDataArray;

        protected Dictionary<string, Queue<GameObject>> poolObjectDataDictionary = new Dictionary<string, Queue<GameObject>>();

        public override void Awake()
        {
            base.Awake();

            InitPool();
        }


        //오브젝트풀 초기화
        private void InitPool()
        {
            foreach (PoolObjectData poolObjectData in poolObjectDataArray)
            {
                poolObjectDataDictionary.Add(poolObjectData.poolPrefabObject.name, GetPoolDataQueue(poolObjectData.poolObjectContainer, poolObjectData.poolPrefabObject, poolObjectData.poolCount));
            }
        }

        //오브젝트가 저장된 큐를 반환함
        private Queue<GameObject> GetPoolDataQueue(Queue<GameObject> poolObjectQueue, GameObject poolObject, int poolCount)
        {
            for (int i = 0; i < poolCount; i++)
            {
                poolObjectQueue.Enqueue(CreateObjcet(poolObject));
            }

            return poolObjectQueue;
        }

        //오브젝트 생성
        private GameObject CreateObjcet(GameObject poolObject)
        {
            GameObject obj = Instantiate(poolObject);
            obj.name = poolObject.name;
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);

            return obj;
        }

        //오브젝트 소환을 요청함
        public GameObject SpawnPoolObject(string objectName, Vector3 position, Quaternion rotaion, float size = 1.0f, Transform parent = null)
        {
            if (poolObjectDataDictionary[objectName].Count > 0)
            {
                GameObject returnObject = poolObjectDataDictionary[objectName].Dequeue();
                SetObject(returnObject, position, rotaion, size, parent);
                return returnObject;
            }
            else
            {
                //미리 생성해둔 오브젝트를 전부 사용했을때
                PoolObjectData obj = Array.Find(poolObjectDataArray, x => x.poolPrefabObject.name == objectName);

                if (obj != null)
                {
                    GameObject returnObject = CreateObjcet(obj.poolPrefabObject);
                    SetObject(returnObject, position, rotaion, size, parent);
                    return returnObject;
                }
            }

            return null;
        }

        //풀에서 오브젝트를 꺼내고 위치, 회전, 크기를 설정함
        private void SetObject(GameObject obj, Vector3 position, Quaternion rotaion ,float size, Transform parent)
        {
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.transform.rotation = rotaion;
            obj.transform.localScale = Vector3.one * size;
            obj.gameObject.SetActive(true);
        }


        //오브젝트를 풀로 반환함
        public void ReturnObject(GameObject obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
            poolObjectDataDictionary[obj.name].Enqueue(obj);
        }
    }
}

