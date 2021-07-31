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
        assignCoinsValues();
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
        StartCoroutine("SetupEntities");
        SaveSceneSystem.SaveScene("0");   
    }

    void Start()
    {
        playerObject = new Player();
        playerObject = Player.LoadPlayer();
        SaveSceneSystem.LoadSceneFromObject(null);

        /*
         * If player at checkpoint at the start of the *current* level, move the position to the appropriate sign post
         * Else just set the entities up the normal way and delete temporary points (Player.CurrentScore) from previous save
         * as well as eventual checkpoint (Player.AtCheckPoint)
         */

        if (playerObject.CurrentLevel == gameController.GetCurrentGameLevel() && playerObject.AtCheckpoint)  
        {
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

    private void assignCoinsValues() 
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (object o in obj)
        {
            GameObject currentGameObject = (GameObject)o;

            if (currentGameObject.CompareTag("Coin"))
            {
                if (currentGameObject != null)
                {
                    coinController coin = currentGameObject.GetComponent<coinController>();
                    Debug.LogWarning("value = " + coin.coinValue + " - ID = " + coin.iD);
                }
            }
        }
    }
}
