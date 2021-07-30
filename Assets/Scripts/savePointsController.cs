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

    public bool milestoneReached;
    public float distance = 50f;
    private int currentLevel;

    Player player;
    
    void Start()
    {
        player = new Player();
        currentLevel = gameController.GetCurrentGameLevel(); // 2 scenes before the actual levels, so we subtract 1
        milestoneReached = false;  // player can only reach checkpoint once
    }

    // Update is called once per frame
    void Update()
    {
        raycastHit2D = Physics2D.Raycast(milestonePoint.position, Vector2.up, distance);

        if (raycastHit2D.collider == true && raycastHit2D.collider.tag == "Player" && !milestoneReached)
        {
            milestoneReached = true; // cannot activate milestone more than once
            player = Player.LoadPlayer();

            if (gameObject.tag == "CheckPoint") // check if at checkpoint or finishline
            {
                player.CurrentLevel = currentLevel;
                player.AtCheckpoint = true;

                AudioSource.PlayClipAtPoint(checkpointSound, transform.position);   // checkpoint sound

                player.SavePlayer();
            }
            else if (gameObject.tag == "Finishline")
            {
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

                AudioSource.PlayClipAtPoint(victorySound, transform.position);  // finishline sound

                player.SavePlayer();

                FadeTransition fadeToLevel = new FadeTransition()   // returning to level selection menu
                {
                    nextScene = 1,
                    fadedDelay = .5f,
                    duration = 1.5f,
                    fadeToColor = Color.white
                };
                TransitionKit.instance.transitionWithDelegate(fadeToLevel);
            }
            else if (gameObject.tag == "BonusLevelEntryPoint")
            {
                //TODO: implement scene saving to get back to the exact situation
                FadeTransition fadeToLevel = new FadeTransition()   // returning to level selection menu
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
