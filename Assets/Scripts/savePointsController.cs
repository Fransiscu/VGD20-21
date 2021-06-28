using Prime31.TransitionKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class savePointsController : MonoBehaviour
{
    public RaycastHit2D raycastHit2D;
    public Transform milestonePoint;

    menuController menu;

    public bool milestoneReached;
    public float distance = 50f;
    private int currentLevel;

    Player player;
    
    void Start()
    {
        player = new Player();
        player = Player.LoadPlayer();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        milestoneReached = false;  // player can only reach checkpoint once
    }

    // Update is called once per frame
    void Update()
    {
        raycastHit2D = Physics2D.Raycast(milestonePoint.position, Vector2.up, distance);
        if (raycastHit2D.collider == true && raycastHit2D.collider.tag == "Player" && !milestoneReached)
        {
            milestoneReached = true; // cannot activate milestone more than once

            if (gameObject.tag == "CheckPoint") // check if at checkpoint or finishline
            {
                player.CurrentLevel = currentLevel;
                player.AtCheckpoint = true;
                player.SavePlayer();
            }
            else if (gameObject.tag == "Finishline")
            {
                if (player.CurrentLives > 0 )
                {
                    if (currentLevel > 0 && currentLevel < 3 && !player.UnlockedLevels.Contains(currentLevel))   // unlocking next level
                    {
                        player.UnlockedLevels.Add(currentLevel + 2);    // 2 are the scenes before the lvl 1
                        player.LifeTimeScore += player.CurrentScore;
                        player.SavePlayer();
                        
                        //menu = GameObject.FindGameObjectWithTag("MenuController").GetComponent<menuController>();
                    }
                }
            }
            else if (gameObject.tag == "BonusLevelEntryPoint")
            {
                // save stuff before bonus level
            }
        }
    }
}
