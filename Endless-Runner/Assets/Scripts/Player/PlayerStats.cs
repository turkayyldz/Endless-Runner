using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;


public class PlayerStats : PlayerDataHandler
{
    [SerializeField] protected Image inGameHungerImage;
    [SerializeField] protected TMP_Text inGameScoreText;
    [SerializeField] protected TMP_InputField nameInputField;  
    [SerializeField] protected TMP_Text inGamePlayerNameText;

    protected const float distanceInvokeDelay = 1f;
    protected const float distanceInvokeRate = 5f;
    protected const float distanceRateIncrement = -0.2f;

    int levelUpCounter = 0;
    protected const int levelUpDistance = 7; // when invoke distance is called for the 7th time increment speed 
    public static int lightUpCounter = 0;
    public static int lightUpDistance = 2;

    const float speedIncrement = 0.2f;
    protected const float planeDefaultSpeed = 5f;

    public static event Action PlayerDied;
    public static event Action EnvironmentChanged;
    public static event Action<float> SpeedUp;

    private void ResetInGameUI()
    {
        currentName = "";
        currentScore = 0;
        currentCollected = 0;
        currentDistance = 0;

        planeMoveSpeed = planeDefaultSpeed;
        Collectibles.speed = planeDefaultSpeed;
        Plane.speed = planeDefaultSpeed;

        levelUpCounter = 0;
        lightUpCounter = 0;

        hungerDropInvokeDelay = 1f;
        hungerDropRate = defHungerInvokeRate;
        inGameHungerImage.fillAmount = 1;
        inGameScoreText.text = "0 km";
        inGamePlayerNameText.text = nameInputField.text == "" ? "Guest" : GetName(); // if name input is null return name as Guest
        currentName = inGamePlayerNameText.text;
    }

    void PlayerDie()
    {
        CancelInvoke("UpdateDistance");
        CancelInvoke("GetHungry");

        levelUpCounter = 0;
        lightUpCounter = 0;

        Collectibles.speed = 0;
        Plane.speed = 0;

        ValidationCheck();     
    }
    

    void UpdateLevel()
    {
        if (levelUpCounter >= levelUpDistance)
        {
            levelUpCounter = 0;
            LevelUp();
        }
    }

   
    protected void LevelUp()
    {
        if (planeMoveSpeed <= 12) // end speed, after this speed doesnt increment
        {
            planeMoveSpeed += speedIncrement;
        }
       
        SpeedUp?.Invoke(planeMoveSpeed);

        Collectibles.speed = planeMoveSpeed;
        Plane.speed = planeMoveSpeed;

        if(hungerDropRate >= 0.2f)
        {
            hungerDropRate += hungerInvokeRateIncrement; 
        }
        
        hungerDropInvokeDelay = 0;
        InvokeHunger(); // hunger invoke rate is sped up and invoked after level up
    }

    void InvokeHunger()
    {        
        CancelInvoke("GetHungry");
        InvokeRepeating("GetHungry", hungerDropInvokeDelay, hungerDropRate);
    }

    void GetHungry()
    {
        if (currentHunger > 0)
        {         
           currentHunger += defHungerDecrease * 95f;   
           inGameHungerImage.fillAmount += defHungerDecrease; // depletes hunger bar
        }

        else if (currentHunger <= 0)
        {
            Debug.Log("Died of hunger");

            CancelInvoke("UpdateDistance");
            currentHunger = 100;
            Collectibles.speed = 0;
            Plane.speed = 0;

            PlayerDied?.Invoke();
        }
    }

    
    void GetFed(float hungerValue)
    {
        currentCollected++;

        if (currentHunger < 100)
        {
            currentHunger += hungerValue * 10;
        }

        if (currentHunger > 100)
        {
            currentHunger = 100;
        }

        inGameHungerImage.fillAmount += hungerValue * 0.1f;
        
    }

    void UpdateDistanceInvoker() // Distance calculation is called through invoke
    {
        CancelInvoke("UpdateDistance");
        InvokeRepeating("UpdateDistance", distanceInvokeDelay, distanceInvokeRate);
    }


    void UpdateDistance() // Updates player distance and calls level update
    {
        currentDistance += planeMoveSpeed * Time.deltaTime * 100f; 
        
        inGameScoreText.text = currentDistance.ToString("F1") + " km";
        currentDistance = MathF.Round(currentDistance,1);

        lightUpCounter++;
        levelUpCounter++;
             
        UpdateLevel();

        if (lightUpCounter >= lightUpDistance)
        {
            EnvironmentChanged?.Invoke(); 
        }
    }

    void CalculatePlayerScore()
    {
        currentScore = currentDistance * currentCollected * 1.2f;
    }

    void ValidationCheck() // this method is checked on menu activated and on game over 
    {
        ValidateCollectibleCount(currentCollected);
        ValidateDistance(currentDistance);

        CalculatePlayerScore();
        ValidateScoreCount(currentScore);
    }

    public void ValidateScoreCount(float score)
    {
        currentScore = score;

        if (currentScore >= bestScore)
        {
            bestScore = currentScore;
            bestPlayerName = currentName;
        }
    }

    public void ValidateCollectibleCount(int collectedFood)
    {
        currentCollected = collectedFood;

        if (collectedFood >= bestCollected)
        {
            bestCollected = currentCollected;
            bestCollectedPlayer = currentName;
        }
    }

    public void ValidateDistance(float playerDistance) 
    {
        currentDistance = playerDistance;

        if (playerDistance >= bestDistance)
        {
            bestDistance = playerDistance;
            bestDistancePlayer = currentName;
        }
    }

    public string GetName() 
    {
        currentName = nameInputField.text;
        return currentName;
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        PlayerCollision.CollectibleCollected += GetFed;
        PlayerCollision.PlayerDied += PlayerDie;

        MenuManager.NewGameStarted += InvokeHunger;
        MenuManager.NewGameStarted += UpdateDistanceInvoker;
        MenuManager.NewGameStarted += ResetInGameUI;

        MenuManager.MenuToggled += ValidationCheck;

    }

    protected override void OnDisable()
    {
        
        PlayerCollision.CollectibleCollected -= GetFed;
        PlayerCollision.PlayerDied -= PlayerDie;

        MenuManager.NewGameStarted -= UpdateDistanceInvoker;
        MenuManager.NewGameStarted -= ResetInGameUI;
        MenuManager.NewGameStarted -= InvokeHunger;

        MenuManager.MenuToggled -= ValidationCheck;
        base.OnDisable();
    }
}
