using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class consumablesPicker : MonoBehaviour
{
    coinController coin;
    playerController playerController;
    doubleJumpController doubleJumpController;
    speedModifierController speedModifierController;
    public GUIController GUIController;

    private GameSettings gameSettings;
    
    public AudioClip doubleJumpPickupSound;
    public AudioClip speedUpPickupSound;
    public AudioClip speedDownPickupSound;
    public AudioClip coinPickupSound;

    float volume;

    private void OnTriggerEnter2D(Collider2D col)
    {
        gameSettings = new GameSettings();
        gameSettings = GameSettings.LoadSettings();
        volume = gameSettings.Sound ? SETTINGS.soundVolume : 0f;

        playerController = gameObject.GetComponent<playerController>();
        string consumableTag = col.gameObject.tag;
        Destroy(col.gameObject);

        switch (consumableTag)
        {
            case "Coin" when !col.GetComponent<coinController>().pickedUp:
                Debug.Log("Picked coin's value = " + col.GetComponent<coinController>().coinValue);

                AudioSource.PlayClipAtPoint(coinPickupSound, transform.position, volume);

                coin = col.GetComponent<coinController>();
                coin.pickedUp = true;

                GUIController.changeGUIScore(coin.coinValue);
                playerController.EditScore(coin.coinValue);

                // if the player is in the first half of the level, save the pickup progression
                if (!playerController.player.AtCheckpoint)
                {
                    SaveSceneSystem.SaveScene(coin.iD); // saving id of acquired item
                }

                break;

            case "BiggerCoin" when !col.GetComponent<coinController>().pickedUp:
                Debug.Log("Picked bigger coin! value * 3 " + col.GetComponent<coinController>().coinValue * 3);

                AudioSource.PlayClipAtPoint(coinPickupSound, transform.position, volume);

                coin = col.GetComponent<coinController>();
                coin.pickedUp = true;

                GUIController.changeGUIScore(coin.coinValue * 3);
                playerController.EditScore(coin.coinValue * 3);

                // if the player is in the first half of the level, save the pickup progression
                if (!playerController.player.AtCheckpoint)
                {
                    SaveSceneSystem.SaveScene(coin.iD); // saving id of acquired item
                }

                break;

            case "DoubleJump" when !col.GetComponent<doubleJumpController>().pickedUp:
                AudioSource.PlayClipAtPoint(doubleJumpPickupSound, transform.position, volume);

                playerController.DoubleJumpEnabler();

                doubleJumpController = col.GetComponent<doubleJumpController>();
                doubleJumpController.pickedUp = true;

                // if the player is in the first half of the level, save the pickup progression
                if (!playerController.player.AtCheckpoint)
                {
                    SaveSceneSystem.SaveScene(doubleJumpController.iD); // saving id of acquired item    
                }

                break;

            case "SpeedUp" when !col.GetComponent<speedModifierController>().pickedUp:
                AudioSource.PlayClipAtPoint(speedUpPickupSound, transform.position, volume);

                playerController.SpeedEditEnabler(true);  // speed up -> true

                speedModifierController = col.GetComponent<speedModifierController>();
                speedModifierController.pickedUp = true;

                // if the player is in the first half of the level and didn't reach the checkpoint, save the pickup progression
                if (!playerController.player.AtCheckpoint)
                {
                    SaveSceneSystem.SaveScene(speedModifierController.iD);  // saving id of acquired item
                }

                break;

            case "SpeedDown" when !col.GetComponent<speedModifierController>().pickedUp:
                AudioSource.PlayClipAtPoint(speedDownPickupSound, transform.position, volume);

                playerController.SpeedEditEnabler(false);  // speed down -> false

                speedModifierController = col.GetComponent<speedModifierController>();
                speedModifierController.pickedUp = true;

                // if the player is in the first half of the level, save the pickup progression
                if (!playerController.player.AtCheckpoint)
                {
                    SaveSceneSystem.SaveScene(speedModifierController.iD);  // saving id of acquired item
                }

                break;  
        }

    }
}
