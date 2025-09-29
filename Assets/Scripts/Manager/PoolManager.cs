using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Singleton instance
    public static PoolManager instance;
    
    private readonly Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();
    private readonly Dictionary<GameObject, GameObject> prefabByInstance = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public GameObject ActivateGameObject(GameObject Obj, Vector3 pos = default(Vector3),
        Quaternion rot = default(Quaternion), Transform parent = default(Transform))
    {
        PrefabType type =  PrefabUtility.GetPrefabType(Obj);

        if (type == PrefabType.PrefabInstance || type == PrefabType.DisconnectedPrefabInstance)
        {
            
            for (int i = 0; i < pool[prefabByInstance[Obj]].Count; i++)
            {
                GameObject tempObj = pool[Obj].Dequeue();
                if (tempObj.activeSelf == false)
                {
                    tempObj.transform.SetParent(parent);
                    tempObj.transform.position = pos;
                    tempObj.transform.rotation = rot;
                    return tempObj;
                }
                Debug.Log($"{Obj.name} 인스턴스이고 재활용한다");
                pool[Obj].Enqueue(tempObj);
            }
            
            Debug.Log($"{Obj.name} 인스턴스이고 새로 줄거다");
            GameObject newObj = Instantiate(Obj, pos, rot, parent);
            prefabByInstance.Add(newObj, Obj);
            return newObj;
        }
        else if (type == PrefabType.Prefab)
        {
            
            if (pool.ContainsKey(Obj) == false)
            {
                Debug.Log($"{Obj.name} 프리펩이고 풀에 존재하지 않는다");
                pool.Add(Obj, new Queue<GameObject>());
                GameObject tempObj = Instantiate(Obj, pos, rot, parent);
                prefabByInstance.Add(tempObj, Obj);
                return tempObj;
            }
            else
            {
                
                for (int i = 0; i < pool[Obj].Count; i++)
                {
                    GameObject tempObj = pool[Obj].Dequeue();
                    if (tempObj.activeSelf == false)
                    {
                        Debug.Log($"{Obj.name} 프리펩이고 풀에 존재해서 재활용한다.");
                        tempObj.transform.SetParent(parent);
                        tempObj.transform.position = pos;
                        tempObj.transform.rotation = rot;
                        return tempObj;
                    }
                    pool[Obj].Enqueue(tempObj);
                }
                
                Debug.Log($"{Obj.name} 프리펩이고 풀에 남는게 없어서 새로 준다");
                GameObject newObj = Instantiate(Obj, pos, rot, parent);
                prefabByInstance.Add(newObj, Obj);
                return newObj;
            }
        }

        return null;
    }

    public void DeActivateObject(GameObject Obj)
    {
        Obj.SetActive(false);
        PrefabType type =  PrefabUtility.GetPrefabType(Obj);

        if (type == PrefabType.PrefabInstance || type == PrefabType.DisconnectedPrefabInstance)
        {
            pool[prefabByInstance[Obj]].Enqueue(Obj);
        }
        else if (type == PrefabType.Prefab)
        {
            pool[Obj].Enqueue(Obj);
        }
        
    }

    public void SetPoolQueue(GameObject Obj)
    {
        pool.Add(Obj, new Queue<GameObject>());
    }

    public GameObject ActiveObject(GameObject Obj, Vector3 pos = default(Vector3),
        Quaternion rot = default(Quaternion), Transform parent = default(Transform))
    {
        if(pool.ContainsKey(Obj) == false) SetPoolQueue(Obj);
        
        for (int i = 0; i < pool[Obj].Count; i++)
        {
            GameObject tempObj = pool[Obj].Dequeue();
            if (tempObj.activeSelf == false)
            {
                tempObj.SetActive(true);
                tempObj.transform.SetParent(parent);
                tempObj.transform.position = pos;
                tempObj.transform.rotation = rot;
                pool[Obj].Enqueue(tempObj);
                return tempObj;
            }
            pool[Obj].Enqueue(Obj);
        }
        
        GameObject newObj = Instantiate(Obj, pos, rot, parent);
        prefabByInstance.Add(newObj, Obj);
        pool[Obj].Enqueue(newObj);
        return newObj;
    }

    public void DeActiveObject(GameObject Obj)
    {
        Obj.SetActive(false);
    }
}