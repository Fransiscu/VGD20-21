using Prime31.TransitionKit;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMenuController : MonoBehaviour
{
    public GameObject cygnus;
    public TextMeshProUGUI playerInterfaceName;
    public GameObject menuController;
    
    Animator cygnusAnimator;
    public Button continueButton;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button backButton;

    Player player;

    void Start()
    {
        // setup animation for the cygnus 
        cygnusAnimator = cygnus.GetComponentInChildren<Animator>();
        
        player = new Player();
        player = Player.LoadPlayer();
        
        SetupInterface();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButtonPress();
        }
    }

    private void SetupInterface()
    {
        // levels menu buttons
        continueButton.interactable = player.AtCheckpoint;
        level1Button.interactable = true;
        level2Button.interactable = player.unlockedLevels.Contains(2);
        level3Button.interactable = player.unlockedLevels.Contains(3);

        cygnusAnimator.Play("cygnus_stand");

        playerInterfaceName.enabled = true;
        playerInterfaceName.SetText(player.Name);
    }

    public void OnContinueButtonPress()
    {
        StartCoroutine("StartLevel", player.CurrentLevel + 1);
    }

    public void OnBackButtonPress()
    {
        StartCoroutine("StartLevel", 0);
    }

    public void OnLevel1ButtonPress()
    {
        ResetGameInProgress();  // if there was a game in progress then delete it
        StartCoroutine("StartLevel", 2);
    }
    
    public void OnLevel2ButtonPress()
    {
        ResetGameInProgress();  // if there was a game in progress then delete it
        StartCoroutine("StartLevel", 3);
    }
    
    public void OnLevel3ButtonPress()
    {
        ResetGameInProgress();  // if there was a game in progress then delete it
        StartCoroutine("StartLevel", 4);
    }
    
    // Start level animation coroutine method
    public IEnumerator StartLevel(int sceneNumber)
    {
        cygnusAnimator.Play("cygnus_attack");
        yield return new WaitForSeconds(1f);
        FadeTransition fadeToLevel = new FadeTransition()
        {
            nextScene = sceneNumber,
            duration = .8f,
            fadeToColor = Color.white
        };
        TransitionKit.instance.transitionWithDelegate(fadeToLevel);
    }

    // Reset current game
    private void ResetGameInProgress()
    {
        player.CurrentLives = SETTINGS.startingLives;
        player.AtCheckpoint = false;
        player.CurrentScore = 0;
        player.currentLevel = 0;
        player.SavePlayer();
    }
}
