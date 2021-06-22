using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class consumablesPicker : MonoBehaviour
{
    int coinValue;
    
    coinController coin;
    playerController playerController;
    doubleJumpController doubleJumpController;
    speedUpController speedUpController;
    public GUIController graphicsController;
    
    public AudioClip doubleJumpPickupSound;
    public AudioClip speedUpPickupSoung;
    public AudioClip coinPickupSound;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Picking consumable");
        playerController = gameObject.GetComponent<playerController>();
        
        if (col.gameObject.tag == "Coin" && !col.GetComponent<coinController>().pickedUp)
        {
            Debug.Log("Picked coin's value = " + col.GetComponent<coinController>().coinValue);

            AudioSource.PlayClipAtPoint(coinPickupSound, transform.position);

            coin = col.GetComponent<coinController>();

            coinValue = coin.coinValue;
            coin.pickedUp = true;

            graphicsController.changeGUIScore(coinValue);
            playerController.EditScore(coinValue);

            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "DoubleJump" && !col.GetComponent<doubleJumpController>().pickedUp)
        {
            AudioSource.PlayClipAtPoint(doubleJumpPickupSound, transform.position);
            playerController.DoubleJumpEnabler();

            doubleJumpController = col.GetComponent<doubleJumpController>();
            doubleJumpController.pickedUp = true;

            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "SpeedUp" && !col.GetComponent<speedUpController>().pickedUp) 
        {
            AudioSource.PlayClipAtPoint(speedUpPickupSoung, transform.position);
            playerController.SpeedEditEnabler(true);  // speed up -> true

            speedUpController = col.GetComponent<speedUpController>();
            speedUpController.pickedUp = true;

            Destroy(col.gameObject);
        }

    }
}
