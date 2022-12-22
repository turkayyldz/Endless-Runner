using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerDataHandler : PlayerManager
{

    protected string bestPlayerName;
    protected float bestScore;

    protected string bestCollectedPlayer;
    protected int bestCollected;

    protected string bestDistancePlayer;
    protected float bestDistance;

    [SerializeField] protected TMP_Text[] bestStats;

    protected override void OnEnable()
    {
        base.OnEnable();
     
        MenuManager.GameSaved += SaveGameData;
        MenuManager.GameLoad += LoadGameData;
        MenuManager.GameDataDeleted += DeleteDataFile;
    }
   
    protected override void OnDisable()
    {
        base.OnEnable();

        MenuManager.GameSaved -= SaveGameData;
        MenuManager.GameLoad -= LoadGameData;
        MenuManager.GameDataDeleted -= DeleteDataFile;
    }


    [System.Serializable]
    class SaveData
    {
        public string dataBestScorePlayer;
        public float dataBestScore;

        public string dataBestDistancePlayer;
        public float dataBestDistance;

        public string dataMostCollectedPlayer;
        public int dataMostCollected;

        public string dataPlayerName;
        public float dataPlayerDistance;
        public int dataPlayerCollected;
        public float dataPlayerScore;
    }

    public void SaveGameData(string name) //Saves data in Json format
    {
        SaveData data = new SaveData();

        data.dataMostCollected = bestCollected;
        data.dataBestDistance = bestDistance;
        data.dataBestScore = bestScore;

        data.dataPlayerCollected = currentCollected;
        data.dataPlayerDistance = currentDistance;
        data.dataPlayerScore = currentScore;

        data.dataBestScorePlayer = bestPlayerName;
        data.dataBestDistancePlayer = bestDistancePlayer;
        data.dataMostCollectedPlayer = bestCollectedPlayer;
      
        data.dataPlayerName = name;

        BestStatsDisplay(bestScore, bestPlayerName, bestDistance, bestDistancePlayer, bestCollected,
                                   bestCollectedPlayer, currentName, currentScore, currentDistance, currentCollected);

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

  
    public void LoadGameData() //Loads data from the existing file
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);


            bestCollected = data.dataMostCollected;
            bestDistance = data.dataBestDistance;
            bestScore = data.dataBestScore;

            currentCollected = data.dataPlayerCollected;
            currentDistance = data.dataPlayerDistance;
            currentScore = data.dataPlayerScore;

            currentName = data.dataPlayerName;

            bestPlayerName = data.dataBestScorePlayer;
            bestDistancePlayer = data.dataBestDistancePlayer;
            bestCollectedPlayer = data.dataMostCollectedPlayer;

           
            BestStatsDisplay(bestScore, bestPlayerName, bestDistance, bestDistancePlayer, bestCollected,
                                   bestCollectedPlayer, currentName, currentScore, currentDistance, currentCollected);

        }
    }


    public void DeleteDataFile()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            File.Delete(Application.persistentDataPath + "/savefile.json");
        }

        bestCollected = 0;
        bestDistance = 0;
        bestScore = 0;

        bestPlayerName = "";
        bestDistancePlayer = "";
        bestCollectedPlayer = "";

        BestStatsDisplay(bestScore, bestPlayerName, bestDistance, bestDistancePlayer, bestCollected,
                                    bestCollectedPlayer, currentName, currentScore, currentDistance, currentCollected);


    }

    void BestStatsDisplay(float bestScore, string bestPlayerName, float bestDistance, string bestDistancePlayer, int bestCollected, 
                    string bestCollectedPlayer, string currentName, float currentScore, float currentDistance, int currentCollected )
    {
        bestStats[0].text = $"{bestScore} points by {bestPlayerName}";
        bestStats[1].text = $"{bestDistance} metres by {bestDistancePlayer}";
        bestStats[2].text = $"{bestCollected} by {bestCollectedPlayer}";

        bestStats[3].text = $"{currentName}:";
        bestStats[4].text = $"{currentScore} points";
        bestStats[5].text = $"{currentDistance} metres";
        bestStats[6].text = $"{currentCollected} tons of meat eaten";
    }
}
