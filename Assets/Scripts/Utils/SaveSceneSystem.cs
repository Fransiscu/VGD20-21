using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        Debug.LogWarning("boh");
        string itemsIDsString;
        List<string> itemsIDs = new List<string>();
        
            Debug.LogWarning(PlayerPrefs.HasKey(idPrefName));
        if (PlayerPrefs.HasKey(idPrefName))
        {
            Debug.LogWarning("apparently");
            LoadSceneFromJson(PlayerPrefs.GetString(idPrefName));
        }

        itemsIDs.Add(id);
        itemsIDs.Add(id);
        itemsIDs.Add(id);

        itemsIDsString  = JsonConvert.SerializeObject(itemsIDs);

        PlayerPrefs.SetString(idPrefName, itemsIDsString);
    }

    Movie m = JsonConvert.DeserializeObject<Movie>(json);
    public static void LoadSceneFromJson(string jsonList)
    {
        string test = JsonToken.Pa
        Debug.LogWarning(test);
    }

    public static void DeleteSceneSave()
    {
        PlayerPrefs.DeleteKey(idPrefName);
    }
}
