using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct HeroesOnGridData
{
    public HeroType Type;
    public int[] coordinates;
}

public static class DataBaseHandler 
{
    private static string _mergeLevelS = "MergeLevel";
    private static string _mergeLevelProgressS = "MergeProgress";

    private static string _battleLevelS = "BattleLevel";

    private static string _jsonFileHeroesOnGrid = Application.persistentDataPath +"/gridData.json";
    private static string _jsonFileHeroesCounter = Application.persistentDataPath + "/counterData.json";

    public static void UpgradeMergeLevel()
    {
        int level =  PlayerPrefs.GetInt(_mergeLevelS);
        level++;
        PlayerPrefs.SetInt(_mergeLevelS, level);
    }

    public static int GetMergeLevel()
    {
        if (!PlayerPrefs.HasKey(_mergeLevelS))
        {
            PlayerPrefs.SetInt(_mergeLevelS, 0);
            return 0;
        }

        return PlayerPrefs.GetInt(_mergeLevelS);
    }

    public static void SaveMergeLevelProgress(int value)
    {
        PlayerPrefs.SetInt(_mergeLevelProgressS, value);
    }

    public static int GetMergeLevelProgress()
    {
        if (!PlayerPrefs.HasKey(_mergeLevelProgressS))
        {
            PlayerPrefs.SetInt(_mergeLevelProgressS, 0);
            return 0;
        }
        return PlayerPrefs.GetInt(_mergeLevelProgressS);
    }

    public static void UpgradeBattleLevel()
    {
        int level = PlayerPrefs.GetInt(_battleLevelS);
        level++;
        PlayerPrefs.SetInt(_battleLevelS, level);
    }

    public static int GetBattleLevel()
    {
        if (!PlayerPrefs.HasKey(_battleLevelS))
        {
            PlayerPrefs.SetInt(_battleLevelS, 0);
            return 0;
        }
        return PlayerPrefs.GetInt(_battleLevelS);
    }

    public static void SaveHeroesOnGrid(HeroesOnGridData[] heroesData)
    {
        if (!File.Exists(_jsonFileHeroesOnGrid))
        {
            File.Create(_jsonFileHeroesOnGrid).Dispose();
        }

        string json = JsonUtilityHelper.SerializeHeroesOnGridDataArray(heroesData);
        

        File.WriteAllText(_jsonFileHeroesOnGrid, json);
    }

    public static HeroesOnGridData[] GetSavedHeroesOnGridData()
    {
        if (!File.Exists(_jsonFileHeroesOnGrid))
        {
            return null;
        }

        string json = File.ReadAllText(_jsonFileHeroesOnGrid);

        HeroesOnGridData[] data = JsonUtilityHelper.DeserializeHeroesOnGridDataArray(json);
        return data;
    }

    public static void SaveHeroesCounter(HeroCounter[] counter)
    {
        if (!File.Exists(_jsonFileHeroesCounter))
        {
            File.Create(_jsonFileHeroesCounter).Dispose();
        }

        string json = JsonUtilityHelper.SerializerHeroesCounterArray(counter);
        

        File.WriteAllText(_jsonFileHeroesCounter, json);
    }

    public static HeroCounter[] GetHeroesCounters()
    {
        if (!File.Exists(_jsonFileHeroesCounter))
        {
            return null;
        }

        string json = File.ReadAllText(_jsonFileHeroesCounter);
        HeroCounter[] counters = JsonUtilityHelper.DeserializeHeroesCouner(json);
        return counters;
    }
}
