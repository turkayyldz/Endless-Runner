using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{       
    [SerializeField] List<GameObject> initiatedMapsL = new List<GameObject>(); 
    [SerializeField] GameObject[] mapPrefabsArr;
    [SerializeField] GameObject spawnParentTransform;
    [SerializeField] GameObject initialMap;

    GameObject mapToSpawn;

    private Vector3 spawnPos = new Vector3(0, 0, 150f);
    private Vector3 startPos = new Vector3(0, 0, 50f);

    public static bool canSpawn = false;
    public static MapSpawner instance;

    private void Awake()
    {
        if(instance != null & instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }        

        foreach (var currentRoad in mapPrefabsArr)
        {
            var tempRoad = Instantiate(currentRoad, spawnPos, Quaternion.identity, spawnParentTransform.transform);
            tempRoad.SetActive(false);
            initiatedMapsL.Add(tempRoad);
        }
    }

    public void SpawnAtPoint()
    {        
        mapToSpawn = PickSpawnMap();

        mapToSpawn.transform.position = new Vector3(0, 0, 150f);
        mapToSpawn.SetActive(true);
    }

    private GameObject PickSpawnMap() // picks a random map from initiatedMapsL, checks if that map is alread active. 
    {   
        mapToSpawn = initiatedMapsL[Random.Range(0, mapPrefabsArr.Length)];   

        if (mapToSpawn.activeSelf)
        {
            mapToSpawn = initiatedMapsL[Random.Range(0, mapPrefabsArr.Length)];
            PickSpawnMap();
        }
        return mapToSpawn;
    }

    private void DisableMaps() // Sets all maps to false
    {
        foreach (GameObject road in initiatedMapsL)
        {
            road.SetActive(false);
            road.transform.position = new Vector3(0, 0, 150f);
        }

        initialMap.SetActive(true);
        initialMap.transform.position = startPos;
    }
   
    private void OnEnable()
    {
        MenuManager.NewGameStarted += DisableMaps;
    }

    private void OnDisable()
    {       
        foreach (GameObject road in initiatedMapsL)
        {
            Destroy(road);
        } 
        MenuManager.NewGameStarted -= DisableMaps;
    }

    
 

    
}
