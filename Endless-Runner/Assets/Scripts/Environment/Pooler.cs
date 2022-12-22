using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public GameObject parent;
        public GameObject spawnObject;
        public int objID;
        public int size;
    }

    public List<Pool> pools = new List<Pool>();
    public Dictionary<int, Queue<GameObject>> poolDictionary;

    public static Pooler instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        poolDictionary = new Dictionary<int, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.spawnObject, pool.parent.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.objID, objectPool);
        }
    }

    public void SpawnRandomNumber(int ID, Quaternion rotation, int max, GameObject parentPos)
    {
        for (int i = 0; i < max; i++)
        {
            SpawnFromPool(ID, rotation, parentPos);
        }
    }

    public GameObject SpawnFromPool(int ID, Quaternion rotation, GameObject parentPos)
    {
        if (!poolDictionary.ContainsKey(ID))
        {
            Debug.LogWarning($"Pool with tag {ID} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[ID].Dequeue();
        objectToSpawn.SetActive(true);

        objectToSpawn.transform.position = randomPosition();
        objectToSpawn.transform.rotation = rotation;
        CheckForCollision(objectToSpawn);

        poolDictionary[ID].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    void CheckForCollision(GameObject obj)
    {
        if (Physics.CheckSphere(obj.transform.position, 0.9f, 7))
        {
            obj.transform.position = randomPosition();
            CheckForCollision(obj);
        }
        else
            return;
    }

    int[] xBound = new int[3] { -3, 0, 3 };
    float[] yBound = new float[3] { 0.5f, 1.5f, 3 }; // adjust according to jump height
   
    Vector3 randomPos;
    public Vector3 randomPosition()
    {
        randomPos = new Vector3(xBound[Random.Range(0, xBound.Length)], yBound[Random.Range(0, yBound.Length)], Random.Range(5,180f));
        return randomPos;
    }
}
