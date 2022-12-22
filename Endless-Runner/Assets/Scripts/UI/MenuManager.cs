using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class MenuManager : GameStateManager
{
    [SerializeField] Button bttnStartNewGame;
    [SerializeField] Button bttnPause;
    [SerializeField] Button bttnResume;
    [SerializeField] Button bttnSaveGame;
    [SerializeField] Button bttnPlayerStats;
    [SerializeField] Button bttnExit;
    [SerializeField] Button bttnReturn;
    [SerializeField] Button bttnDeleteGameData;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject statsMenu;

    public static event Action NewGameStarted;
    public static event Action<string> GameSaved;
    public static event Action GameLoad;
    public static event Action GameDataDeleted;
    public static event Action MenuToggled; // validates scores and data on menu toggle

    protected override void Awake()
    {
        base.Awake();
        GameLoad?.Invoke();
    }

    private void StartNewGame()
    {
        NewGameStarted?.Invoke();

        Time.timeScale = 1;

        OnStartGameState();

        inGameUI.SetActive(true);
        mainMenu.SetActive(false);

        ToggleInteractionButton(bttnResume, true);
        ToggleInteractionButton(bttnSaveGame, true);
    }

    private void SaveGame()
    {
        MenuToggled?.Invoke();
        GameSaved?.Invoke(currentName);
    }

    private void DeleteGameData()
    {
        GameDataDeleted?.Invoke();
    }

    private void PauseToggle(InputAction.CallbackContext context)
    {
        if (!statsMenu.activeSelf && (boolStates & isGameStarted) == isGameStarted)
        {
            if ((boolStates & isGamePaused) == isGamePaused) ResumeGame();
            else PauseGame();
        }
    }

    private void PauseGame()
    {
        MenuToggled?.Invoke();
        GameSaved?.Invoke(currentName);

        inGameUI.SetActive(false);
        mainMenu.SetActive(true);

        Time.timeScale = 0f;
        boolStates |= isGamePaused;
    }

    
    private void ResumeGame()
    {
        inGameUI.SetActive(true);
        mainMenu.SetActive(false);

        StartCoroutine(WaitorResume(0.2f));
        boolStates &= ~isGamePaused;
    }

    protected IEnumerator WaitorResume(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }

   

    private void ReturnToMenu()
    {
        statsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void OpenStatsMenu()
    {
        GameLoad?.Invoke();

        statsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    private void OnGameOver()
    {
        GameSaved?.Invoke(currentName);
        StartCoroutine(WaitorWithMehtod(2f, ResetUI));
    }

    private void ResetUI()
    {
        if ((boolStates & isGameOver) != 0)
        {
            inGameUI.SetActive(false);
            mainMenu.SetActive(true);

            ToggleInteractionButton(bttnResume, false);

            Time.timeScale = 0;
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        bttnStartNewGame.onClick.AddListener(() => StartNewGame());
        bttnPause.onClick.AddListener(() => PauseGame());
        bttnResume.onClick.AddListener(() => ResumeGame());
        bttnSaveGame.onClick.AddListener(() => SaveGame());
        bttnPlayerStats.onClick.AddListener(() => OpenStatsMenu());
        bttnReturn.onClick.AddListener(() => ReturnToMenu());
        bttnDeleteGameData.onClick.AddListener(() => DeleteGameData());
        bttnExit.onClick.AddListener(() => QuitGame());

        PlayerStats.PlayerDied += OnGameOver;
        PlayerCollision.PlayerDied += OnGameOver;

        playerInputActions.Player.PauseGame.started += PauseToggle;
        ToggleInteractionButton(bttnResume, false);
        ToggleInteractionButton(bttnSaveGame, false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        bttnStartNewGame.onClick.RemoveListener(() => StartNewGame());
        bttnPause.onClick.RemoveListener(() => PauseGame());
        bttnResume.onClick.RemoveListener(() => ResumeGame());
        bttnSaveGame.onClick.RemoveListener(() => SaveGame());
        bttnPlayerStats.onClick.RemoveListener(() => OpenStatsMenu());
        bttnReturn.onClick.RemoveListener(() => ReturnToMenu());
        bttnDeleteGameData.onClick.RemoveListener(() => DeleteGameData());
        bttnExit.onClick.RemoveListener(() => QuitGame());

        PlayerStats.PlayerDied -= OnGameOver;
        PlayerCollision.PlayerDied -= OnGameOver;
        playerInputActions.Player.PauseGame.started -= PauseToggle;
    }
}
