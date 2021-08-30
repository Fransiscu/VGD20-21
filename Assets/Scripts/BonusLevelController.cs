using Prime31.TransitionKit;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusLevelController : MonoBehaviour
{
    public GameObject player;
    public GameObject GUIController;

    public TextMeshProUGUI textOnScreen;
    
    private playerController pController;
    private GUIController gController;

    Player playerObject;

    void OnEnable()
    {
        SetUpConsumables();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        pController = player.gameObject.GetComponent<playerController>();
        gController = GUIController.gameObject.GetComponent<GUIController>();

        playerObject = Player.LoadPlayer();

        Debug.LogWarning(player.ToString());

        SetupBonusLevel();
    }

    public void SetupBonusLevel()
    {
        StartCoroutine("SetUp");
    }

    private IEnumerator SetUp()
    {
        gController.ChangeGUIScore(playerObject.CurrentScore, false);
        gController.ChangeGUILives(playerObject.CurrentLives, false);
        pController.PlayerInvincibleToggle(true);    // freezing player
        pController.PlayerFreezeToggle(true);    // freezing player
        yield return new WaitForSeconds(2f); // waiting for fade effect to finish before loading the entities
        yield return gController.BonusLevelIntroduction(textOnScreen);   // displaying countdown
        pController.PlayerInvincibleToggle(false);    // freezing player
        pController.PlayerFreezeToggle(false);    // unfreezing player
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    void Update()
    {
        // if the player falls down he goes back to the normal level
        if (player.transform.position.y <= -10)
        {
            playerObject.InBonusLevel = false;
            playerObject.ComingFromBonusLevel = true;
            playerObject.SavePlayer();
            Debug.LogWarning("dentro " + playerObject.ToString());
            SceneManager.LoadScene(3);  // instant transition 
            FadeTransition fadeToLevel = new FadeTransition()
            {
                duration = 1.5f,
                fadeToColor = Color.white
            };
            TransitionKit.instance.transitionWithDelegate(fadeToLevel);
        }
    }

    private void SetUpConsumables()
    {
        object[] objectsInScene = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (object item in objectsInScene)
        {
            GameObject currentGameObject = (GameObject)item;
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
