using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PoolManager : SingletonMonoBehaviour<PoolManager>
{
    #region Tooltip
    [Tooltip("Populate this array with prefabs that are needed to be added to the pool, specify the # of gameobjects to be created for each")]
    #endregion
    [SerializeField] private Pool[] poolArray = null;
    private Transform objectPoolTransform;
    private Dictionary<int, Queue<Component>> poolDictionary = new Dictionary<int, Queue<Component>>();

    [System.Serializable] // more info: https://docs.unity3d.com/Manual/script-Serialization.html
    public struct Pool // when assigning variables in structs an instance of the object is copied (as opposed to normal variable values which are copied on assignment)
    {
        public int poolSize;
        public GameObject prefab;
        public string componentType;
    }

    private void Start()
    {
        // The singleton gameobject will be the object pool parent
        objectPoolTransform = this.gameObject.transform;

        // Create object pools on start
        for (int i = 0; i < poolArray.Length; i++)
        {
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize, poolArray[i].componentType);
        }
    }

    /// <summary>
    ///  Create the object pool with the specified prefabs and the specified pool size for each
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="poolSize"></param>
    /// <param name="componentType"></param>
    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        int poolKey = prefab.GetInstanceID();
        string prefabName = prefab.name; // get prefab name (used when creating parent anchor underneath the objectPoolTransform)
        GameObject parentGameObject = new GameObject(prefabName + "Anchor"); // create the parent gameobject to assign child objects to
        parentGameObject.transform.SetParent(objectPoolTransform);

        // Create objects in object pool
        // Ensure pool dictionary doesn't already contain a pool for this prefab
        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<Component>());
            // Loop through the same # of times as the pool size, for every iteration create a new object in pool
            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform) as GameObject; // objects in the pool are created with type GameObject as a child to the parentGameObject
                newObject.SetActive(false);
                poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType))); // this is why component type is needed as a variable, since the component type will be serialized in the editor
            }
        }
    }

    /// <summary>
    /// Reuse a gameobject component in the pool. 'prefab' is the prefab gameobject containing the component. <br/>
    /// 'position' is the world position for the gameobject where it should appear when enabled. <br/>
    /// 'rotation' should be set if the gameobject needs to be rotated.
    /// This method is called by any code that wants to get a component out of the pool.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();
        if (poolDictionary.ContainsKey(poolKey))
        {
            // Get object from pool queue
            Component componentToReuse = GetComponentFromPool(poolKey);

            // reset the game object
            ResetObject(position, rotation, componentToReuse, prefab);

            return componentToReuse;
        }
        else
        {
            Debug.Log("No object pool for " + prefab);
            return null;
        }
    }

    /// <summary>
    /// Resets the object position, rotation and scale.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="componentToReuse"></param>
    /// <param name="prefab"></param>
    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
    {
        componentToReuse.transform.position = position;
        componentToReuse.transform.rotation = rotation;
        componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;
    }

    /// <summary>
    /// Get a gameobject component from the pool using the 'poolKey'
    /// This method dequeues and enqueues the component to reuse. If the component is active, deactivate it.
    /// </summary>
    /// <param name="poolKey"></param>
    /// <returns></returns>
    private Component GetComponentFromPool(int poolKey)
    {
        Component componentToReuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(componentToReuse);
        if (componentToReuse.gameObject.activeSelf == true)
        {
            componentToReuse.gameObject.SetActive(false);
        }
        return componentToReuse;
    }
    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(poolArray), poolArray);
    }

#endif
    #endregion
}