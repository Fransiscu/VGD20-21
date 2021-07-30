using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

    public Player()
    {
    }

    public Player (string name)
    {
        this.name = name;
        this.gender = Gender.MALE;
        this.lifeTimeScore = 0;
        this.finishedGame = false;
        this.currentLevel = 0;
        this.currentLives = 3;
        this.currentScore = 0;
        this.atCheckpoint = false;
        this.unlockedLevels = new List<int>() { 1 };
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
        PlayerPrefs.SetString("save_data", JsonUtility.ToJson(this));
    }

    public static Player LoadPlayer()
    {
        return JsonUtility.FromJson<Player>(PlayerPrefs.GetString("save_data"));
    }

}
