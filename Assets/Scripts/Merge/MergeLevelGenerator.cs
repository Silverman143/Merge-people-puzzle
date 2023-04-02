using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
struct GridLevel
{
    public int LevelNumber;
    public Vector3 Pos;
    public int xSize;
    public int zSize;
}

public class MergeLevelGenerator : MonoBehaviour
{
    [SerializeField] private int _gridHeight;
    [SerializeField] private int _gridWidth;
    [SerializeField] private GridLevel[] _gridLevels;

    private int _herosAmount;

    [SerializeField] private GameObject _heroPrefab;

    public static MergeLevelGenerator Instance;

    private void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _herosAmount = Enum.GetValues(typeof(HeroType)).Length;

        //CreateRandomLevel();
    }

    private void OnEnable()
    {
        GlobalEvents.OnFillInButton.AddListener(FillInBlanks);
    }

    private void OnDisable()
    {
        GlobalEvents.OnFillInButton.RemoveListener(FillInBlanks);
    }

    public void CreateRandomLevel(int heigh, int width)
    {
        GridSystem.Instance.GenerateGrid(heigh, width);
        SetHeros();
    }

    public void LoadLevel()
    {
        int heigh;
        int width;
        DataHandler.Instance.GetCurrentMergeLevelGridSize(out width, out heigh);

        if (DataHandler.Instance.IsNewMergeLevel())
        {
            CreateRandomLevel(heigh, width);
        }
        else
        {
            HeroesOnGridData[] heroes = DataHandler.Instance.SavedHeroesOnGrid;

            GridSystem.Instance.GenerateGrid(heigh, width);
            AppendHeroes(heroes);
        }
    }

    private void AppendHeroes(HeroesOnGridData[] heroes)                                                    // Add saved heroes
    {
        foreach(HeroesOnGridData hero in heroes)
        {
            GridItem gridItem = GridSystem.Instance.GetByCoordinates(hero.coordinates[0], hero.coordinates[1]);
            CreateSingleHero(hero.Type, gridItem);
        }
    }

    private void SetHeros()                                                                                 //Generate new random heroes
    {
        int herosAmount = GridSystem.Instance.GetGridLenght() / 2;
        Debug.Log(herosAmount);

        for (int i = 0; i < herosAmount; i++)
        {
            HeroType type = (HeroType)UnityEngine.Random.Range(0, _herosAmount);
            List<GridItem> gridItems = new List<GridItem>();

            gridItems.Add(GridSystem.Instance.GetRandomEmpty());
            gridItems.Add(GridSystem.Instance.GetRandomStep(gridItems[0]));

            if (gridItems[1] != null)
            {
                CreateСoupleHero(type, gridItems);
            }
            else
            {
                Debug.LogError("Cant create couple heroes");

                CreateSingleHero(type, gridItems[0]);
            }
        }
    }

    private void FillInBlanks()
    {
        StartCoroutine(FillInBlanksCoroutine());
    }

    private void CreateСoupleHero(HeroType type, List<GridItem> gridItems)
    {
        foreach (GridItem item in gridItems)
        {
            GameObject heroObj = Instantiate(_heroPrefab, item.Obj.transform.position, Quaternion.identity, item.Obj.transform);
            MergeHeroController hero = heroObj.GetComponent<MergeHeroController>();
            hero.Activate(type, item);

        }
    }

    public void CreateSingleHero(HeroType type, GridItem gridItem)
    {
        GameObject heroObj = Instantiate(_heroPrefab, gridItem.Obj.transform.position, Quaternion.identity, gridItem.Obj.transform);
        MergeHeroController hero = heroObj.GetComponent<MergeHeroController>();
        hero.Activate(type, gridItem);
    }

    IEnumerator FillInBlanksCoroutine()
    {
        GridItem gridItem = GridSystem.Instance.GetRandomEmpty();
        while (gridItem != null)
        {
            HeroType type = (HeroType)UnityEngine.Random.Range(0, _herosAmount);
            CreateSingleHero(type, gridItem);

            yield return new WaitForSeconds(0.1f);
            gridItem = GridSystem.Instance.GetRandomEmpty();
        }
    }
}

