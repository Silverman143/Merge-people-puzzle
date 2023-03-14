using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelData
{
    public int LevelNum;
    public int ProgressMax;
}

public struct HeroCounter
{
    public HeroType type;
    public int amount;
}

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance;
    [SerializeField] private LevelData[] _levelData;
    [SerializeField] private HeroCounter[] _heroesCounters;
    [SerializeField] private int _heroesTotalAmount;
    public int HeroesTotal => _heroesTotalAmount;
    [SerializeField] private int _heroCollected = 0;
    public int Zombies => _heroCollected;

    [SerializeField] private int _coinsCollected = 0;
    public int Coins => _coinsCollected;

    [SerializeField] private int _currentProgress = 0;
    public int LevelProgress => _currentProgress;

    private void Awake()
    {
        if(Instance != this)
        {
            Instance = this;
        }
        CreateHeroCounters();

    }

    private void CreateHeroCounters()
    {
        int herosAmount = Enum.GetValues(typeof(HeroType)).Length;
        _heroesCounters = new HeroCounter[herosAmount];

        for(int i = 0; i< herosAmount; i++)
        {
            int amount = 0;
            HeroCounter newCounter = new HeroCounter { amount = amount, type = (HeroType)i };
            _heroesTotalAmount += amount;
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

        _heroesTotalAmount += amount;

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

        _heroesTotalAmount -= amount;

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

    public void AddProgress(int amount)
    {
        _currentProgress += amount;
    }
}
