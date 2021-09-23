using Prime31.TransitionKit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject gameStatsResetMenu;
    public GameObject nameInputMenu;
    public GameObject genderPicker;
    public GameObject settingsMenu;
    public GameObject mainMenu;

    public GameObject cygnus;

    public TMP_InputField newPlayerNameInputField;
    public TextMeshProUGUI playerInterfaceName;
    public Button soundSettingToggle;
    public Button musicSettingToggle;

    public AudioSource music;

    Animator cygnusAnimator;

    GameSettings gameSettings;
    Player player;

    public float musicFadeTimer = 1.5f;
    private bool inSettingsMenu;

    private static readonly string gender = PlayerPrefsKey.menuGenderSelectionPrefName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inSettingsMenu)
            {
                mainMenu.SetActive(true);
                settingsMenu.SetActive(false);
                inSettingsMenu = false;
            }
            else
            {
                QuitGame();
            }
        }
    }

    private void Awake()
    {
        Time.timeScale = 1f;    // make sure the game is running at its normal  time when back to the menu
    }

    void Start()
    {
        inSettingsMenu = false;
        if (PlayerPrefs.GetString(PlayerPrefsKey.saveDataPrefName).Equals("")) // if we have no playerdata saved yet
        {
            FirstStart();
        }
        else
        {
            gameSettings = new GameSettings();
            gameSettings = GameSettings.LoadSettings();
            player = new Player();
            player = Player.LoadPlayer();
            SetupInterface(player, gameSettings);
        }

        // setting up music according to the game settings
        gameSettings = new GameSettings();
        gameSettings = GameSettings.LoadSettings();
        SetupMusic();
    }

    public void SetupNewPlayer()
    {
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

        SetupInterface(player, gameSettings);   // setting up the rest of the interface 
    }

    private void FirstStart()
    {
        // setting up game settings at first start 
        gameSettings = new GameSettings(true, true);
        gameSettings.SaveSettings();

        // hide standard interface at first start
        gameStatsResetMenu.SetActive(false);
        nameInputMenu.SetActive(false);
        settingsMenu.SetActive(false);
        genderPicker.SetActive(true);
        mainMenu.SetActive(false);

        playerInterfaceName.enabled = false;

        // show name input menu at first start
        nameInputMenu.SetActive(true);
    }

    private void SetupInterface(Player player, GameSettings gameSettings)
    {
        cygnusAnimator = cygnus.GetComponentInChildren<Animator>();
        cygnusAnimator.Play("cygnus_stand");

        soundSettingToggle.GetComponentInChildren<TextMeshProUGUI>().SetText(gameSettings.Sound == true ? "Sound On" : "Sound Off");
        musicSettingToggle.GetComponentInChildren<TextMeshProUGUI>().SetText(gameSettings.Music == true ? "Music On" : "Music Off");

        gameStatsResetMenu.SetActive(false);    // turning the game stats menu off at start
        nameInputMenu.SetActive(false);  // turning first time interface off at start
        genderPicker.SetActive(false);  // turning the gender picker off at start
        settingsMenu.SetActive(false);  // turning the settings menu off at start
        mainMenu.SetActive(true);   // turning the main menu on at start
        
        cygnusAnimator.SetBool("cygnus_wakeup", true); // setting up main menu cygnus animations 
        cygnusAnimator.SetBool("cygnus_stand", true);

        playerInterfaceName.enabled = true;
        playerInterfaceName.SetText(player.Name); 

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

    public void OnSettingsButtonPress()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        inSettingsMenu = true;
    }

    public void OnPlayButtonPress()
    {
        SceneManager.LoadScene(1);
        /*
        StartCoroutine("FadeToLevelMenu");
        StartCoroutine("FadeToMenu");
    */
    }

    public void OnSoundToggleButtonPress()
    {
        gameSettings.Sound = !gameSettings.Sound;
        soundSettingToggle.GetComponentInChildren<TextMeshProUGUI>().SetText(gameSettings.Sound == true ? "Sound On" : "Sound Off");
        gameSettings.SaveSettings();
    }

    public void OnMusicToggleButtonPress()
    {
        gameSettings.Music = !gameSettings.Music;
        musicSettingToggle.GetComponentInChildren<TextMeshProUGUI>().SetText(gameSettings.Music == true ? "Music On" : "Music Off");
        gameSettings.SaveSettings();

        if (!gameSettings.Music) 
        {
            music.Stop(); 
        }
        else
        {
            music.volume = SETTINGS.musicVolume;
            music.Play(); 
        }
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

    private void SetupMusic()
    {
        if (gameSettings.Music)
        {
            music.volume = SETTINGS.musicVolume;
            music.Play();
        }
        else
        {
            music.Stop();
        }
    }

    private IEnumerator FadeToLevelMenu()
    {
        for (float i = 0; i < 1.5f; i += 1f)
        {

            if (musicFadeTimer > 0)
            {
                music.volume -= 0.015f;
                musicFadeTimer -= Time.deltaTime;
            }
            if (music.volume == 0)
            {
                break;
            }
            yield return new WaitForSeconds(.5f);
        }
        SceneManager.LoadScene(1);
    }

    private IEnumerator FadeToMenu()
    {
        yield return new WaitForSeconds(.1f);
        FadeTransition fadeToLevel = new FadeTransition()
        {
            duration = .8f,
            fadeToColor = Color.white
        };
        TransitionKit.instance.transitionWithDelegate(fadeToLevel);
    }

}
