using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();

    public void SetPoolQueue(GameObject poolObject)
    {
        if (pool.ContainsKey(poolObject)) return;
        
        pool.Add(poolObject, new Queue<GameObject>());
    }

    public GameObject ActiveObject(GameObject poolObject, Vector3 pos = default, Quaternion rot = default, Transform parent = null)
    {
        for (int i = 0; i < pool[poolObject].Count; i++)
        {
            GameObject temp = pool[poolObject].Dequeue();

            if (temp.activeSelf == false)
            {
                temp.SetActive(true);
                temp.transform.position = pos;
                temp.transform.rotation = rot;
                temp.transform.SetParent(parent);
                pool[poolObject].Enqueue(temp);
                return temp;
            }
            pool[poolObject].Enqueue(temp);
        }
        
        GameObject newObject = Instantiate(poolObject, pos, rot, parent);
        pool[poolObject].Enqueue(newObject);
        return newObject;
    }
}
