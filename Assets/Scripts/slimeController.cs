using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = System.Random;

public class slimeController : MonoBehaviour
{
    private float slimeMovementDirection;
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = .1f;
    public int slimeSpeed = 5;
    public int slimeJumpPower = 50; // to change/powerup
    public bool facingRight = false;
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
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            MoveSlime();
            latestDirectionChangeTime = Time.time;
        }
    }

    /* 
    0 -> idle
    1 -> left
    2 -> right
    3 -> Jump 
    */
    public void MoveSlime()
    {
        Vector2 movement;
        //Random dice = new Random();
        //slimeMovementDirection = dice.Next(0, 4);
        slimeMovementDirection = 3;
        //Debug.Log(slimeMovementDirection);
        movement = new Vector2();

        float horizontalMovement = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && touchingGround)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isJumping", true);
            SlimeJump();
        }

        if (slimeMovementDirection == 0)    // if the slime is not moving
        {
            movement = new Vector2(0 * slimeSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
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
        else    // if the slime is moving
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

        if (slimeMovementDirection == 3 && touchingGround)  // jumping
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * slimeJumpPower);
            animator.SetBool("isIdle", false);
            animator.SetBool("isJumping", true);
            SlimeJump();
        }

        if (slimeMovementDirection == 2 && !facingRight)   // moving right
        {
            movement = new Vector2(1 * slimeSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isJumping", false);
            if (!touchingGround)
            {
                animator.SetBool("isWalking", false);
            }
            FlipSlime();
        }
        else if (slimeMovementDirection == 1 && facingRight)   // moving left
        {
            movement = new Vector2(-1 * slimeSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isJumping", false);

            if (!touchingGround)
            {
                animator.SetBool("isWalking", false);
            }

            FlipSlime();
        }

        /* 
        https://forum.unity.com/threads/velocity-time-deltatime.91518/
        No delta time: Actually, you don't want to use Time.deltaTime with rigidbody.velocity. 
        Due to the fact that it already moves your character at a speed that is framerate independent, using Time.deltaTime actually breaks things.
        It basically becomes framerate dependent again due to your velocity essentially being speed * Time.deltaTime * Time.deltaTime.
        So, just set rigidbody.velocity equal to Vector3(0,0,speed). 
        It should work just fine.
        */

        //movement = new Vector2(slimeMovementDirection * slimeSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        gameObject.GetComponent<Rigidbody2D>().velocity = movement;

    }   
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            touchingGround = true;
            animator.SetBool("isJumping", false); // back to the ground
        }
    }

    public void FlipSlime()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void SlimeJump()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * slimeJumpPower);
        touchingGround = false;
    }

}
