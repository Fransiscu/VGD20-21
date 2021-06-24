using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = System.Random;

public class slimeController : MonoBehaviour
{
    public Transform groundPresenceChecker;
    public LayerMask groundLayerMask;
    public LayerMask wallsLayerMask;
    public Collider2D slimeCollider;
    public Rigidbody2D rigidBody;
    private Animator animator;
    private bool needsFlipping;
    public bool onPatrolDuty;
    public float slimeSpeed;
    public bool facingRight;
    public float hitDamage;

    void Start()
    {
        SetupSlime();
    }

    void Update()
    {
        if (onPatrolDuty)
        {
            Patrol();
        }
        else if (!onPatrolDuty)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }

    }
    void FixedUpdate()
    {
        if (onPatrolDuty)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            needsFlipping = !Physics2D.OverlapCircle(groundPresenceChecker.position, 0.1f, groundLayerMask);
        }
        else if (!onPatrolDuty)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }
    }
    private void Patrol()
    {
        if (needsFlipping || slimeCollider.IsTouchingLayers(wallsLayerMask))
        {
            FlipSlime();
        }
        rigidBody.velocity = new Vector2(slimeSpeed * Time.fixedDeltaTime, rigidBody.velocity.y);
    }
    private void FlipSlime()
    {
        onPatrolDuty = false;
        facingRight = !facingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        slimeSpeed *= -1;
        onPatrolDuty = true;
        needsFlipping = false;
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            float approachDirection = col.transform.position.x - transform.position.x;

            // slime won't flip unless it damages the player on its front
            if (approachDirection < 0 && facingRight)  // player to its left
            {
                needsFlipping = false;
            } 
            else if (approachDirection > 0 && !facingRight)  // player to its right
            {
                needsFlipping = false;
            }
            else 
            {
                needsFlipping = true;
            }

            // dealing damage
            col.gameObject.GetComponent<playerController>().EditLives(hitDamage);
        }   
    }

    private void SetupSlime()
    {
        facingRight = true;
        animator = GetComponentInChildren<Animator>();
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); // flipping default slime at start 
        onPatrolDuty = true;
        hitDamage = SETTINGS.slimeHitDamage;
    }
}
