using Prime31.TransitionKit;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusLevelController : MonoBehaviour
{
    public GameObject player;
    public GameObject GUIController;

    public TextMeshProUGUI textOnScreen;
    
    private PlayerController pController;
    private GUIController gController;

    public AudioSource music;

    GameSettings gameSettings;

    Player playerObject;

    void OnEnable()
    {
        SetUpConsumables();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        pController = player.gameObject.GetComponent<PlayerController>();
        gController = GUIController.gameObject.GetComponent<GUIController>();

        gameSettings = new GameSettings();
        gameSettings = GameSettings.LoadSettings();

        playerObject = Player.LoadPlayer();

        SetupBonusLevel();

        if (gameSettings.Music) music.volume = SETTINGS.musicVolume; else music.Stop();
    }

    public void SetupBonusLevel()
    {
        StartCoroutine("SetUp");
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

            SceneManager.LoadScene(3);  // instant transition 
            FadeTransition fadeToLevel = new FadeTransition()
            {
                duration = 1.5f,
                fadeToColor = Color.white
            };
            TransitionKit.instance.transitionWithDelegate(fadeToLevel);
        }
    }

    private IEnumerator SetUp()
    {
        gController.ChangeGUIScore(playerObject.CurrentScore);
        gController.ChangeGUILives(playerObject.CurrentLives, false);
        pController.PlayerInvincibleToggle(true);    // freezing player
        pController.PlayerFreezeToggle(true);    // freezing player
        yield return new WaitForSeconds(2f); // waiting for fade effect to finish before loading the entities
        yield return gController.BonusLevelIntroduction(textOnScreen);   // displaying countdown
        pController.PlayerInvincibleToggle(false);    // freezing player
        pController.PlayerFreezeToggle(false);    // unfreezing player
    }

    // Method to setup the consumables in the level
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
                        CoinController coin = currentGameObject.GetComponent<CoinController>();
                        coin.SetUp();
                    }
                    catch (Exception) 
                    {
                        Debug.LogWarning("Error setting up consumable");
                    }
                    break;

                case "DoubleJump":
                    try
                    {
                        DoubleJumpController doubleJump = currentGameObject.GetComponent<DoubleJumpController>();
                        doubleJump.SetUp();
                    }
                    catch (Exception)
                    {
                        Debug.LogWarning("Error setting up consumable");
                    }
                    break;

                case "SpeedUp":
                    try
                    {
                        SpeedModifierController speedModifier = currentGameObject.GetComponent<SpeedModifierController>();
                        speedModifier.SetUp();
                    }
                    catch (Exception)
                    {
                        Debug.LogWarning("Error setting up consumable");
                    }
                    break;
            }
        }
    }
}
