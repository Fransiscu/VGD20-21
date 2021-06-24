using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public TextMeshProUGUI textcoins;
    public TextMeshProUGUI countDown;

    bool countDownOver;
    int currentScore;
    
    void Start()
    {
        countDownOver = false;
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
