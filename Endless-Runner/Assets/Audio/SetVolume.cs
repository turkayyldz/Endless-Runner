using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private string parameterName;

    [SerializeField]
    private Toggle audioToggle;

    [SerializeField]
    private AudioMixer audioMixer;
    private float sliderValue;

    private bool toggleOn = false;

    private float audioDB;

    private void Awake()
    {
        audioToggle.isOn = true;
        slider.value = slider.maxValue / 2;
        audioDB = Mathf.Log10(slider.value) * 20 ;
    }
    void Start()
    {
        //audioToggle.isOn = PlayerPrefs.GetInt(parameterName) == 0 ? true : false;
        //audioMixer.GetFloat(parameterName, out audioDB);
        
        audioMixer.SetFloat(parameterName, audioToggle.isOn ? audioDB : -80);
        audioToggle.onValueChanged.AddListener(ToggleAudio);
       
    }
    public void ToggleAudio(bool isOn)
    {       

        if (isOn)
        {
            audioMixer.SetFloat(parameterName, audioDB);
        }
        else
        {
            audioMixer.SetFloat(parameterName, -80);
        }

        PlayerPrefs.SetInt(parameterName, isOn ? 1 : 0);
        toggleOn = isOn;
        if (toggleOn == false)
        {
            slider.value = slider.minValue;
        }
        else
        {
            slider.value = slider.maxValue / 2;
        }

    }

    public void SetLevel(float sliderValue)
    {
        audioMixer.SetFloat(parameterName, Mathf.Log10(sliderValue) * 20);

    }
}
