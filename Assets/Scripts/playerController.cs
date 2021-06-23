﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer characterSprite;

    public AudioClip jumpSound;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip checkpointSound;
    public AudioClip victorySound;

    Player player;
    
    private float playerMovementDirection;
    public float playerMovementSpeed; 
    public int playerJumpPower;
    
    private bool facingRight = true;
    public bool touchingGround; 

    private int jumpCounter;
    private float lives;
    private float score;

    public bool doubleJumpActive;
    public bool isInvincible;
    public bool inputFrozen;

    public void Start()
    {
        Debug.Log("Starting player controller");
        player = LoadPlayer();
        SetupCurrentGame();
    }

    void Update()
    {
        if (!inputFrozen)
        {
            MovePlayer();
        }
    }

    public void MovePlayer() 
    {
        playerMovementDirection = 0;
        playerMovementDirection = Input.GetAxis("Horizontal");

        if (playerMovementDirection == 0)    // if the character is not moving
        {
            if (!touchingGround)    // and is jumping
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", false);
            }
            else    // and is not jumping
            {
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);
            }
        } 
        else    // if the character is moving
        {
            if (!animator.GetBool("isWalking")) // and if is not already walking
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", true);
            }

            if (!touchingGround)
            {
                animator.SetBool("isWalking", false);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isJumping", true);
            PlayerJump(); 
        }

        if (playerMovementDirection > 0.0f && facingRight == false)   // moving right
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isJumping", false);
            if (!touchingGround)
            {
                animator.SetBool("isWalking", false);
            }
            FlipPlayer(); 
        }
        else if (playerMovementDirection < 0.0f && facingRight == true)   // moving left
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isJumping", false);
            
            if (!touchingGround)
            {
                animator.SetBool("isWalking", false);
            }
            FlipPlayer(); 
        }

        /* 
        https://forum.unity.com/threads/velocity-time-deltatime.91518/
        No delta time: Actually, you don't want to use Time.deltaTime with rigidbody.velocity. 
        Due to the fact that it already moves your character at a speed that is framerate independent, using Time.deltaTime actually breaks things.
        It basically becomes framerate dependent again due to your velocity essentially being speed * Time.deltaTime * Time.deltaTime.
        So, just set rigidbody.velocity equal to Vector3(0,0,speed). 
        It should work just fine.
        */

        Vector2 movement = new Vector2(playerMovementDirection * playerMovementSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);  

        gameObject.GetComponent<Rigidbody2D>().velocity = movement;
        
    }

    public void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            touchingGround = false;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            touchingGround = true;
            jumpCounter = 0;
            animator.SetBool("isJumping", false); // back to the ground
        }
        else if (col.gameObject.tag == "Enemy")
        {
            if (!isInvincible)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);  // playing sound on hit
                knockBackPlayer(col, false);
            }
        }
        else if (col.gameObject.tag == "Obstacle")
        {
            if (!isInvincible)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);  // playing sound on hit
                knockBackPlayer(col, true);   // knocking always to the right
            }
            else
            {
                touchingGround = true;
                animator.SetBool("isJumping", false); 
            }
        }
        else if (col.gameObject.tag == "CheckPoint")
        {
            AudioSource.PlayClipAtPoint(checkpointSound, transform.position);
            Debug.Log("Checkpoint reached");
            // do stuff 
        }
    }

    public void knockBackPlayer(Collision2D col, bool knockForward)
    {
        float approachDirection = col.gameObject.transform.position.x - transform.position.x;

        if (knockForward)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(+SETTINGS.knockBackStrength, SETTINGS.knockBackDistance, 0));
            animator.SetBool("isJumping", true);
            touchingGround = false;
        }
        else
        {
            if (approachDirection > 0) // > 0 enemy is to the right 
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-SETTINGS.knockBackStrength, SETTINGS.knockBackDistance, 0));
                animator.SetBool("isJumping", true);
                touchingGround = false;
            }
            else if (approachDirection < 0) // < 0 enemy is to the left
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(+SETTINGS.knockBackStrength, SETTINGS.knockBackDistance, 0));
                animator.SetBool("isJumping", true);
                touchingGround = false;
            }
        }
        StartCoroutine("CharacterFlash", "hit");
        StartCoroutine("GiveInvincibilityFrames");
        StartCoroutine("FreezeInput");
    }

    public void PlayerJump()
    {
        if (doubleJumpActive && jumpCounter < 1 && !touchingGround)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * playerJumpPower);
            touchingGround = false;
            jumpCounter++;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
        else if (touchingGround)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * playerJumpPower);
            touchingGround = false;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
    }

    private IEnumerator GiveInvincibilityFrames()
    {
        isInvincible = true;

        for (float i = 0;
            i < SETTINGS.invincibilityFramesDurationSeconds; i += SETTINGS.invincibilityFramesDeltaTime)
        {
            yield return new WaitForSeconds(SETTINGS.invincibilityFramesDeltaTime);
        }

        isInvincible = false;
    }

    private IEnumerator FreezeInput()
    {
        inputFrozen = true;

        for (float i = 0; i < SETTINGS.playerHitInputFreezeDuration; i += SETTINGS.playerHitInputFreezeFramesDeltaTime)
        {
            yield return new WaitForSeconds(SETTINGS.playerHitInputFreezeFramesDeltaTime);
        }

        inputFrozen = false;
    }

    private IEnumerator CharacterFlash(String trigger)
    {
        Color startingColor = characterSprite.material.color;
        Color flashingColor;

        switch (trigger)
        {
            case "hit":
                flashingColor = Color.red;
                break;
            case "speedUp":
                flashingColor = Color.cyan;
                break;
            case "doubleJump":
                flashingColor = Color.magenta;
                break;
            default:
                flashingColor = Color.black;
                break;
        }

        for (int i = 0; i < 5; i++)
        {
            characterSprite.material.color = flashingColor;
            yield return new WaitForSeconds(SETTINGS.invincibilityFramesDeltaTime);
            characterSprite.material.color = Color.clear;
            yield return new WaitForSeconds(SETTINGS.invincibilityFramesDeltaTime);
            characterSprite.material.color = startingColor;
        }

        characterSprite.material.color = startingColor;
    }

    private IEnumerator EnableSpeedBoost(bool speedModifier)
    {
        Debug.Log("Speed boost on");

        if (speedModifier)  // true -> speed up
        {
            Debug.Log("Speed up");
            playerMovementSpeed *= SETTINGS.speedBoost;
        } 
        else
        {
            Debug.Log("Speed down");
            playerMovementSpeed *= SETTINGS.speedDeficit;
        }

        for (float i = 0; i < SETTINGS.speedBoostBuffDuration; i+= 1f)
        {
            yield return new WaitForSeconds(1f);
        }

        playerMovementSpeed = SETTINGS.basePlayerSpeed;

        Debug.Log("Speed boost off");
    }

    private IEnumerator EnableDoubleJump()
    {
        doubleJumpActive = true;
        Debug.Log("double jump on");
        
        for (float i = 0; i < SETTINGS.doubleJumpBuffDuration; i += 1f)
        {
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("double jump off");
        doubleJumpActive = false;
    }

    // other methods

    public void SpeedEditEnabler(bool modifier)
    {
        StartCoroutine("EnableSpeedBoost", modifier);
        
        if (modifier) // if speed up
        {
            StartCoroutine("CharacterFlash", "speedUp");
        }
        else
        {
            StartCoroutine("CharacterFlash", "speedDown");
        }
    }

    public void DoubleJumpEnabler()
    {
        StartCoroutine("CharacterFlash", "doubleJump");
        StartCoroutine("EnableDoubleJump");
    }

    private Player LoadPlayer()
    {
        // Load player from savefile if present
        // player.name = "ciao";
        return new Player();
    }

    public Player getPlayer()
    {
        return player;
    }

    public void SetupCurrentGame()
    {
        characterSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        playerMovementSpeed = SETTINGS.basePlayerSpeed;
        playerJumpPower = SETTINGS.basePlayerJumpPower;
        jumpCounter = 0;
        
        touchingGround = true;
        inputFrozen = false;

        EditLives(SETTINGS.startingLives);
        EditScore(0);
    }

    public void EditLives(float change)
    {
        Debug.LogWarning(isInvincible);
            Debug.LogWarning("got hit ");
        if(!isInvincible)
        {
            this.lives += change;
        }
    }

    public void EditScore(int amount)
    {
        score += amount;
        player.lifeTimeScore += amount;
        Debug.Log("total score = " + score);
    }
}
