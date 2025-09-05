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


        //������ƮǮ �ʱ�ȭ
        private void InitPool()
        {
            foreach (PoolObjectData poolObjectData in poolObjectDataArray)
            {
                poolObjectDataDictionary.Add(poolObjectData.poolPrefabObject.name, GetPoolDataQueue(poolObjectData.poolObjectContainer, poolObjectData.poolPrefabObject, poolObjectData.poolCount));
            }
        }

        //������Ʈ�� ����� ť�� ��ȯ��
        private Queue<GameObject> GetPoolDataQueue(Queue<GameObject> poolObjectQueue, GameObject poolObject, int poolCount)
        {
            for (int i = 0; i < poolCount; i++)
            {
                poolObjectQueue.Enqueue(CreateObjcet(poolObject));
            }

            return poolObjectQueue;
        }

        //������Ʈ ����
        private GameObject CreateObjcet(GameObject poolObject)
        {
            GameObject obj = Instantiate(poolObject);
            obj.name = poolObject.name;
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);

            return obj;
        }

        //������Ʈ ��ȯ�� ��û��
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
                //�̸� �����ص� ������Ʈ�� ���� ���������
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

        //Ǯ���� ������Ʈ�� ������ ��ġ, ȸ��, ũ�⸦ ������
        private void SetObject(GameObject obj, Vector3 position, Quaternion rotaion ,float size, Transform parent)
        {
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.transform.rotation = rotaion;
            obj.transform.localScale = Vector3.one * size;
            obj.gameObject.SetActive(true);
        }


        //������Ʈ�� Ǯ�� ��ȯ��
        public void ReturnObject(GameObject obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
            poolObjectDataDictionary[obj.name].Enqueue(obj);
        }
    }
}

