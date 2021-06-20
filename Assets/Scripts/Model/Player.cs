using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name;
    public enum Gender {MALE, FEMALE};
    public int maxScore;
    public int lifeTimeScore;
    public bool finishedGame;
    public List<int> unlockedLevels;

    public string Name { get => name; set => name = value; }
    public int MaxScore { get => maxScore; set => maxScore = value; }
    public int LifeTimeScore { get => lifeTimeScore; set => lifeTimeScore = value; }
    public bool FinishedGame { get => finishedGame; set => finishedGame = value; }
    public List<int> UnlockedLevels { get => unlockedLevels; set => unlockedLevels = value; }

    public Player()
    {
    }
}
