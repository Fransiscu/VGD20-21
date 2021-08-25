using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Gender {MALE, FEMALE};
    

[System.Serializable]
public class Player
{
    public string name;
    public Gender gender;
    public int currentLevel;
    public float currentLives;
    public bool atCheckpoint;
    public int currentScore;
    public int lifeTimeScore;
    public bool finishedGame;
    public List<int> unlockedLevels;

    private static readonly string saveData = PlayerPrefsKey.saveDataPrefName;
    public Player()
    {
    }

    public Player (string name, Gender gender)
    {
        this.name = name;
        this.gender = gender;
        lifeTimeScore = 0;
        finishedGame = false;
        currentLevel = 0;
        currentLives = 3;
        currentScore = 0;
        atCheckpoint = false;
        unlockedLevels = new List<int>() { 1 };
    }

    // getters and setters
    public string Name { get => name; set => name = value; }
    public int LifeTimeScore { get => lifeTimeScore; set => lifeTimeScore = value; }
    public bool FinishedGame { get => finishedGame; set => finishedGame = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public float CurrentLives { get => currentLives; set => currentLives = value; }
    public int CurrentScore { get => currentScore; set => currentScore = value; }
    public bool AtCheckpoint { get => atCheckpoint; set => atCheckpoint = value; }
    public List<int> UnlockedLevels { get => unlockedLevels; set => unlockedLevels = value; }

    /*
     *  https://forum.unity.com/threads/how-would-i-do-the-following-in-playerprefs.397516/#post-2595609 
     *  Saving player data as a json string and loading it into the PlayerPrefs.
     *  Will restore it through a jsonread
     */
    public void SavePlayer()
    {
        PlayerPrefs.SetString(saveData, JsonUtility.ToJson(this));
    }

    public static Player LoadPlayer()
    {
        return JsonUtility.FromJson<Player>(PlayerPrefs.GetString(saveData));
    }
}
