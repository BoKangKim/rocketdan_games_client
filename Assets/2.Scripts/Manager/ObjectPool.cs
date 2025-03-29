using System.Collections.Generic;
using UnityEngine;

// 오브젝트를 재사용하기 위한 클래스
public class ObjectPool : MonoBehaviour
{
    // Pool을 담는 Dictionary
    private Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();
    // 생성한 오브젝트들을 종류별로 담기 위한 빈 오브젝트 Dictionary
    private Dictionary<string, GameObject> objectBoxDict = new Dictionary<string, GameObject>();

    private const string CLONE_NAME_FORMAT = "_Clone";

    // Pool에 재사용 가능한 오브젝트가 있으면 해당 오브젝트 리턴
    // 없으면 새로 생성
    public GameObject InstantiatePoolObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        string key = prefab.name;

        Queue<GameObject> poolQueue = null;
        GameObject instance = null;
        GameObject box = null;

        if (!poolDict.TryGetValue(key, out poolQueue))
        {
            poolQueue = new Queue<GameObject>();
            poolDict.Add(key, poolQueue);
        }

        if (!objectBoxDict.TryGetValue(key, out box))
        {
            box = new GameObject($"{key}_Box");
            box.transform.position = Vector3.zero;
            box.transform.rotation = Quaternion.identity;
            objectBoxDict.Add(key, box);
        }

        if (poolQueue.Count > 0)
        {
            instance = poolQueue.Dequeue();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.transform.SetParent(parent == null ? box.transform : parent);
        }
        else
        {
            instance = GameObject.Instantiate(prefab, position, rotation, parent == null ? box.transform : parent);
            instance.name = $"{key}{CLONE_NAME_FORMAT}";
        }

        instance.gameObject.SetActive(true);

        return instance;
    }

    // GameObject이외의 타입 리턴을 해주기위한 함수
    public T InstantiateT<T>(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
    {
        GameObject instance = InstantiatePoolObject(prefab, position, rotation, parent);

        return instance.GetComponent<T>();
    }

    // 삭제되어야 하는 오브젝트 Pool에 담기
    public void DestroyPoolObject(GameObject target)
    {
        string key = target.name.Replace(CLONE_NAME_FORMAT, "");

        Queue<GameObject> poolQueue = null;

        if (!poolDict.TryGetValue(key, out poolQueue))
        {
            GameObject.Destroy(target);
            Debug.LogWarning($"{target} is not pooling object");
            return;
        }

        target.gameObject.SetActive(false);
        poolQueue.Enqueue(target);
    }

    // Pool 삭제
    public void ClearPool()
    {
        foreach (var pool in poolDict)
        {
            while (pool.Value.Count > 0)
            {
                GameObject.Destroy(pool.Value.Dequeue().gameObject);
            }
        }

        foreach (var box in objectBoxDict)
        {
            GameObject.Destroy(box.Value);
        }

        poolDict.Clear();
        objectBoxDict.Clear();
    }
}
