using UnityEngine;

// Script to handle the pick up of consumables
public class ConsumablesPicker : MonoBehaviour
{
    CoinController coin;
    PlayerController playerController;
    DoubleJumpController doubleJumpController;
    SpeedModifierController speedModifierController;
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

        playerController = gameObject.GetComponent<PlayerController>();
        string consumableTag = col.gameObject.tag;
        Destroy(col.gameObject);

        // handling the different kind of pickups
        switch (consumableTag)
        {
            case "Coin" when !col.GetComponent<CoinController>().pickedUp:
                Debug.Log("Picked coin's value = " + col.GetComponent<CoinController>().coinValue);

                AudioSource.PlayClipAtPoint(coinPickupSound, transform.position, volume);

                coin = col.GetComponent<CoinController>();
                coin.pickedUp = true;

                // if player already at checkpoint we don't save the score.
                // In case of death the checkpoint saved score will be restored.
                if (playerController.player.AtCheckpoint)
                {
                    playerController.IncreaseScore(coin.coinValue, false);
                }
                // else we save it
                else
                {
                    playerController.IncreaseScore(coin.coinValue, true);
                }

                // if the player is in the first half of the level, save the pickup progression
                if (!playerController.player.AtCheckpoint)
                {
                    SaveSceneSystem.SaveScene(coin.iD); // saving id of acquired item
                }

                break;

            case "BiggerCoin" when !col.GetComponent<CoinController>().pickedUp:
                Debug.Log("Picked bigger coin! value = " + col.GetComponent<CoinController>().coinValue * SETTINGS.biggerCoinMultiplier);

                AudioSource.PlayClipAtPoint(coinPickupSound, transform.position, volume);

                coin = col.GetComponent<CoinController>();
                coin.pickedUp = true;

                // if player already at checkpoint we don't save the score.
                // In case of death the checkpoint saved score will be restored.
                if (playerController.player.AtCheckpoint)
                {
                    playerController.IncreaseScore(coin.coinValue * SETTINGS.biggerCoinMultiplier, false);
                }
                // else we save it
                else
                {
                    playerController.IncreaseScore(coin.coinValue * SETTINGS.biggerCoinMultiplier, true);
                }

                // if the player is in the first half of the level, save the pickup progression
                if (!playerController.player.AtCheckpoint)
                {
                    SaveSceneSystem.SaveScene(coin.iD); // saving id of acquired item
                }

                break;

            case "DoubleJump" when !col.GetComponent<DoubleJumpController>().pickedUp:
                AudioSource.PlayClipAtPoint(doubleJumpPickupSound, transform.position, volume);

                playerController.DoubleJumpEnabler();

                doubleJumpController = col.GetComponent<DoubleJumpController>();
                doubleJumpController.pickedUp = true;

                // if the player is in the first half of the level, save the pickup progression
                if (!playerController.player.AtCheckpoint)
                {
                    SaveSceneSystem.SaveScene(doubleJumpController.iD); // saving id of acquired item    
                }

                break;

            case "SpeedUp" when !col.GetComponent<SpeedModifierController>().pickedUp:
                AudioSource.PlayClipAtPoint(speedUpPickupSound, transform.position, volume);

                playerController.SpeedEditEnabler(true);  // speed up -> true

                speedModifierController = col.GetComponent<SpeedModifierController>();
                speedModifierController.pickedUp = true;

                // if the player is in the first half of the level and didn't reach the checkpoint, save the pickup progression
                if (!playerController.player.AtCheckpoint)
                {
                    SaveSceneSystem.SaveScene(speedModifierController.iD);  // saving id of acquired item
                }

                break;

            case "SpeedDown" when !col.GetComponent<SpeedModifierController>().pickedUp:
                AudioSource.PlayClipAtPoint(speedDownPickupSound, transform.position, volume);

                playerController.SpeedEditEnabler(false);  // speed down -> false

                speedModifierController = col.GetComponent<SpeedModifierController>();
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
