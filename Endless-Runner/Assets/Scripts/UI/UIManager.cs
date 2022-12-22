using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : GameStateManager
{
   
  /*  protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    //    hungerImage.fillAmount =1;
      //  ToggleInteractionButton(resumeButton, false);
    }*/


   /* public void OnStartInGameUI()
    {
        hungerImage.fillAmount = 1;

        inGameUI.SetActive(true);
        mainMenu.SetActive(false);
      
      

        ToggleInteractionButton(resumeButton, true);  
    }*/

   /* private void GameOverUI()
    {
       
       
    }

 


    public void ReturntoMenu()
    {
        statsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

 

    public void OpenStatsMenu()
    {
        statsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }*/

/*    public void SaveGameButton()
    {
        PlayerStats.plCurrentName = playerNameText.text;
        PlayerDataHandler.instance.SaveGameData(); 
        MenuToggled?.Invoke();
    }
*/
 /*   public void DeleteGameData()
    {
        Time.timeScale = 0;
        PlayerDataHandler.instance.DeleteDataFile();
    }*/
 /*   public string GetName()
    {
        PlayerStats.plCurrentName = nameInputField.text;
        return PlayerStats.plCurrentName;
    }

   


    private void OnEnable()
    {
        PlayerStats.GameOver += GameOverUI;
        MenuManager.NewGameStarted += OnStartInGameUI;

    }

    private void OnDisable()
    {
        PlayerStats.GameOver -= GameOverUI;
        MenuManager.NewGameStarted -= OnStartInGameUI;
    }*/
}
