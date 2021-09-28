using Prime31.TransitionKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controller for the save points in game
public class SavePointsController : MonoBehaviour
{
    public RaycastHit2D raycastHit2D;
    public Transform milestonePoint;
    public AudioClip checkpointSound;
    public AudioClip victorySound;
    public AudioClip portalSound;

    public bool milestoneReached;
    public float distance = 50f;
    private int currentLevel;

    FadeTransition fadeToLevel;

    Player player;

    private void Awake()
    {
        currentLevel = GameController.GetCurrentGameLevel(); 
        milestoneReached = false;  // player can only reach checkpoint once
    }

    void Update()
    {
        // setting up a raycast to detect player walking over it
        raycastHit2D = Physics2D.Raycast(milestonePoint.position, Vector2.up, distance);

        if (raycastHit2D.collider == true && raycastHit2D.collider.tag == "Player" && !milestoneReached)
        {
            milestoneReached = true; // cannot activate milestone more than once
            player = Player.LoadPlayer();

            if (gameObject.tag == "CheckPoint") // check if at checkpoint or finishline
            {
                Debug.LogWarning("at checkpoint");
                player.LifeTimeScore += player.CurrentScore;
                player.ComingFromBonusLevel = false;
                player.CurrentLevel = currentLevel;
                player.AtCheckpoint = true;
                player.SavePlayer();

                AudioSource.PlayClipAtPoint(checkpointSound, transform.position);   // checkpoint sound
            }
            else if (gameObject.tag == "Finishline")
            {
                if (player.InBonusLevel)
                {
                    // if in bonus level go back to level 2
                    SceneManager.LoadScene(3); // instant load and fade transition
                    fadeToLevel = new FadeTransition()   
                    {
                        duration = 1.5f,
                        fadeToColor = Color.white
                    };
                    TransitionKit.instance.transitionWithDelegate(fadeToLevel);

                    player.LifeTimeScore += player.CurrentScore;
                    player.InBonusLevel = false;
                    player.ComingFromBonusLevel = true;
                    player.SavePlayer();
                    return;
                }
                
                // if last level, trigger the Player.FinishedGame boolean
                if (GameController.GetCurrentGameLevel() == 3)
                {
                    player.FinishedGame = true;
                }

                // unlocking next level if necessary
                if (currentLevel > 0 && currentLevel < 3 && !player.UnlockedLevels.Contains(currentLevel + 1))   
                {
                    player.UnlockedLevels.Add(currentLevel + 1);  
                }

                player.LifeTimeScore += player.CurrentScore;
                player.CurrentScore = 0;    // resetting current score for next level
                player.AtCheckpoint = false;
                player.InBonusLevel = false;
                player.ComingFromBonusLevel = false;
                player.currentLives = SETTINGS.startingLives;   // resetting total lives for next level
                SaveSceneSystem.DeleteSceneSave();  // deleting any save file for the current level
                player.SavePlayer();

                AudioSource.PlayClipAtPoint(victorySound, transform.position);  // finishline sound

                SceneManager.LoadScene(1);
                fadeToLevel = new FadeTransition()   // returning to level selection menu
                {
                    duration = 1.5f,
                    fadeToColor = Color.white
                };
                TransitionKit.instance.transitionWithDelegate(fadeToLevel);
            }
            else if (gameObject.tag == "BonusLevelEntryPoint")
            {
                player.InBonusLevel = true;
                player.SavePlayer();

                AudioSource.PlayClipAtPoint(portalSound, transform.position);  // portal sound

                FadeTransition fadeToLevel = new FadeTransition()   
                {
                    nextScene = 5,  // bonus level scene index = 5
                    fadedDelay = .5f,
                    duration = 1.5f,
                    fadeToColor = Color.cyan
                };
                TransitionKit.instance.transitionWithDelegate(fadeToLevel);

                Debug.LogWarning("entering bonus level");
            }
        }
    }
}
