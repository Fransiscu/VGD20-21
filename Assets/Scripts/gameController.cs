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

    public AudioSource music;

    public Transform checkpoint;

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
        StartCoroutine("SetupEntities");
    }

    void Start()
    {
        // loading player object for use
        playerObject = new Player();
        playerObject = Player.LoadPlayer();

        // setting up music according to the game settings
        gameSettings = new GameSettings();
        gameSettings = GameSettings.LoadSettings();

        if (gameSettings.Music) music.volume = SETTINGS.musicVolume; else music.Stop();

        /*
         * If player at checkpoint at the start of the *current* level, move the position to the appropriate sign post
         * Else just set the entities up the normal way and delete temporary points (Player.CurrentScore) from previous save
         * as well as eventual checkpoint (Player.AtCheckPoint)
         */

        if (playerObject.CurrentLevel != gameController.GetCurrentGameLevel())
        {
            SaveSceneSystem.DeleteSceneSave();  // deleting potential leftover saved scenes
            playerObject.currentLevel = gameController.GetCurrentGameLevel();
            playerObject.AtCheckpoint = false;
            playerObject.currentScore = 0;
            playerObject.SavePlayer();
            //StartCoroutine("SetupEntities");
        }
        else if (playerObject.CurrentLevel == gameController.GetCurrentGameLevel() && playerObject.AtCheckpoint)  
        {
            Debug.LogWarning("at checkpoint rn spawning");
            SaveSceneSystem.LoadSceneFromObject();
            player.transform.position = new Vector3(checkpoint.transform.position.x + 5, checkpoint.transform.position.y, checkpoint.transform.position.z);
            cygnus.transform.position = new Vector3(player.transform.position.x - 10, cygnus.transform.position.y, cygnus.transform.position.z);
            playerObject.SavePlayer();
            StartCoroutine("SetupEntities");
        }
        else
        {
            SaveSceneSystem.DeleteSceneSave();  // deleting potential leftover saved scenes
            playerObject.AtCheckpoint = false;
            playerObject.currentScore = 0;
            playerObject.SavePlayer();
            StartCoroutine("SetupEntities");
        }

    }

    void Update()
    {
        
    }

    IEnumerator SetupEntities()
    {
        player.gameObject.GetComponent<playerController>().PlayerFreezeToggle(true);    // freezing player
        yield return new WaitForSeconds(1f); // waiting for fade effect to finish before loading the entities
        yield return cygnus.GetComponent<cygnusController>().RiseAndShine();    // waking up cygnus + freeze
        yield return GUIController.gameObject.GetComponent<GUIController>().StartCountDown();   // displaying countdown
        player.gameObject.GetComponent<playerController>().PlayerFreezeToggle(false);    // unfreezing player
        cygnus.gameObject.GetComponent<cygnusController>().UnFreezeCygnus();    // unfreezing cygnus
    }

    public static int GetCurrentGameLevel()
    {
        return SceneManager.GetActiveScene().buildIndex - 1; // 2 scenes before the actual levels, so we subtract 1
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
