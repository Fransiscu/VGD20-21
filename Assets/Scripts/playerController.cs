using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private float playerMovementDirection;
    public int playerSpeed; 
    public int playerJumpPower; // to change/powerup
    private bool facingRight = true;
    public bool touchingGround; 
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    Player player;

    private float lives;
    private float score;
    public bool isInvincible;
    public bool inputFrozen;

    void Start()
    {
        Debug.Log("Starting player controller");
        touchingGround = true;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = LoadPlayer();
        SetupCurrentGamePlayer();
        inputFrozen = false;
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

        if (Input.GetButtonDown("Jump") && touchingGround)
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

        Vector2 movement = new Vector2(playerMovementDirection * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);  

        gameObject.GetComponent<Rigidbody2D>().velocity = movement;
        
    }

    public void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            touchingGround = true;
            animator.SetBool("isJumping", false); // back to the ground
        }
        else if (col.gameObject.tag == "Enemy")
        {
            if (!isInvincible)
            {
                knockBackPlayer(col, false);
            }
        }
        else if (col.gameObject.tag == "Obstacle")
        {
            if (!isInvincible)
            {
                knockBackPlayer(col, true);   // knocking always to the right
            }
            else
            {
                touchingGround = true;
                animator.SetBool("isJumping", false); 
            }
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
        StartCoroutine("FlashOnHit");
        StartCoroutine("GiveInvincibilityFrames");
        StartCoroutine("FreezeInput");
    }

    public void PlayerJump()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * playerJumpPower);
        touchingGround = false;
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

    private IEnumerator FlashOnHit()
    {
        for (int i = 0; i < 5; i++)
        {
            Color currentColor = spriteRenderer.material.color;
            spriteRenderer.material.color = Color.red;
            yield return new WaitForSeconds(SETTINGS.invincibilityFramesDeltaTime);
            spriteRenderer.material.color = Color.clear;
            yield return new WaitForSeconds(SETTINGS.invincibilityFramesDeltaTime);
            spriteRenderer.material.color = currentColor;
        }
    }

    // other methods

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

    public void SetupCurrentGamePlayer()
    {
        EditLives(SETTINGS.startingLives);
        EditScore(0);
    }

    public void EditLives(float change)
    {
        this.lives += change;
    }

    public void EditScore(int amount)
    {
        score += amount;
        player.lifeTimeScore += amount;
        Debug.Log("total score = " + score);
    }
}
