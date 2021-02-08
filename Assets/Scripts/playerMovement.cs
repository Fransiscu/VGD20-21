using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float horizontalMovement;
    public int playerSpeed = 10; 
    public int playerJumpPower = 50; // to change/powerup
    private bool facingRight = true;
    public bool touchingGround; 

    private Animator animator;

    void Start()
    {
        touchingGround = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    public void PlayerMove() 
    {
        horizontalMovement = Input.GetAxis("Horizontal");

        if (horizontalMovement == 0)    // if the character is not moving
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
            Jump(); 
        }

        if (horizontalMovement > 0.0f && facingRight == false)   // moving right
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
        else if (horizontalMovement < 0.0f && facingRight == true)   // moving left
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

        Vector2 movement = new Vector2(horizontalMovement * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);  


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
    }

    public void Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpPower);
        touchingGround = false;
    }

}
