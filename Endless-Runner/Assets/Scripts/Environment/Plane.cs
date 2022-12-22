using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public static float speed;

    private void Update()
    {
        MovePlane();
    }


    [SerializeField] protected int removePlanePosition = -100;
    protected void MovePlane()
    {
        if (((GameStateManager.boolStates & 1) == 0) && ((GameStateManager.boolStates & 4) == 0) && ((GameStateManager.boolStates & 32) == 0))
        {
            transform.position += -transform.forward * Time.deltaTime * speed;
        }
        
        if (transform.position.z <= removePlanePosition)
        {
            RemoveObj();
        }
    }

    protected virtual void RemoveObj()
    {
        if (transform.position.z <= removePlanePosition)
        {
            this.gameObject.SetActive(false);
        }
    }
}
