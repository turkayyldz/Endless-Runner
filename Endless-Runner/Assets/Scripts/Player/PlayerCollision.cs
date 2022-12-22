using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCollision : Player
{
    public static event Action<float> CollectibleCollected;    
    public static event Action<bool> HasTurned;
    public static event Action PlayerDied;

    private void OnCollisionEnter(Collision collision)
    {        
        if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {           
            if (collision.gameObject.CompareTag("obsTrip")) 
            { 
                playerCapsule.direction = 2;
                playerCapsule.center = new Vector3(0, 0.15f, 2f);
                playerCapsule.radius = 0.05f;               
                PlayerDie(1);                               
            }

            else if (collision.gameObject.CompareTag("obsCrash")) 
            {    
                PlayerDie(2);
                playerSideMoveGameObj.transform.eulerAngles = new(0, 0, 6);                
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            boolStates |= (byte)(isOnGround | canJump);        
        }
    }

    private void OnTriggerEnter(Collider other)
    {  
        if (other.CompareTag("TurnInput"))
        {
            MapSpawner.canSpawn = true;
            isOnTurn = true;
        }

     /*   if(other.CompareTag("TurnTriggerLeft") && turnDirection == 0)
        {
            HasTurned?.Invoke(true);
            //PlaneMovement.instance.RotatePlaneLeft();
        }

        if (other.CompareTag("TurnTriggerRight") && turnDirection == 2)
        {
            HasTurned?.Invoke(false);
            //PlaneMovement.instance.RotatePlaneRight();
        }
        if (other.CompareTag("TurnTrigger"))
        {
            SortRotationDirection(turnDirection);
        }*/
        if (other.CompareTag("TurnTrigger") || other.CompareTag("TurnTriggerLeft") || other.CompareTag("TurnTriggerRight"))
        {
            
            SortRotationDirection(turnDirection);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Collectible"))
        {
            other.gameObject.SetActive(false);
            PoolObjectSpawner.spawnerIns.CallForSpawn(RandomInt());
            
            CollectibleCollected?.Invoke(other.GetComponent<Collectibles>().hungerValue);
        }

        if (other.CompareTag("Spawner"))
        {
            MapSpawner.canSpawn = true;
            if (MapSpawner.canSpawn == true)
            {
                MapSpawner.instance.SpawnAtPoint();
                MapSpawner.canSpawn = false;
            }
        }
    }
 

    private void PlayerDie(byte causeOfDeath)
    {
        currentHunger = defTotalHunger;
        OnDiePlayAnim(causeOfDeath);       
       
        PlayerDied?.Invoke();
    }

  

    private void SortRotationDirection(float turnDir)
    {
        if (turnDir == 0)
        {
            HasTurned?.Invoke(true);
           // PlaneMovement.instance.RotatePlaneLeft();        
        }

        else if (turnDir == 2)
        {
            HasTurned?.Invoke(false);
           // PlaneMovement.instance.RotatePlaneRight();          
        }
    }
}
