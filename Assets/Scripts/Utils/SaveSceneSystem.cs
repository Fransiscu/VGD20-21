using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Linq;

public class SaveSceneSystem : MonoBehaviour
{
    public static readonly string idPrefName = "items_ids";

    /*
     *  I'll be saving a list of items ids to a json array, storing it in PlayerPrefs
     *  and using it to restore the status of the game objects in the scene
     *  in such a way to replicate the exact state of the level at the moment of the save
     */

    public static void SaveScene(string id)
    {
        ItemsIDs idsObject = new ItemsIDs();
        idsObject.Ids = new List<string>();
        string jsonIdsString;
        
        /* 
         * If id == 0 we are at the beginning of the level and can just return.
         * This is necessary to avoid a small freeze the first time this method is called
         * during a level (for some unknown reason). We trigger it at the beginning of the scene
         * during the animation so it's not noticeable for the player when picking up the first consumable
         * mid gameplay.
        */

        if (id.Equals("0"))
        {
            jsonIdsString = JsonConvert.SerializeObject(idsObject);
            return;
        }
        
        if (PlayerPrefs.HasKey(idPrefName)) // checking if we already have something saved
        {
            idsObject = LoadSceneDetailsFromJson(PlayerPrefs.GetString(idPrefName));   // if yes, load it
        }

        idsObject.Ids.Add(id);  // add id to the list 
        jsonIdsString = JsonConvert.SerializeObject(idsObject);  // serialize to json

        Debug.LogWarning(jsonIdsString);

        PlayerPrefs.SetString(idPrefName, jsonIdsString); // save to playerprefs
    }

    public static ItemsIDs LoadSceneDetailsFromJson(string jsonIdsString)
    {
        return JsonConvert.DeserializeObject<ItemsIDs>(jsonIdsString);
    }

    public static void LoadSceneFromObject()
    {
        ItemsIDs idsObject = LoadSceneDetailsFromJson(PlayerPrefs.GetString(idPrefName));   // ok
        object[] objectsInScene = GameObject.FindObjectsOfType(typeof(GameObject));
        
        // cycling through the items and destroying the ones matching the list
        foreach (object item in objectsInScene)
        {
            GameObject currentGameObject = (GameObject) item;
            string consumableTag = currentGameObject.tag;

            switch (consumableTag)
            {
                case "Coin":
                case "BiggerCoin":
                    coinController coin = currentGameObject.GetComponent<coinController>();
                    Debug.LogWarning("value = " + coin.coinValue + " - ID = " + coin.iD);
                    
                    if (idsObject.Ids.Contains(coin.iD))
                    {
                        Destroy(coin.gameObject);
                    }
                    break;

                case "DoubleJump":
                    doubleJumpController doubleJump = currentGameObject.GetComponent<doubleJumpController>();

                    if (idsObject.Ids.Contains(doubleJump.iD))
                    {
                        Destroy(doubleJump.gameObject);
                    }
                    break;

                case "SpeedUp":
                case "SpeedDown":
                    speedModifierController speedModifier = currentGameObject.GetComponent<speedModifierController>();
                   
                    if (idsObject.Ids.Contains(speedModifier.iD))
                    {
                        Destroy(speedModifier.gameObject);
                    }
                    break;
            }
        }
    }

    public static void DeleteSceneSave()
    {
        PlayerPrefs.DeleteKey(idPrefName);
    }

    /*  
     *  Support class created to help saving consumables ids
     *  in json format
     */
    public class ItemsIDs
    {
        List<string> ids;

        public ItemsIDs() { }

        public List<string> Ids { get => ids; set => ids = value; }
    }
}
