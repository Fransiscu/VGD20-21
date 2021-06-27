using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum Gender {MALE, FEMALE};
public class Player
{
    public string name;
    public Gender gender;
    public int currentLevel;
    public bool atCheckpoint;
    public int lifeTimeScore;
    public bool finishedGame;
    public List<int> unlockedLevels;

    public string Name { get => name; set => name = value; }
    public int LifeTimeScore { get => lifeTimeScore; set => lifeTimeScore = value; }
    public bool FinishedGame { get => finishedGame; set => finishedGame = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public bool AtCheckpoint { get => atCheckpoint; set => atCheckpoint = value; }
    public List<int> UnlockedLevels { get => unlockedLevels; set => unlockedLevels = value; }

    public Player()
    {
    }

    public Player (string name)
    {
        this.name = name;
        this.gender = Gender.MALE;
        this.currentLevel = 0;
        this.atCheckpoint = false;
        this.lifeTimeScore = 0;
        this.finishedGame = false;
        this.unlockedLevels.Add(1);
    }

    /*
     *  https://forum.unity.com/threads/how-would-i-do-the-following-in-playerprefs.397516/#post-2595609 
     *  Saving player data as a json string and loading it into the PlayerPrefs.
     *  Will restore it through a jsonread
     */
    public string SavePlayer()
    {
        return "saved";
    }
}
