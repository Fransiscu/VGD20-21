using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class consumablesPicker : MonoBehaviour
{
    int coinValue;
    coinController coin;
    playerController playerController;
    public GUIController graphicsController;
    public AudioClip coinPickupSound;
    public AudioClip doubleJumpPickupSound;

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
        else if (col.gameObject.tag == "DoubleJump")
        {
            AudioSource.PlayClipAtPoint(doubleJumpPickupSound, transform.position);
            playerController.DoubleJumpEnabler();
            
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "SpeedUp") 
        {
            Destroy(col.gameObject);
        }

    }
}
