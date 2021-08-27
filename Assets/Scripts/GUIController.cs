using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public TextMeshProUGUI textcoins;
    public TextMeshProUGUI countDown;
    public TextMeshProUGUI lives;

    int currentScore;
    float currentLives;
    
    void Start()
    {
        currentScore = 0;
        textcoins.SetText(currentScore.ToString());
    }

    public void ChangeGUILives(float newLivesValue, bool subtract)
    {
        if (subtract)
        {
            currentLives -= newLivesValue;
        }
        else
        {
            currentLives = newLivesValue;
        }
        lives.text = currentLives.ToString();
    }

    public void ChangeGUIScore(int newScoreValue, bool add)
    {
        if (add)
        {
            currentScore += newScoreValue;
        }
        else
        {
            currentScore = newScoreValue;
        }
        textcoins.text = currentScore.ToString();
    }

    void Update()
    {
        
    }

    public IEnumerator StartCountDown()
    {
        countDown.text = "3";
        yield return new WaitForSeconds(1f);
        countDown.text = "2";
        yield return new WaitForSeconds(1f);
        countDown.text = "1";
        yield return new WaitForSeconds(1f);
        countDown.text = "RUN!";
        yield return new WaitForSeconds(2f);
        countDown.enabled = false;
        yield return true;
    }
}
