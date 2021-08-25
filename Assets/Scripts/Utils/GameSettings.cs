using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameSettings
{
    public bool sound;
    public bool music;

    private static readonly string settingsData = PlayerPrefsKey.gameSettingsPrefName;

    public GameSettings() { }
    public GameSettings(bool sound, bool music)
    {
        this.sound = sound;
        this.music = music;
    }

    public bool Sound { get => sound; set => sound = value; }
    public bool Music { get => music; set => music = value; }

    public void SaveSettings()
    {
        PlayerPrefs.SetString(settingsData, JsonUtility.ToJson(this));
    }

    public static GameSettings LoadSettings()
    {
        return JsonUtility.FromJson<GameSettings>(PlayerPrefs.GetString(settingsData));
    }
}
