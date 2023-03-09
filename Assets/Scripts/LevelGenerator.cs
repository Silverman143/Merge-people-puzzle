using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
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

    public void CreateRandomLevel()
    {
        GridSystem.Instance.GenerateGrid(_gridHeight, _gridWidth);
        SetHeros();
    }

    private void SetHeros()
    {
        List<GridItem> gridItems = GridSystem.Instance._items.ToList<GridItem>();

        int herosAmount = gridItems.Count / 2;

        for (int i = 0; i < herosAmount; i++)
        {
            HeroType type = (HeroType)UnityEngine.Random.Range(0, _herosAmount);
            List<GridItem> selectedGridItems = new List<GridItem>();

            for(int q = 0; q < 2; q++)
            {
                GridItem item;
                if (selectedGridItems.Count == 0)
                {
                    item = gridItems[UnityEngine.Random.Range(0, gridItems.Count)];

                }
                else
                {
                    int[] coordinates = selectedGridItems[0].coordinates;
                    int coordNum = UnityEngine.Random.Range(0, 1);

                    List<GridItem> checkItems = new List<GridItem>();
                    foreach(GridItem gritdItem in gridItems)
                    {
                        if (gritdItem.coordinates[coordNum] == coordinates[coordNum])
                        {
                            checkItems.Add(gritdItem);
                        }
                    }

                    item = checkItems[UnityEngine.Random.Range(0, checkItems.Count)];

                }
                
                selectedGridItems.Add(item);
                gridItems.Remove(item);
            }

            CreateСoupleHero(type, selectedGridItems);
        }
    }

    private void CreateСoupleHero(HeroType type, List<GridItem> gridItems)
    {
        foreach(GridItem item in gridItems)
        {
            GameObject heroObj = Instantiate(_heroPrefab, item.Obj.transform.position, Quaternion.identity, item.Obj.transform);
            HeroController hero = heroObj.GetComponent<HeroController>();

            hero.Activate(type, item);

        }
    }
}
