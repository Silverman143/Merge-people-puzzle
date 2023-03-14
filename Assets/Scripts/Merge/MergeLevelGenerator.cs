using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class MergeLevelGenerator : MonoBehaviour
{
    [SerializeField] private int _gridHeight;
    [SerializeField] private int _gridWidth;

    private int _herosAmount;

    [SerializeField] private GameObject _heroPrefab;

    private void Start()
    {
        _herosAmount = Enum.GetValues(typeof(HeroType)).Length;

        CreateRandomLevel();
    }

    private void OnEnable()
    {
        GlobalEvents.OnFillInButton.AddListener(FillInBlanks);
    }

    private void OnDisable()
    {
        GlobalEvents.OnFillInButton.RemoveListener(FillInBlanks);
    }

    public void CreateRandomLevel()
    {
        GridSystem.Instance.GenerateGrid(_gridHeight, _gridWidth);
        SetHeros();
    }

    private void SetHeros()
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

