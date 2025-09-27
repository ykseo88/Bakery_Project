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

    // The key is the original prefab, the value is a queue of instances.
    private Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();
    // The key is the instance, the value is the original prefab.
    private Dictionary<GameObject, GameObject> prefabByInstance = new Dictionary<GameObject, GameObject>();

    public void SetPoolQueue(GameObject poolObject)
    {
        if (pool.ContainsKey(poolObject)) return;
        
        pool.Add(poolObject, new Queue<GameObject>());
    }

    public GameObject ActiveObject(GameObject poolObject, Vector3 pos = default, Quaternion rot = default, Transform parent = null)
    {
        // Ensure the pool for this prefab exists.
        if (!pool.ContainsKey(poolObject))
        {
            SetPoolQueue(poolObject);
        }

        // Try to get an object from the pool.
        if (pool[poolObject].Count > 0)
        {
            GameObject instance = pool[poolObject].Dequeue();
            
            // Safety check, should always be inactive.
            if (instance.activeSelf == false)
            {
                instance.transform.SetParent(parent);
                instance.transform.position = pos;
                instance.transform.rotation = rot;
                instance.SetActive(true);
                
                // Add mapping for the reused object.
                prefabByInstance.Add(instance, poolObject);
                
                return instance;
            }
        }

        // If pool is empty or no inactive object was found, create a new one.
        GameObject newInstance = Instantiate(poolObject, pos, rot, parent);
        // Add mapping for the new object.
        prefabByInstance.Add(newInstance, poolObject);
        
        return newInstance;
    }

    public void DeActiveObject(GameObject instance)
    {
        if (prefabByInstance.TryGetValue(instance, out GameObject originalPrefab))
        {
            instance.SetActive(false);
            pool[originalPrefab].Enqueue(instance);
            prefabByInstance.Remove(instance); // Clean up the mapping
        }
        else
        {
            // This case happens if you try to pool an object that was not created by this pool manager.
            // It's safer to just destroy it to prevent issues.
            Debug.LogWarning("Deactivating an object that is not managed by the PoolManager. Destroying it instead.", instance);
            Destroy(instance);
        }
    }
}