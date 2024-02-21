using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnTrash : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    public int minObjects = 2, maxObjects = 5;
    public float interval = 3f;

    void Start()
    {
        InvokeRepeating(nameof(InstantiateObjects), 0f, interval);
    }

    void InstantiateObjects()
    {
        int amount = Random.Range(minObjects, maxObjects);
        for (int i = 0; i < amount; i++)
        {
            Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
        }
    }
}
