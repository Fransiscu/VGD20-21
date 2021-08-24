using Prime31.TransitionKit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuController : MonoBehaviour
{
    public GameObject gameStatsResetMenu;
    public GameObject nameInputMenu;
    public GameObject genderPicker;
    public GameObject settingsMenu;
    public GameObject levelsMenu;
    public GameObject mainMenu;

    public GameObject cygnus;

    public TMP_InputField newPlayerNameInputField;
    public TextMeshProUGUI playerNameScore;

    private static readonly string gender = DefaultValues.menuGenderSelectionPrefName;

    Animator cygnusAnimator;

    Player player;

    void Start()
    {
        if (PlayerPrefs.GetString(DefaultValues.saveDataPrefName).Equals("")) // if we have no playerdata saved yet
        {
            FirstStart();   
        }
        else
        {
            Debug.LogWarning(PlayerPrefs.GetString(DefaultValues.saveDataPrefName));
            player = new Player();
            player = Player.LoadPlayer();
            SetupInterface(player);
        }
        
    }

    public void SetupNewPlayer()
    {
        Player player = new Player();

        string selectedGender = PlayerPrefs.GetString(gender);
            
        if (newPlayerNameInputField.text.Length < 15 && newPlayerNameInputField.text.Length > 2 && !selectedGender.Equals(""))
        {
            player = new Player(newPlayerNameInputField.text, selectedGender.Equals("Male") ? Gender.MALE : Gender.FEMALE);
        }
        else
        {
            return;
        }

        nameInputMenu.SetActive(false); // hiding the name input menu
        settingsMenu.SetActive(true);   // showing main menu 

        player.SavePlayer();

        SetupInterface(player);   // setting up the rest of the interface 
    }

    private void FirstStart()
    {
        // hide standard interface at first start
        gameStatsResetMenu.SetActive(false);
        nameInputMenu.SetActive(false);
        settingsMenu.SetActive(false);
        genderPicker.SetActive(true);
        levelsMenu.SetActive(false);
        mainMenu.SetActive(false);

        playerNameScore.enabled = false;

        // show name input menu at first start
        nameInputMenu.SetActive(true);
    }

    private void SetupInterface(Player player)
    {
        cygnusAnimator = cygnus.GetComponentInChildren<Animator>();
        cygnusAnimator.Play("cygnus_stand");

        gameStatsResetMenu.SetActive(false);    // turning the game stats menu off at start
        nameInputMenu.SetActive(false);  // turning first time interface off at start
        genderPicker.SetActive(false);  // turning the gender picker off at start
        settingsMenu.SetActive(false);  // turning the settings menu off at start
        levelsMenu.SetActive(false);  // turning the levels menu off at start
        mainMenu.SetActive(true);   // turning the main menu on at start
        
        cygnusAnimator.SetBool("cygnus_wakeup", true); // setting up main menu cygnus animations 
        cygnusAnimator.SetBool("cygnus_stand", true);

        playerNameScore.enabled = true;
        playerNameScore.SetText(player.Name + " - " + player.LifeTimeScore); 

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
        SceneManager.LoadScene(1);
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

    private IEnumerator FadeToMenu()
    {
        yield return new WaitForSeconds(.1f);
        FadeTransition fadeToLevel = new FadeTransition()
        {
            duration = 1.5f,
            fadeToColor = Color.white
        };
        TransitionKit.instance.transitionWithDelegate(fadeToLevel);
    }
}
