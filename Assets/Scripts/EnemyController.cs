using UnityEngine;

// Controller for the enemies
public class EnemyController : MonoBehaviour
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

    private void Awake()
    {
        SetupEnemy();
    }

    void Update()
    {
        // if on patrol
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
            // calculating if needs to be flipped
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
        // make enemies move
        rigidBody.velocity = new Vector2(enemySpeed * Time.fixedDeltaTime, rigidBody.velocity.y);
    }

    // Method to flip the enemies
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
        EntitiesCollisionHandler(col);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        EntitiesCollisionHandler(col);
    }

    // Collisions handler
    private void EntitiesCollisionHandler(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            float approachDirection = col.transform.position.x - transform.position.x;

            // enemy won't flip unless it damages the player on its front
            if (approachDirection <= 0 && facingRight)  // player to its left
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
        }
        // If colliding with obstacles or other enemies, simply ignore them
        else if (col.gameObject.tag == "Obstacle")
        {
            Physics2D.IgnoreCollision(enemyCollider, col.collider);
        }
        else if (col.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(enemyCollider, col.collider);
        }
    }

    // Method to setup enemies in the level
    private void SetupEnemy()
    {
        facingRight = true;
        animator = GetComponentInChildren<Animator>();
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); // flipping default enemy at start 
        onPatrolDuty = true;

        // Calculating enemy stats for the correct level
        switch (GameController.GetCurrentGameLevel())
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
