using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Controller for the in game GUI
public class GUIController : MonoBehaviour
{
    public TextMeshProUGUI textcoins;
    public TextMeshProUGUI countDown;
    public TextMeshProUGUI lives;

    float currentLives;

    /*  
     *  Changes lives displayed in the GUI
     *  Passing boolean true for subtract subtracts from current score displayed
     *  Passing boolean false for subtract simply displays the number
     */
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
        lives.SetText(" Lives: " + currentLives.ToString());
    }

    public void ChangeGUIScore(int newScoreValue)
    {
        textcoins.SetText(" Score: " + newScoreValue.ToString());
    }

    // Count down coroutine method
    public IEnumerator StartCountDown()
    {
        countDown.enabled = true;
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

    // Bonus level count down coroutine method
    public IEnumerator BonusLevelIntroduction(TextMeshProUGUI textObject)
    {
        textObject.enabled = true;
        yield return new WaitForSeconds(1.5f);
        textObject.text = "Welcome to the Bonus level!";
        yield return new WaitForSeconds(2.5f);
        textObject.text = "Gather as many coins as you can...";
        yield return new WaitForSeconds(2.6f);
        textObject.text = "But make sure you do not fall down! \n \n Ready?";
        yield return new WaitForSeconds(3f);
        textObject.text = "3";
        yield return new WaitForSeconds(1f);
        textObject.text = "2";
        yield return new WaitForSeconds(1f);
        textObject.text = "1";
        yield return new WaitForSeconds(1f);
        textObject.text = "GO!";
        yield return new WaitForSeconds(2f);
        textObject.enabled = false;
        yield return true;
    }
}
