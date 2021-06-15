using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = System.Random;

public class slimeController : MonoBehaviour
{
    public float walkSpeed;
    public bool mustPatrol;
    public bool facingRight;
    public Rigidbody2D rb;
    private bool mustTurn;
    public Transform groundCheckpos;
    public LayerMask groundLayer;
    private Animator animator;
    void Start()
    {
        facingRight = true;
        animator = GetComponentInChildren<Animator>();
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); // flipping default slime at start 
        mustPatrol = true;
    }

    void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }
        else if (!mustPatrol)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }

    }
    void FixedUpdate()
    {
        if (mustPatrol)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            mustTurn = !Physics2D.OverlapCircle(groundCheckpos.position, 0.1f, groundLayer);
        }
        else if (!mustPatrol)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }
    }
    void Patrol()
    {
        if (mustTurn)
        {
            animator.SetBool("isWalking", true);
            Flip();
        }
        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }
    void Flip()
    {
        mustPatrol = false;
        facingRight = !facingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustPatrol = true;
    }
}
