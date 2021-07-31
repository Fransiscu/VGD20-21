using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class consumablesPicker : MonoBehaviour
{
    int coinValue;
    
    coinController coin;
    playerController playerController;
    doubleJumpController doubleJumpController;
    speedModifierController speedModifierController;
    public GUIController GUIController;
    
    public AudioClip doubleJumpPickupSound;
    public AudioClip speedUpPickupSound;
    public AudioClip speedDownPickupSound;
    public AudioClip coinPickupSound;

    private List<GameObject> obtainedItems;

    private void OnTriggerEnter2D(Collider2D col)
    {
        playerController = gameObject.GetComponent<playerController>();
        string consumableTag = col.gameObject.tag;

        switch (consumableTag)
        {
            case "Coin" when !col.GetComponent<coinController>().pickedUp:
                Debug.Log("Picked coin's value = " + col.GetComponent<coinController>().coinValue);

                AudioSource.PlayClipAtPoint(coinPickupSound, transform.position);

                coin = col.GetComponent<coinController>();
                coinValue = coin.coinValue;
                coin.pickedUp = true;

                GUIController.changeGUIScore(coinValue);
                playerController.EditScore(coinValue);

                SaveSceneSystem.SaveScene(coin.iD); // saving id of aquired item

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

                SaveSceneSystem.SaveScene(coin.iD); // saving id of aquired item

                Destroy(col.gameObject);
                break;

            case "DoubleJump" when !col.GetComponent<doubleJumpController>().pickedUp:
                AudioSource.PlayClipAtPoint(doubleJumpPickupSound, transform.position);

                playerController.DoubleJumpEnabler();

                doubleJumpController = col.GetComponent<doubleJumpController>();
                doubleJumpController.pickedUp = true;

                SaveSceneSystem.SaveScene(doubleJumpController.iD); // saving id of aquired item    

                Destroy(col.gameObject);
                break;

            case "SpeedUp" when !col.GetComponent<speedModifierController>().pickedUp:
                AudioSource.PlayClipAtPoint(speedUpPickupSound, transform.position);

                playerController.SpeedEditEnabler(true);  // speed up -> true

                speedModifierController = col.GetComponent<speedModifierController>();
                speedModifierController.pickedUp = true;

                SaveSceneSystem.SaveScene(speedModifierController.iD);  // saving id of aquired item

                Destroy(col.gameObject);
                break;

            case "SpeedDown" when !col.GetComponent<speedModifierController>().pickedUp:
                AudioSource.PlayClipAtPoint(speedDownPickupSound, transform.position);

                playerController.SpeedEditEnabler(false);  // speed false -> false

                speedModifierController = col.GetComponent<speedModifierController>();
                speedModifierController.pickedUp = true;

                SaveSceneSystem.SaveScene(speedModifierController.iD);  // saving id of aquired item

                Destroy(col.gameObject);
                break;

        }

    }
}
