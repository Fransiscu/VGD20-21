using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cygnusController : MonoBehaviour
{
    public Transform player;
    Animator cygnusAnimator;
    Rigidbody2D cygnusRigidBody;
    Collider2D cygnusCollider;

    public float yMin;
    public float yMax;

    public float cygnusSpeed;
    public float hitDamage;

    private void Awake()
    {
        setupCygnus();
    }
    void Update()
    {
        MoveCygnus();
        KeepCygnusOnPlayersYAxis();
    }

    private void KeepCygnusOnPlayersYAxis()
    {
        // making the cygnus follow our player in its y axis but at the same time never get under the ground level
        float y = Mathf.Clamp(player.transform.position.y, yMin, yMax);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
    }

    private void setupCygnus()
    {
        hitDamage = SETTINGS.cygnusDamage;
        cygnusAnimator = GetComponentInChildren<Animator>();
        cygnusRigidBody = GetComponent<Rigidbody2D>();
        cygnusCollider = GetComponent<Collider2D>();
    }

    private void MoveCygnus()
    {
        cygnusRigidBody.velocity = new Vector2(cygnusSpeed * Time.fixedDeltaTime, cygnusRigidBody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        EntitiesCollisionHandler(col);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        EntitiesCollisionHandler(col);
    }

    private void EntitiesCollisionHandler(Collision2D col)
    {
        // ignoring all collisions but with Player
        if (!(col.gameObject.tag == "Player"))
        {
            Physics2D.IgnoreCollision(col.collider, cygnusCollider);
        }
        else
        {
            StartCoroutine("AttackPlayerAnimationHandler");
        }
    }

    public IEnumerator RiseAndShine()
    {
        cygnusSpeed = 50f;
        yield return new WaitForSeconds(1f);
        cygnusAnimator.SetBool("cygnus_wakeup", true);
        yield return new WaitForSeconds(1f);
        cygnusAnimator.SetBool("cygnus_stand", true);
        yield return new WaitForSeconds(1f);
        cygnusAnimator.SetBool("cygnus_move", true);
        yield return new WaitForSeconds(1f);
        FreezeCygnus();
    }

    private IEnumerator AttackPlayerAnimationHandler()
    {
        cygnusAnimator.SetBool("cygnus_move", false);
        cygnusAnimator.SetTrigger("cygnus_attack");
        yield return new WaitForSeconds(2.5f);
        cygnusAnimator.ResetTrigger("cygnus_attack");
        cygnusAnimator.SetBool("cygnus_move", true);
    }
    public void FreezeCygnus()
    {
        cygnusSpeed = 0f;
    }

    public void UnFreezeCygnus()
    {
        cygnusSpeed = SETTINGS.cygnusSpeed;
    }
}
