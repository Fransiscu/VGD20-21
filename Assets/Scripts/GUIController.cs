using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public TextMeshProUGUI textcoins;
    int currentScore;
    
    void Start()
    {
        currentScore = 0;
        textcoins.SetText(currentScore.ToString());
    }

    public void changeGUIScore(int score)
    {
        currentScore += score;
        textcoins.text = currentScore.ToString();
    }

    void Update()
    {
        
    }
}
