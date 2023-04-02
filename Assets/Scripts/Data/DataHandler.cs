using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public struct MergeLevelData
{
    public int LevelNum;
    public int ProgressMax;
    public int GridHigh;
    public int GridWidth;
}

[Serializable]
public struct HeroCounter
{
    public HeroType type;
    public int amount;
}

[Serializable]
public struct HeroesData
{
    public HeroType type;
    public float health;
}

[Serializable]
public struct EnemyData
{
    public EnemyType Type;
    public int Amount;
    public float Health;
}

[Serializable]
public struct BattleLevelData
{
    public int Level;
    public EnemyData[] EnemiesDatas;
    public float TotalHealth => CountTotalHealth();
    public int TotalEnemies => CountEnemies();

    private int CountEnemies()
    {
        int enemies = 0;

        foreach (EnemyData enemyData in EnemiesDatas)
        {
            enemies += enemyData.Amount;
        }
        return enemies;
    }

    private float CountTotalHealth()
    {
        float health = 0;
        foreach(EnemyData enemyData in EnemiesDatas)
        {
            float value = enemyData.Amount * enemyData.Health;
            health += value;
        }
        return health;
    }
}

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance;
    [SerializeField] private MergeLevelData[] _mergeLevelData;                                  // Data about all exist Levels / needed Progress
    [SerializeField] private BattleLevelData[] _battleLevelDatas;                               // Data aboo all exist Levels (enemies, health)
    [SerializeField] private HeroesData[] _heroesDatas;                                         // Info about heroes, there type and health
    [SerializeField] private HeroCounter[] _heroesCounters;                                     // Counter of collected heroes for buttle

    [SerializeField] private int _battleCurrentLevel = 0;
    public int BattleCurrentLevel => _battleCurrentLevel;

    [SerializeField] private int _mergeCurrentLevel = 0;
    public int MergeCurrentLevel => _mergeCurrentLevel;

    public BattleLevelData CurrentBattleLevelData => GetCurrentBattleLevelData();

    public int HeroesTotal => GetTotalHeroes();

    [SerializeField] private int _heroCollected = 0;
    public int Zombies => _heroCollected;

    [SerializeField] private int _coinsCollected = 0;
    public int Coins => _coinsCollected;

    [SerializeField] private int _mergerCurrentProgress = 0;
    public int LevelProgress => _mergerCurrentProgress;

    private HeroesOnGridData[] _savedHeroesOnGrid;
    public HeroesOnGridData[] SavedHeroesOnGrid => _savedHeroesOnGrid;

    private void Awake()
    {
        LoadSavedMergeData();
        LoadSavedBattleData();


        if(Instance != this)
        {
            Instance = this;
        }
        if(_heroesCounters == null)
        {
            CreateHeroCounters();
        }
    }

    public void GetCurrentMergeLevelGridSize(out int width, out int higth)
    {
        foreach(MergeLevelData data in _mergeLevelData)
        {
            if (data.LevelNum == _mergeCurrentLevel)
            {
                width = data.GridWidth;
                higth = data.GridHigh;
                return;
            }
        }

        throw new Exception($"Didnt have current leveldata. level = {_mergeCurrentLevel}");
    }

    public bool IsNewMergeLevel()
    {
        if (_mergerCurrentProgress != 0) return false;
        else return true;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("============Quit=============");
        SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveData();
    }
    private void SaveData()
    {
        MergeHeroController[] heroes = FindObjectsOfType<MergeHeroController>();

        HeroesOnGridData[] data = new HeroesOnGridData[heroes.Length];
        for(int i =0; i < data.Length; i++)
        {
            HeroesOnGridData d = new HeroesOnGridData { Type = heroes[i].Type, coordinates = heroes[i]._gridItem.coordinates };
            data[i] = d;
        }

        DataBaseHandler.SaveHeroesCounter(_heroesCounters);
        DataBaseHandler.SaveHeroesOnGrid(data);
        DataBaseHandler.SaveMergeLevelProgress(_mergerCurrentProgress);
    }

    private void LoadSavedMergeData()
    {
        _mergeCurrentLevel = DataBaseHandler.GetMergeLevel();
        _mergerCurrentProgress = DataBaseHandler.GetMergeLevelProgress();
        _savedHeroesOnGrid = DataBaseHandler.GetSavedHeroesOnGridData();
        _heroesCounters = DataBaseHandler.GetHeroesCounters();
    }

    private void LoadSavedBattleData()
    {
        _battleCurrentLevel = DataBaseHandler.GetBattleLevel();
    }

    private void CreateHeroCounters()
    {
        int herosAmount = Enum.GetValues(typeof(HeroType)).Length;
        _heroesCounters = new HeroCounter[herosAmount];

        for(int i = 0; i< herosAmount; i++)
        {
            int amount = 0;
            HeroCounter newCounter = new HeroCounter { amount = amount, type = (HeroType)i};
            _heroesCounters[i] = newCounter;
        }
    }

    public void AddHero(int amount, HeroType type)
    {

        for(int i = 0; i<_heroesCounters.Length; i++)
        {
            if (_heroesCounters[i].type == type)
            {
                _heroesCounters[i].amount += amount;
            }
        }

        GlobalEvents.ChangedHeroesAmountInvoke(_heroCollected, type);
    }

    public void RemoveHero(int amount, HeroType type)
    {

        for (int i = 0; i < _heroesCounters.Length; i++)
        {
            if (_heroesCounters[i].type == type)
            {
                _heroesCounters[i].amount -= amount;
            }
        }

        GlobalEvents.ChangedHeroesAmountInvoke(_heroCollected, type);
    }

    public int GetTotalHeroes()
    {
        int amount = 0;

        foreach(HeroCounter counter in _heroesCounters)
        {
            amount += counter.amount;
        }

        return amount;
    }

    public float GetTotalHeroesHealth()
    {
        float total = 0;

        foreach (HeroCounter counter in _heroesCounters)
        {
            HeroType type = counter.type;
            total += counter.amount * GetHelthOfHeroType(type);
        }

        return total;
    }

    private float GetHelthOfHeroType(HeroType type)
    {
        foreach(HeroesData data in _heroesDatas)
        {
            if (data.type == type) return data.health;
        }
        return 0;
    }

    public bool HasHeroes(out HeroType type)
    {
        //type = HeroType.a;

        //List<HeroCounter> heroes = new List<HeroCounter>();

        //foreach(HeroCounter counter in _heroesCounters)
        //{
        //    if (counter.amount > 0)
        //    {
        //        heroes.Add(counter);
        //    }
        //}

        //if (heroes.Any())
        //{
        //    int typeIndex = UnityEngine.Random.Range(0, heroes.Count);
        //    type = heroes[typeIndex].type;
        //    RemoveHero(1, type);
        //    return true;
        //}


        //return false;

        type = HeroType.a;
        Debug.LogError("DataHandler return test hero type");
        return true;
    }

    public void AddCoins(int amount = 0)
    {
        _coinsCollected += amount;
        GlobalEvents.ChangeCoinsAmountInvoke();
    }

    public void AddMergeLevelProgress(int amount)
    {
        _mergerCurrentProgress += amount;
    }

    private BattleLevelData GetCurrentBattleLevelData()
    {
        Debug.Log($"Current battleLevel = {_battleCurrentLevel}, enemies = {_battleLevelDatas[_battleCurrentLevel].EnemiesDatas.Length}");
        return _battleLevelDatas[_battleCurrentLevel];
    }
}
