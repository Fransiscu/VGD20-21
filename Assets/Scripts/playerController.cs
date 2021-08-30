using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class playerController : MonoBehaviour
{
    gameController gController;

    private Animator playerAnimator;
    public RuntimeAnimatorController maleCharacterAnimatorController;
    public RuntimeAnimatorController femaleCharacterAnimatorController;
    private SpriteRenderer characterSprite;
    public GUIController guiController;

    public AudioClip jumpSound;
    public AudioClip doubleJumpSound;
    public AudioClip hitSound;
    public AudioClip deathSound;

    public Player player;

    public GameSettings gameSettings;
    public float volume;

    private float playerMovementDirection;
    public float playerMovementSpeed;
    public int playerJumpPower;

    private bool facingRight = true;
    public bool touchingGround;

    private int jumpCounter;
    private float lives;

    private bool isFlashing = false;
    public bool doubleJumpActive;
    public bool isInvincible;
    public bool inputFrozen = true;
    public bool knockedBack;

    private void Awake()
    {
        player = LoadPlayer();
        SetupCurrentPlayer();
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
                playerAnimator.SetBool("isIdle", false);
                playerAnimator.SetBool("isWalking", false);
            }
            else    // and is not jumping
            {
                playerAnimator.SetBool("isIdle", true);
                playerAnimator.SetBool("isWalking", false);
            }
        }
        else    // if the character is moving
        {
            if (!playerAnimator.GetBool("isWalking")) // and if is not already walking
            {
                playerAnimator.SetBool("isIdle", false);
                playerAnimator.SetBool("isWalking", true);
            }

            if (!touchingGround)
            {
                playerAnimator.SetBool("isWalking", false);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            playerAnimator.SetBool("isIdle", false);
            playerAnimator.SetBool("isJumping", true);
            PlayerJump();
        }

        if (playerMovementDirection > 0.0f && facingRight == false)   // moving right
        {
            playerAnimator.SetBool("isIdle", false);
            playerAnimator.SetBool("isWalking", true);
            playerAnimator.SetBool("isJumping", false);
            if (!touchingGround)
            {
                playerAnimator.SetBool("isWalking", false);
            }
            FlipPlayer();
        }
        else if (playerMovementDirection < 0.0f && facingRight == true)   // moving left
        {
            playerAnimator.SetBool("isIdle", false);
            playerAnimator.SetBool("isWalking", true);
            playerAnimator.SetBool("isJumping", false);

            if (!touchingGround)
            {
                playerAnimator.SetBool("isWalking", false);
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

    public void PlayerInvincibleToggle(bool invincible)
    {
        isInvincible = invincible;
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
        switch (col.gameObject.tag)
        {
            case "Ground":
                touchingGround = true;
                jumpCounter = 0;
                playerAnimator.SetBool("isJumping", false); // back to the ground
                break;
            case "Enemy":
            case "Cygnus":
            case "Obstacle":
                EntitiesCollisionHandler(col);
                break;
            default:
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        EntitiesCollisionHandler(col);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            touchingGround = false;
        }
    }

    private void EntitiesCollisionHandler(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Enemy":
                if (!isInvincible)
                {
                    AudioSource.PlayClipAtPoint(hitSound, transform.position, volume);  // playing sound on hit
                    UpdateLives(col.gameObject.GetComponent<enemyController>().hitDamage);   
                    knockBackPlayer(col, false);
                }
                break;
            case "Cygnus":
                if (!isInvincible)
                {
                    AudioSource.PlayClipAtPoint(hitSound, transform.position, volume);  // playing sound on hit
                    UpdateLives(col.gameObject.GetComponent<cygnusController>().hitDamage);
                    knockBackPlayer(col, true);   // knocking always to the right
                }
                break;
            case "Obstacle":
                if (!isInvincible)
                {
                    AudioSource.PlayClipAtPoint(hitSound, transform.position, volume);  // playing sound on hit
                    UpdateLives(col.gameObject.GetComponent<SpikesController>().hitDamage);
                    knockBackPlayer(col, true);   // knocking always to the right
                }
                else
                {
                    // ignoring collisions with obstacle so we don't get stuck between them and cygnus
                    col.gameObject.GetComponent<SpikesController>().DisableColliderEnabler();
                    touchingGround = true;
                    playerAnimator.SetBool("isJumping", false);
                }
                break;
            default:
                break;
        }
    }

    // bool knockForward is used for some cases in which we force a forward knock, ignoring the direction of the hit
    public void knockBackPlayer(Collision2D col, bool knockForward)
    {
        float approachDirection = col.gameObject.transform.position.x - transform.position.x;
        knockedBack = true;

        if (knockForward)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(+SETTINGS.knockBackStrength, SETTINGS.knockBackDistance, 0));
            playerAnimator.SetBool("isJumping", true);
            touchingGround = false;
        }
        else
        {
            if (approachDirection > 0) // > 0 enemy is to the right 
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-SETTINGS.knockBackStrength, SETTINGS.knockBackDistance, 0));
                playerAnimator.SetBool("isJumping", true);
                touchingGround = false;
            }
            else if (approachDirection <= 0) // < 0 enemy is to the left
            {
                if (col.gameObject.tag == "Cygnus") // cygnus knocks back harder
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(+SETTINGS.cygnusKnockBackStrength, SETTINGS.cygnusKnockBackDistance, 0));
                }
                else
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(+SETTINGS.knockBackStrength, SETTINGS.knockBackDistance, 0));
                    playerAnimator.SetBool("isJumping", true);
                    touchingGround = false;
                }
            }
        }

        StartCoroutine("FreezeInput");
        StartCoroutine("CharacterFlash", "hit");
        StartCoroutine("GiveInvincibilityFrames");
        knockedBack = false;
    }

    public void PlayerJump()
    {
        // checking if the jump counter is < 1 in case of a double jump active buff
        if (doubleJumpActive && jumpCounter < 1 && !touchingGround) 
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * playerJumpPower);
            touchingGround = false;
            jumpCounter++;
            AudioSource.PlayClipAtPoint(doubleJumpSound, transform.position, volume);
        }
        else if (touchingGround)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * playerJumpPower);
            touchingGround = false;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position, volume);
        }
    }

    private IEnumerator GiveInvincibilityFrames()
    {
        new WaitForSeconds(.1f);
        isInvincible = true;

        for (float i = 0;
            i < SETTINGS.invincibilityFramesDurationSeconds; i += SETTINGS.invincibilityFramesDeltaTime)
        {
            yield return new WaitForSeconds(SETTINGS.invincibilityFramesDeltaTime);
        }

        isInvincible = false;
    }

    public void PlayerFreezeToggle(bool frozen)
    {
        inputFrozen = frozen;
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

        if (!isFlashing)    // if the character is not already flashing
        {
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
                case "speedDown":
                    flashingColor = Color.gray;
                    break;
                default:
                    Debug.LogWarning("???");
                    flashingColor = Color.black;
                    break;
            }

            for (int i = 0; i < 5; i++)
            {
                isFlashing = true;  // toggling isFlashing in case of other events trying to make the character flash
                characterSprite.material.color = flashingColor;
                yield return new WaitForSeconds(SETTINGS.invincibilityFramesDeltaTime);
                characterSprite.material.color = Color.clear;
                yield return new WaitForSeconds(SETTINGS.invincibilityFramesDeltaTime);
                characterSprite.material.color = startingColor;
                isFlashing = false; // toggling isFlashing again
            }

        characterSprite.material.color = startingColor; // setting the default color again after flashing
        }
    }

    private IEnumerator EnableSpeedBoost(bool speedModifier)
    {
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

        for (float i = 0; i < SETTINGS.speedBoostBuffDuration; i += 1f)
        {
            yield return new WaitForSeconds(1f);
        }

        playerMovementSpeed = SETTINGS.basePlayerSpeed;
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
        return Player.LoadPlayer();
    }

    public Player GetPlayer()
    {
        player = Player.LoadPlayer();
        return player;
    }

    public void SetupCurrentPlayer()
    {
        // if we are not in bonus level
        if (gameController.GetCurrentGameLevel() != 4)
        {
            gController = GameObject.Find("GameController").GetComponent<gameController>();
        }

        characterSprite = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();

        // loading specific animator according to gender
        if (player.gender == Gender.FEMALE) 
        {
            playerAnimator.runtimeAnimatorController = femaleCharacterAnimatorController;
        }
        else
        {
            playerAnimator.runtimeAnimatorController = maleCharacterAnimatorController;
        }

        playerMovementSpeed = SETTINGS.basePlayerSpeed;
        playerJumpPower = SETTINGS.basePlayerJumpPower;
        jumpCounter = 0;

        touchingGround = true;

        DisplayValues();

        gameSettings = new GameSettings();
        gameSettings = GameSettings.LoadSettings(); 
        volume = gameSettings.Sound ? SETTINGS.soundVolume : 0f;
    }

    public void DisplayValues()
    {
        guiController.ChangeGUILives(player.CurrentLives, false);
        guiController.ChangeGUIScore(player.CurrentScore, false);
    }

    // bool save -> save the score to current, false don't and just refresh the GUI
    public void IncreaseScore(int increment, bool save)
    {
        player = Player.LoadPlayer();
        if (save)
        {
            player.CurrentScore += increment;
            guiController.ChangeGUIScore(player.CurrentScore, false);
            player.SavePlayer();
        }
        else
        {
            guiController.ChangeGUIScore(increment, false);
        }
    }

    public void UpdateLives(float change)
    {
        if(!isInvincible)
        {
            player = Player.LoadPlayer();
            player.CurrentLives -= change;
            guiController.ChangeGUILives(player.CurrentLives, false);
            player.SavePlayer();
            if (player.CurrentLives <= 0)
            {
                guiController.ChangeGUILives(0, false);
                gController.DeathHandler();
            }
        }
    }

    public Boolean IsPlayerInvincible()
    {
        return isInvincible;
    }

}
