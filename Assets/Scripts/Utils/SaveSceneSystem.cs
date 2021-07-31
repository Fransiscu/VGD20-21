using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Threading;

public class SaveSceneSystem
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
            Debug.LogWarning("apparently");
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

    public static void LoadSceneFromObject(ItemsIDs ids)
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            Debug.Log(g.name);
        }
        // cycle through ids and disable objects
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
