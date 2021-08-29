using Prime31.TransitionKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class savePointsController : MonoBehaviour
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
        player = new Player();
        currentLevel = gameController.GetCurrentGameLevel(); // 2 scenes before the actual levels, so we subtract 1
        milestoneReached = false;  // player can only reach checkpoint once
    }

    void Update()
    {
        raycastHit2D = Physics2D.Raycast(milestonePoint.position, Vector2.up, distance);

        if (raycastHit2D.collider == true && raycastHit2D.collider.tag == "Player" && !milestoneReached)
        {
            milestoneReached = true; // cannot activate milestone more than once
            player = Player.LoadPlayer();

            if (gameObject.tag == "CheckPoint") // check if at checkpoint or finishline
            {
                Debug.LogWarning("at checkpoint");
                player.CurrentLevel = currentLevel;
                player.AtCheckpoint = true;

                AudioSource.PlayClipAtPoint(checkpointSound, transform.position);   // checkpoint sound
                player.SavePlayer();
            }
            else if (gameObject.tag == "Finishline")
            {
                if (player.InBonusLevel)
                {
                    fadeToLevel = new FadeTransition()   // returning to level selection menu
                    {
                        nextScene = 3,
                        fadedDelay = 1.5f,
                        duration = 1.5f,
                        fadeToColor = Color.white
                    };
                    TransitionKit.instance.transitionWithDelegate(fadeToLevel);
                    player.InBonusLevel = false;
                    player.ComingFromBonusLevel = true;
                    player.SavePlayer();
                    return;
                }
                
                // if last level, trigger the Player.FinishedGame boolean
                if (gameController.GetCurrentGameLevel() == 3)
                {
                    player.FinishedGame = true;
                }

                // unlocking next level if necessary
                if (currentLevel > 0 && currentLevel < 3 && !player.UnlockedLevels.Contains(currentLevel + 1))   
                {
                    player.UnlockedLevels.Add(currentLevel + 1);  
                }

                player.LifeTimeScore += player.CurrentScore;
                player.CurrentScore = 0;    // resetting current score
                player.AtCheckpoint = false;
                player.currentLives = SETTINGS.startingLives;   // resetting total lives for next level
                SaveSceneSystem.DeleteSceneSave();  // deleting any save file for the current level

                AudioSource.PlayClipAtPoint(victorySound, transform.position);  // finishline sound

                player.SavePlayer();

                fadeToLevel = new FadeTransition()   // returning to level selection menu
                {
                    nextScene = 1,
                    fadedDelay = 1.5f,
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
