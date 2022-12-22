using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] public float hungerValue;

    public static float speed = 5f;

    private void Update()
    {
        MoveObj();
    }

    private void MoveObj()
    {
        if (((GameStateManager.boolStates & 1) == 0) && ((GameStateManager.boolStates & 4) == 0))
        {
            transform.position += -transform.forward * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("collectibleObs"))
        {
            gameObject.SetActive(false);
            PoolObjectSpawner.spawnerIns.CallForSpawn(1);
        }
    }

}
