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
    void Start()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); // flipping default slime at start
        mustPatrol = true;
    }

    void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }

    }
    void FixedUpdate()
    {
        if (mustPatrol)
        {
            Debug.Log("in fixed update");
            mustTurn = !Physics2D.OverlapCircle(groundCheckpos.position, 0.1f, groundLayer);
        }
    }
    void Patrol()
    {
        if (mustTurn)
        {
            Debug.Log("Turning");
            Flip();
        }
        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
        Debug.Log("Moving");
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
