using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private void Awake()
    {
        instance = this;
    }
    
    private Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, GameObject> prefabByInstance = new Dictionary<GameObject, GameObject>();

    public void SetPoolQueue(GameObject poolObject)
    {
        if (pool.ContainsKey(poolObject)) return;
        
        pool.Add(poolObject, new Queue<GameObject>());
    }

    public GameObject ActiveObject(GameObject poolObject, Vector3 pos = default, Quaternion rot = default, Transform parent = null)
    {
        if (!pool.ContainsKey(poolObject))
        {
            pool.Add(poolObject, new Queue<GameObject>());
        }

        GameObject instance = null;

        // Find a valid, non-destroyed object in the pool
        while (pool[poolObject].Count > 0)
        {
            instance = pool[poolObject].Dequeue();
            if (instance != null)
            {
                // Found a valid object, stop searching.
                break;
            }
            Debug.LogWarning("Found and removed a destroyed object reference from the pool.");
        }

        // If 'instance' is null here, the pool was empty or only contained destroyed objects.
        if (instance == null)
        {
            // Create a new one if no valid pooled object was found.
            instance = Instantiate(poolObject, pos, rot, parent);
            prefabByInstance.Add(instance, poolObject); // Track the new instance
        }
        else
        {
            // Configure the reused object from the pool.
            instance.transform.SetParent(parent);
            instance.transform.position = pos;
            instance.transform.rotation = rot;
            instance.SetActive(true); // Always ensure it's active
            prefabByInstance.Add(instance, poolObject); // Re-track the reused instance
        }
        
        return instance;
    }

    public void DeActiveObject(GameObject instance)
    {
        if (prefabByInstance.TryGetValue(instance, out GameObject originalPrefab))
        {
            instance.SetActive(false);
            instance.transform.SetParent(transform);
            pool[originalPrefab].Enqueue(instance);
            prefabByInstance.Remove(instance); // Clean up the mapping
        }
        else
        {
            Debug.LogWarning("Deactivating an object that is not managed by the PoolManager. Destroying it instead.", instance);
            Destroy(instance);
        }
    }
}