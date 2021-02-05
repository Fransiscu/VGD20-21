using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public int playerSpeed = 10; 
    private bool facingRight = true;
    public int playerJumpPower = 350; // to change/powerup
    private float horizontalMovement;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    public void PlayerMove() 
    {
        horizontalMovement = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && touchingGround)
        {
            Jump(); // TODO
        }

        if (horizontalMovement > 0.0f && facingRight == false)   // moving right
        {
            FlipPlayer(); // TODO
        }
        else if (horizontalMovement < 0.0f && facingRight == true)   // moving left
        {
            FlipPlayer(); // TODO
        }

        /* 
        https://forum.unity.com/threads/velocity-time-deltatime.91518/
        No delta time: Actually, you don't want to use Time.deltaTime with rigidbody.velocity. 
        Due to the fact that it already moves your character at a speed that is framerate independent, using Time.deltaTime actually breaks things.
        It basically becomes framerate dependent again due to your velocity essentially being speed * Time.deltaTime * Time.deltaTime.
        So, just set rigidbody.velocity equal to Vector3(0,0,speed). 
        It should work just fine.
        */

        Vector2 movement = new Vector2(horizontalMovement * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);  


        gameObject.GetComponent<Rigidbody2D>().velocity = movement;
        
    }



}
