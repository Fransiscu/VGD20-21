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
        facingRight = true;
        animator = GetComponentInChildren<Animator>();
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); // flipping default slime at start 
        onPatrolDuty = true;
        hitDamage = SETTINGS.slimeHitDamage;
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
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Slime colliding with player");
            collision.gameObject.GetComponent<playerController>().EditLives(hitDamage);
        }   
    }
}
