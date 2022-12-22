using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlaneMovement : GameStateManager
{
    [SerializeField] GameObject[] turningPoints;

    const float rotationSpeed = 1.2f;
    const float lerpDuration = 0.8f;

    public static Action<string> isTurningAnim;

    protected override void Awake()
    {
        base.Awake();       
        transform.position = new Vector3(0, 0, 0);
    }  

    private void RotationMethodSort(bool direction)
    {
        boolStates |= isTurning;
        if (direction)
        {
            RotatePlaneLeft();
        }
        else if(!direction)
        {
            RotatePlaneRight();
        }
    }

    protected void RotatePlaneLeft() //turnDirection 0
    {
        isTurningAnim?.Invoke("hasTurnedLeft");
        boolStates |= isTurning;
        if (onLane == 0)
        {
            StartCoroutine(LerpRotation(lerpDuration, Quaternion.Euler(0, 90, 0), turningPoints[3]));         
        }

        else if (onLane == 1)
        {
            StartCoroutine(LerpRotation(lerpDuration, Quaternion.Euler(0, 90, 0), turningPoints[1]));
         
        }

        else if (onLane == 2)
        {
            StartCoroutine(LerpRotation(lerpDuration, Quaternion.Euler(0, 90, 0), turningPoints[4]));          
        }
        
        turnDirection = 1;
        isOnTurn = false;     
    }

    protected void RotatePlaneRight() //turnDirection 2
    {
        isTurningAnim?.Invoke("hasTurnedRight");
        boolStates |= isTurning;
        if (onLane == 0)
        {
            StartCoroutine(LerpRotation(lerpDuration, Quaternion.Euler(0, -90, 0), turningPoints[0]));
        }

        else if (onLane == 1)
        {
            StartCoroutine(LerpRotation(lerpDuration, Quaternion.Euler(0, -90, 0), turningPoints[1]));
        }

        else if (onLane == 2)
        {
            StartCoroutine(LerpRotation(lerpDuration, Quaternion.Euler(0, -90, 0), turningPoints[2]));
        }

        turnDirection = 1;
        isOnTurn = false;
    }

    IEnumerator LerpRotation(float totalDuration, Quaternion rotationAmount, GameObject obj)
    {
        float elapsedTime = 0f;
        Quaternion lastRotation = Quaternion.identity;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            float percentageComplete = elapsedTime / totalDuration;

            Quaternion newRotation = Quaternion.Lerp(Quaternion.identity, rotationAmount, percentageComplete);
            obj.transform.rotation *= newRotation * Quaternion.Inverse(lastRotation);

            lastRotation = newRotation;
            boolStates &= ~isTurning;
            yield return null;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerCollision.HasTurned += RotationMethodSort;
    }

    protected override void OnDisable()
    {
       foreach(GameObject obj in turningPoints)
        {
            obj.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        PlayerCollision.HasTurned -= RotationMethodSort;
    }
}
