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
    public GUIController GUIController;
    
    public AudioClip doubleJumpPickupSound;
    public AudioClip speedUpPickupSoung;
    public AudioClip coinPickupSound;

    private void OnTriggerEnter2D(Collider2D col)
    {
        playerController = gameObject.GetComponent<playerController>();

        switch (col.gameObject.tag)
        {
            case "Coin" when !col.GetComponent<coinController>().pickedUp:
                Debug.Log("Picked coin's value = " + col.GetComponent<coinController>().coinValue);

                AudioSource.PlayClipAtPoint(coinPickupSound, transform.position);

                coin = col.GetComponent<coinController>();
                coinValue = coin.coinValue;
                coin.pickedUp = true;

                GUIController.changeGUIScore(coinValue);
                playerController.EditScore(coinValue);

                Destroy(col.gameObject);
                break;

            case "BiggerCoin" when !col.GetComponent<doubleJumpController>().pickedUp:
                Debug.Log("Picked bigger coin! value * 3 " + col.GetComponent<coinController>().coinValue * 3);

                AudioSource.PlayClipAtPoint(coinPickupSound, transform.position);

                coin = col.GetComponent<coinController>();
                coinValue = coin.coinValue;
                coin.pickedUp = true;

                GUIController.changeGUIScore(coinValue * 3);
                playerController.EditScore(coinValue * 3);

                Destroy(col.gameObject);
                break;

            case "DoubleJump" when !col.GetComponent<doubleJumpController>().pickedUp:
                AudioSource.PlayClipAtPoint(doubleJumpPickupSound, transform.position);

                playerController.DoubleJumpEnabler();

                doubleJumpController = col.GetComponent<doubleJumpController>();
                doubleJumpController.pickedUp = true;

                Destroy(col.gameObject);
                break;

            case "SpeedUp" when !col.GetComponent<speedUpController>().pickedUp:
                AudioSource.PlayClipAtPoint(speedUpPickupSoung, transform.position);

                playerController.SpeedEditEnabler(true);  // speed up -> true

                speedUpController = col.GetComponent<speedUpController>();
                speedUpController.pickedUp = true;

                Destroy(col.gameObject);
                break;
        }
    }
}
