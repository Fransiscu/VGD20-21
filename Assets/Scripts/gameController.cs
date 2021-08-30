using Prime31.TransitionKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    public GameObject cygnus;
    public GameObject player;
    public GameObject GUIController;
    
    private playerController pController;
    private cygnusController cController;
    private GUIController gController;

    public GameObject deathScene;

    public AudioSource music;

    public Transform checkpoint;
    public Transform bonusPortal;

    GameSettings gameSettings;
    Player playerObject;

    void OnEnable()
    {
        SetUpConsumables();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // setting up entities before game start
    {
        // triggering the savescene feature one time at the beginning,
        // more info as of why in the implementation
        SaveSceneSystem.SaveScene("1234");
    }

    private void Awake()
    {
        // make sure the game is running at its normal  time when back to the menu
        Time.timeScale = 1f;    

        // loading player object for use
        playerObject = Player.LoadPlayer();

        // setting death scene off by default
        deathScene.SetActive(false);

        // setting up music according to the game settings
        gameSettings = new GameSettings();
        gameSettings = GameSettings.LoadSettings();

        // setting up controllers
        pController = player.gameObject.GetComponent<playerController>();
        cController = cygnus.gameObject.GetComponent<cygnusController>();
        gController = GUIController.gameObject.GetComponent<GUIController>();

        if (gameSettings.Music) music.volume = SETTINGS.musicVolume; else music.Stop();
    }

    private void Start()
    {
        /*
        * If player at checkpoint at the start of the *current* level, move the position to the appropriate sign post
        * Else just set the entities up the normal way and delete temporary points (Player.CurrentScore) from previous save
        * as well as eventual checkpoint or other bool values
        */

        // if we're starting a brand new level
        if (playerObject.CurrentLevel != gameController.GetCurrentGameLevel())
        {
            SaveSceneSystem.DeleteSceneSave();  // deleting potential leftover saved scenes
            playerObject.CurrentLevel = gameController.GetCurrentGameLevel();
            playerObject.AtCheckpoint = false;
            playerObject.CurrentScore = 0;
            playerObject.SavePlayer();

            StartCoroutine("SetupEntities");
        }
        // if we are at the checkpoint of the current level
        else if (playerObject.CurrentLevel == gameController.GetCurrentGameLevel() && playerObject.AtCheckpoint)
        {
            Debug.LogWarning("at checkpoint rn spawning");
            player.transform.position = new Vector3(checkpoint.transform.position.x + 5, checkpoint.transform.position.y, 250);
            cygnus.transform.position = new Vector3(player.transform.position.x - 10, cygnus.transform.position.y, cygnus.transform.position.z);
            // if the player died
            if (playerObject.currentLives <= 0)
            {
                playerObject.currentLives = SETTINGS.startingLives;
                playerObject.SavePlayer();
            }
            SaveSceneSystem.LoadSceneFromObject();
            StartCoroutine("SetupEntities");
        }
        // spawning after bonus level
        else if (playerObject.CurrentLevel == 2 && gameController.GetCurrentGameLevel() == 2 && playerObject.ComingFromBonusLevel)
        {
            playerObject.ComingFromBonusLevel = true;
            playerObject.InBonusLevel = false;
            player.transform.position = new Vector3(bonusPortal.transform.position.x + 5, bonusPortal.transform.position.y, 250);
            cygnus.transform.position = new Vector3(player.transform.position.x - 15, cygnus.transform.position.y, cygnus.transform.position.z);
            playerObject.SavePlayer();

            SaveSceneSystem.LoadSceneFromObject();
            StartCoroutine("SetupEntities");
        }
        // same level no checkpoint no bonus no nothing
        else
        {
            SaveSceneSystem.DeleteSceneSave();  // deleting potential leftover saved scenes
            playerObject.CurrentLevel = gameController.GetCurrentGameLevel();
            playerObject.AtCheckpoint = false;
            playerObject.ComingFromBonusLevel = false;
            playerObject.InBonusLevel = false;
            playerObject.CurrentScore = 0;
            playerObject.SavePlayer();

            StartCoroutine("SetupEntities");
        }

        gController.ChangeGUILives(playerObject.CurrentLives, false);
        gController.ChangeGUIScore(playerObject.CurrentScore, false);
    }
    IEnumerator SetupEntities()
    {
        pController.PlayerInvincibleToggle(true);    // freezing player
        pController.PlayerFreezeToggle(true);    // freezing player
        yield return new WaitForSeconds(1f); // waiting for fade effect to finish before loading the entities
        yield return cController.RiseAndShine();    // waking up cygnus + freeze
        yield return gController.StartCountDown();   // displaying countdown
        pController.PlayerInvincibleToggle(false);    // freezing player
        pController.PlayerFreezeToggle(false);    // unfreezing player
        cController.UnFreezeCygnus();    // unfreezing cygnus
    }

    public static int GetCurrentGameLevel()
    {
        return SceneManager.GetActiveScene().buildIndex - 1; // 2 scenes before the actual levels, so we subtract 1
    }

    public void DeathHandler()
    {
        Debug.LogWarning("Dead");
        StartCoroutine("DeathSceneTransition");
    }

    private IEnumerator DeathSceneTransition()
    {
        yield return new WaitForSeconds(.5f);
        FadeTransition fadeToDeahScreen = new FadeTransition()
        {
            duration = 3f,
            fadeToColor = Color.black
        };
        TransitionKit.instance.transitionWithDelegate(fadeToDeahScreen);
        yield return new WaitForSeconds(1f);
        deathScene.SetActive(true);
        Time.timeScale = 0;
    }

    private void SetUpConsumables() 
    {
        object[] objectsInScene = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (object item in objectsInScene)
        {
            GameObject currentGameObject = (GameObject) item;
            string consumableTag = currentGameObject.tag;

            switch (consumableTag)
            {
                case "Coin":
                case "BiggerCoin":
                    try
                    {
                        coinController coin = currentGameObject.GetComponent<coinController>();
                        coin.SetUp();
                    }
                    catch (Exception) { }
                    break;

                case "DoubleJump":
                    try
                    {
                        doubleJumpController doubleJump = currentGameObject.GetComponent<doubleJumpController>();
                        doubleJump.SetUp();
                    }
                    catch (Exception) { }
                    break;

                case "SpeedUp":
                case "SpeedDown":
                    try
                    {
                        speedModifierController speedModifier = currentGameObject.GetComponent<speedModifierController>();
                        speedModifier.SetUp();
                    }
                    catch (Exception) { }
                    break;
            }
        }
    }
}
