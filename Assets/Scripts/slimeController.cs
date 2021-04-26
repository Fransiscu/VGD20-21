using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = System.Random;

public class slimeController : MonoBehaviour
{
    public float slimeSpeed;
    public bool facingRight = false;
    public Transform groundDetection;
    private float latestDirectionChangeTime;
    private readonly double directionChangeTime = 1.5;
    private float slimeMovementDirection;
    public int slimeJumpPower; // to change/powerup
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
        Vector2 movement = new Vector2();
        Random dice = new Random();
        slimeMovementDirection = dice.Next(0, 4);
        //slimeMovementDirection = 3;

        Debug.Log(slimeMovementDirection);

        if (slimeMovementDirection == 3)  // result == jump
        {
            if (touchingGround) // if slime on the ground
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("isJumping", true);
                SlimeJump();
            }
            else // if not on the ground
            {
                // TODO: add here double jump code
                //animator.SetBool("isIdle", false);
                //animator.SetBool("isWalking", false);
                //animator.SetBool("isJumping", true);
            }
            return;
        }
        else
        {
            if (slimeMovementDirection == 0)    // result == idle
            {
                if (!touchingGround)    // if is jumping
                {
                    animator.SetBool("isIdle", true);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isJumping", false);
                }
                else    // if is not jumping
                {
                    animator.SetBool("isIdle", true);
                    animator.SetBool("isWalking", false);
                }

                //movement = new Vector2(0 * slimeSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }

            if (slimeMovementDirection == 2)   // moving right
            {
                if (!facingRight)
                {
                    FlipSlime();
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isJumping", false);
                }

                if (!touchingGround)
                {
                    animator.SetBool("isWalking", false);
                }
                else
                {
                    animator.SetBool("isWalking", true);
                }

                movement = new Vector2(1 * slimeSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            else if (slimeMovementDirection == 1)   // moving left
            {
                if (facingRight)
                {
                    FlipSlime();
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isJumping", false);
                }

                if (!touchingGround)
                {
                    animator.SetBool("isWalking", false);
                }
                else
                {
                    animator.SetBool("isWalking", true);
                }

                movement = new Vector2(-1 * slimeSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.x);
            }

            gameObject.GetComponent<Rigidbody2D>().velocity = movement;
        }


        /* 
        https://forum.unity.com/threads/velocity-time-deltatime.91518/
        No delta time: Actually, you don't want to use Time.deltaTime with rigidbody.velocity. 
        Due to the fact that it already moves your character at a speed that is framerate independent, using Time.deltaTime actually breaks things.
        It basically becomes framerate dependent again due to your velocity essentially being speed * Time.deltaTime * Time.deltaTime.
        So, just set rigidbody.velocity equal to Vector3(0,0,speed). 
        It should work just fine.
        */
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
        //gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.up * slimeJumpPower;
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * slimeJumpPower); // vector 3 or 2?
        //gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * slimeJumpPower);
        touchingGround = false;
    }

}