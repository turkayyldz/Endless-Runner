using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjectSpawner : MonoBehaviour
{
    Pooler pooler;
    [SerializeField] int[] objectIDToSpawn;
    [SerializeField] int spawnAmount = 6;

    [SerializeField] GameObject parentPrefab;

    Vector3 parentBound;
    public static PoolObjectSpawner spawnerIns;

    [SerializeField] string[] objectsToSpawn;

    private void Start()
    {
        spawnerIns = this;  
        CallForSpawn(spawnAmount);
    }

    public void CallForSpawn(int callNum)
    {
        pooler = Pooler.instance;

        for (int i = 0; i < objectsToSpawn.Length; i++)
        {
            pooler.SpawnRandomNumber(0, Quaternion.identity, callNum, parentPrefab);
        }
    }
}
