using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject cygnus;
    public GameObject player;
    public GameObject GUIController;

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
