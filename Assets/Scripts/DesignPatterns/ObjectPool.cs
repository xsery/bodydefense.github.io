using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is an object pool, that we use for recycling object
/// </summary>
public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// An array of prefabs used to create an object in the gameworld
    /// </summary>
    [SerializeField]
    private GameObject[] objectPrefabs;

    /// <summary>
    /// A list of objects in the pool
    /// </summary>
    private List<GameObject> pooledObjects = new List<GameObject>();

    /// <summary>
    /// Gets an object from the pool
    /// </summary>
    /// <param name="type">The type of object that we request</param>
    /// <returns>A GameObject of specific type</returns>
    public GameObject GetObject(string type)
    {
        //Check the pool for the object
        foreach (GameObject go in pooledObjects)
        {
            //If the pool contains the object that we need then we reuse it
            if (go.name == type && !go.activeInHierarchy)
            {
                //Sets the object as active
                go.SetActive(true);

                //returns the objects
                return go;
            }
        }
        //If the pool didn't contain the object, that we needed then we need to create a new one
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            //If we have a prefab for creating the object
            if (objectPrefabs[i].name == type)
            {
                //We instantiate the prefab of the correct type
                GameObject newObject = Instantiate(objectPrefabs[i]);
                newObject.name = type;
                pooledObjects.Add(newObject);
                return newObject;
            }
        }

        //If everything fails return null
        return null;
    }

    /// <summary>
    /// Releases the object in the pool
    /// </summary>
    /// <param name="gameObject"></param>
    public void ReleaseObject(GameObject gameObject)
    {
        //Sets the object to inactive
        gameObject.SetActive(false);
    }
}
