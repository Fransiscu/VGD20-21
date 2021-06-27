using Prime31.TransitionKit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuController : MonoBehaviour
{
    public GameObject gameStatsResetMenu;
    public GameObject nameInputMenu;
    public GameObject settingsMenu;
    public GameObject levelsMenu;
    public GameObject mainMenu;

    public TMP_InputField newPlayerNameInputField;
    public TextMeshProUGUI playerNameScore;

    public Button continueButton;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    
    public GameObject cygnus;
    
    Animator cygnusAnimator;

    void Start()
    {
        if (PlayerPrefs.GetString("save_data").Equals("")) // if we have no playerdata saved yet
        {
            FirstStart();   
            Debug.LogWarning("player not found");
        }
        else
        {
            Debug.LogWarning("player found");
            SetupInterface();
        }
        
    }

    public void SetupNewPlayer()
    {
        Player player = new Player();
        player.Name = newPlayerNameInputField.text;
    }

    private void FirstStart()
    {
        // hide standard interface at first start
        gameStatsResetMenu.SetActive(false); 
        nameInputMenu.SetActive(false);
        settingsMenu.SetActive(false);
        levelsMenu.SetActive(false);
        mainMenu.SetActive(false);

        // show name input menu at first start
        nameInputMenu.SetActive(true);
        playerNameScore.enabled = false;
    }

    private void SetupInterface()
    {
        cygnusAnimator = cygnus.GetComponentInChildren<Animator>();

        gameStatsResetMenu.SetActive(false);    // turning the game stats menu off at start
        settingsMenu.SetActive(false);  // turning the settings menu off at start
        levelsMenu.SetActive(false);  // turning the levels menu off at start
        
        cygnusAnimator.SetBool("cygnus_wakeup", true); // setting up main menu cygnus animations 
        cygnusAnimator.SetBool("cygnus_stand", true);
    }

    public void QuitGame()
    {
        FadeTransition fadeToLevel = new FadeTransition()
        {
            fadedDelay = .5f,
            duration = 1.5f,
            fadeToColor = Color.black
        };
        TransitionKit.instance.transitionWithDelegate(fadeToLevel);
        Application.Quit();
    }

    public void OnPlayButtonPress()
    {
        mainMenu.SetActive(false);
        // TODO check what levels are unlocked or not
        level2Button.interactable = false;
        level3Button.interactable = false;
    }

    public void OnLevel1ButtonPress()
    {
        FadeTransition fadeToLevel = new FadeTransition()
        {
            nextScene = 1,
            fadedDelay = .5f,
            duration = 1.5f,
            fadeToColor = Color.white
        };
        TransitionKit.instance.transitionWithDelegate(fadeToLevel);
    }
 
    public IEnumerator StartingGameAnimation()
    {
        cygnusAnimator.SetBool("cygnus_attack", true);
        yield return new WaitForSeconds(2f);
        // start game
    }

    public void ResetGameStats()
    {
        PlayerPrefs.DeleteAll();
        FadeTransition fadeToLevel = new FadeTransition()
        {
            fadedDelay = .5f,
            duration = 1.5f,
            fadeToColor = Color.black
        };
        TransitionKit.instance.transitionWithDelegate(fadeToLevel);
        QuitGame();
    }
}
