using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = System.Random;

public class slimeController : MonoBehaviour
{
    public float slimeSpeed;
    public bool onPatrolDuty;
    public bool facingRight;
    public Rigidbody2D rigidBody;
    private bool needsFlipping;
    public Transform groundPresenceChecker;
    public LayerMask groundLayerMask;
    public LayerMask wallsLayerMask;
    private Animator animator;
    public Collider2D slimeCollider;

    void Start()
    {
        facingRight = true;
        animator = GetComponentInChildren<Animator>();
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); // flipping default slime at start 
        onPatrolDuty = true;
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
    void Patrol()
    {
        if (needsFlipping || slimeCollider.IsTouchingLayers(wallsLayerMask))
        {
            FlipSlime();
        }
        rigidBody.velocity = new Vector2(slimeSpeed * Time.fixedDeltaTime, rigidBody.velocity.y);
    }
    void FlipSlime()
    {
        onPatrolDuty = false;
        facingRight = !facingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        slimeSpeed *= -1;
        onPatrolDuty = true;
    }
}
