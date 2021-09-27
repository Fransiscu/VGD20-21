using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Gender {MALE, FEMALE};
    
// Player class to handle player object in game
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
    public bool comingFromBonusLevel;
    public bool inBonusLevel;
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
        currentLives = 0;
        currentScore = 0;
        atCheckpoint = false;
        inBonusLevel = false;
        comingFromBonusLevel = false;
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
    public bool ComingFromBonusLevel { get => comingFromBonusLevel; set => comingFromBonusLevel = value; }
    public bool InBonusLevel { get => inBonusLevel; set => inBonusLevel = value; }
    public List<int> UnlockedLevels { get => unlockedLevels; set => unlockedLevels = value; }

    /*
     *  https://forum.unity.com/threads/how-would-i-do-the-following-in-playerprefs.397516/#post-2595609 
     *  Saving player data as a json string and loading it into the PlayerPrefs.
     *  Will restore it through a jsonread method
     */
    public void SavePlayer()
    {
        PlayerPrefs.SetString(saveData, JsonUtility.ToJson(this));
    }

    // Reading player save from json string
    public static Player LoadPlayer()
    {
        return JsonUtility.FromJson<Player>(PlayerPrefs.GetString(saveData));
    }

    public override string ToString()
    {
        return "name = " + name + " level = " + CurrentLevel + " coming from bonus = " + ComingFromBonusLevel + " checkpoint = " + AtCheckpoint;
    }
}
