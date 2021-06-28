using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject cygnus;
    public GameObject player;
    public GameObject GUIController;

    public Transform checkpoint;

    Player playerObject;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
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
        
        /*
         * If player at checkpoint move the position to the checkpoint sign post
         */
        if (playerObject.AtCheckpoint)  
        {
            player.transform.position = checkpoint.transform.position;
            cygnus.transform.position = new Vector3(player.transform.position.x - 10, cygnus.transform.position.y, cygnus.transform.position.z);
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

}
