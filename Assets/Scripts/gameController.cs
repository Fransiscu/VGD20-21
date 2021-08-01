using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    public GameObject cygnus;
    public GameObject player;
    public GameObject GUIController;

    public Transform checkpoint;

    Player playerObject;
    void OnEnable()
    {
        SetUpConsumables();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
       /* if (!playerObject.AtCheckpoint)
        {
            SaveSceneSystem.DeleteSceneSave();  // if player not yet at checkpoint delete the saed consumables pickups
        }*/
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // setting up entities before game start
    {
        StartCoroutine("SetupEntities");
    }

    void Start()
    {
        playerObject = new Player();
        playerObject = Player.LoadPlayer();
        SaveSceneSystem.LoadSceneFromObject();

        /*
         * If player at checkpoint at the start of the *current* level, move the position to the appropriate sign post
         * Else just set the entities up the normal way and delete temporary points (Player.CurrentScore) from previous save
         * as well as eventual checkpoint (Player.AtCheckPoint)
         */

        if (playerObject.CurrentLevel == gameController.GetCurrentGameLevel() && playerObject.AtCheckpoint)  
        {
            Debug.LogWarning("at checkpoint rn spawning");
            player.transform.position = new Vector3(checkpoint.transform.position.x + 5, checkpoint.transform.position.y, checkpoint.transform.position.z);
            cygnus.transform.position = new Vector3(player.transform.position.x - 10, cygnus.transform.position.y, cygnus.transform.position.z);
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
        yield return new WaitForSeconds(1f); // waiting for fade effect to finish before loading the entities
        player.gameObject.GetComponent<playerController>().PlayerFreezeToggle();    // freezing player
        yield return cygnus.GetComponent<cygnusController>().RiseAndShine();    // waking up cygnus + freeze
        yield return GUIController.gameObject.GetComponent<GUIController>().StartCountDown();   // displaying countdown
        player.gameObject.GetComponent<playerController>().PlayerFreezeToggle();    // unfreezing player
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
                    coinController coin = currentGameObject.GetComponent<coinController>();
                    coin.SetUp();
                    break;

                case "DoubleJump":
                    doubleJumpController doubleJump = currentGameObject.GetComponent<doubleJumpController>();
                    doubleJump.SetUp();
                    break;

                case "SpeedUp":
                case "SpeedDown":
                    speedModifierController speedModifier = currentGameObject.GetComponent<speedModifierController>();
                    speedModifier.SetUp();
                    break;
            }
        }
    }
}
