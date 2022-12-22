using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnvironment : MonoBehaviour
{
    [SerializeField] protected Light direLight;


    int lightVal = 5;
    bool isDimming = true;

    void LightChanged()
    {
        if (isDimming)
        {
            StartCoroutine(LerpAngleLight(5f, new Vector3(-lightVal, 0, 0)));

            if (direLight.transform.eulerAngles.x <= 10)
            {
                isDimming = false;
            }
        }
        if (!isDimming)
        {
           
            StartCoroutine(LerpAngleLight(5f, new Vector3(lightVal, 0, 0)));

            if (direLight.transform.eulerAngles.x >= 40)
            {
                isDimming = true;
            }
        }
        PlayerStats.lightUpCounter = 0;      
    }

    Vector3 initSumAngle = new Vector3(0, 0, 0);
    private IEnumerator LerpAngleLight(float seconds, Vector3 incrementVal)
    {
        Vector3 startingRot = direLight.transform.eulerAngles;
        initSumAngle = startingRot + incrementVal;

        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            direLight.transform.eulerAngles = new Vector3(Mathf.Lerp(startingRot.x, initSumAngle.x, (elapsedTime / seconds)), startingRot.y, startingRot.z);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        direLight.transform.eulerAngles = new Vector3(initSumAngle.x, startingRot.y, startingRot.z);
    }
    private void OnEnable()
    {      
        PlayerStats.EnvironmentChanged += LightChanged;
    }
    private void OnDisable()
    {
        PlayerStats.EnvironmentChanged -= LightChanged;
    }
}
