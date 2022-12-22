using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class GameStateManager : MonoBehaviour
{
    protected static Player_InputActions playerInputActions;
    protected const byte isGameOver = 1;  
    protected const byte isGameStarted = 2;
    protected const byte isGamePaused = 4;
    protected const byte isOnGround = 8;
    protected const byte canJump = 16;
    protected const byte isTurning = 32;
    protected static bool GameIsOver = false;
    public static int boolStates; 

    protected static bool isOnTurn;
  
    protected static byte onLane;  // 0 left lane, 1 mid lane, 2 right lane

    protected static Vector3 sumLaneDef = new Vector3(0, 0, 0);
    protected static byte turnDirection; // 0 turning left, 1, 2 turning right

    protected static string currentName;
    protected static float currentScore = 0;
    protected static int currentCollected = 0;
    protected static float currentDistance;   

    public static float planeMoveSpeed;
      
    protected static Vector3 playerDefPos;
    protected static Vector3 playerSideDefPos;
    protected static Quaternion playerDefRotation;

    public delegate void methodToPass();
    public delegate void dataToPass();
    
    protected virtual void Awake()
    {
        playerInputActions = new Player_InputActions();
        Time.timeScale = 0;
        boolStates = 0;
        boolStates |= isGamePaused;
    }

    protected void OnStartGameState()
    {
        boolStates &= ~isGameOver;      
        
        sumLaneDef = new Vector3(0,0,0);
        onLane = 1;
      
        turnDirection = 1;

        currentScore = 0;
        currentCollected = 0;
        currentDistance = 0;

        GameIsOver = false;

        boolStates = 0;
        boolStates |= (byte)(isGameStarted | isOnGround | canJump);
        isOnTurn = false; 
    }

    protected void PlayerDiedMethod()
    {
        boolStates = 0;
        boolStates |= isGameOver;
        GameIsOver = true;
}      

    protected void ToggleInteractionButton(Button button, bool val) // Toggles interactability of buttons 
    {
        button.interactable = val;
    }

    int randomNum;
    protected int RandomInt()
    {
        int i = Random.Range(0, 100);
        
        if(i >= 0 || i <=95)
        {
            randomNum = 1;
        }

        if(i > 95)
        {
            randomNum = 3;
        }

        return randomNum;
    }
    
    protected IEnumerator WaitorWithMehtod(float time, methodToPass delMethod)
    {
        yield return new WaitForSecondsRealtime(time);
        delMethod();
    }

    protected virtual void OnEnable()
    {
        playerInputActions.Enable(); 

        PlayerStats.PlayerDied += PlayerDiedMethod;
        PlayerCollision.PlayerDied += PlayerDiedMethod;
            
    }

    protected virtual void OnDisable()
    {
        PlayerStats.PlayerDied -= PlayerDiedMethod;
        PlayerCollision.PlayerDied -= PlayerDiedMethod;

        playerInputActions.Disable();
    }

}
