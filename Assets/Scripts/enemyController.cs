using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = System.Random;

public class enemyController : MonoBehaviour
{
    public Transform groundPresenceChecker;
    public LayerMask groundLayerMask;
    public LayerMask wallsLayerMask;
    public Collider2D enemyCollider;
    public Rigidbody2D rigidBody;
    private Animator animator;
    private bool needsFlipping;
    public bool onPatrolDuty;
    public float enemySpeed;
    public bool facingRight;
    public float hitDamage;

    void Start()
    {
        SetupEnemy();
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
        if (needsFlipping || enemyCollider.IsTouchingLayers(wallsLayerMask))
        {
            FlipEnemy();
        }

        rigidBody.velocity = new Vector2(enemySpeed * Time.fixedDeltaTime, rigidBody.velocity.y);
    }

    private void FlipEnemy()
    {
        onPatrolDuty = false;
        facingRight = !facingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        enemySpeed *= -1;
        onPatrolDuty = true;
        needsFlipping = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.LogWarning(col.gameObject.tag);
        if (col.gameObject.tag == "Player")
        {
            float approachDirection = col.transform.position.x - transform.position.x;

            // enemy won't flip unless it damages the player on its front
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
        else if (col.gameObject.tag == "Obstacle")
        {
            Physics2D.IgnoreCollision(enemyCollider, col.collider);
        }
    }

    private void SetupEnemy()
    {
        facingRight = true;
        animator = GetComponentInChildren<Animator>();
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); // flipping default enemy at start 
        onPatrolDuty = true;

        // calculating enemy damage for every level
        switch (gameController.GetCurrentGameLevel())
        {
            case 1:
                hitDamage = SETTINGS.level1EnemyDamage;
                enemySpeed = SETTINGS.level1EnemySpeed;
                break;
            case 2:
                hitDamage = SETTINGS.level2EnemyDamage;
                enemySpeed = SETTINGS.level2EnemySpeed;
                break;
            case 3:
                hitDamage = SETTINGS.level3EnemyDamage;
                enemySpeed = SETTINGS.level3EnemySpeed;
                break;
            default:
                Debug.LogWarning("???");
                break;
        }
    }
}
