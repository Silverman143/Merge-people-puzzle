using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridItem
{
    public GameObject Obj;
    public int[] coordinates;
    public bool isEmpty;
}

public class GridSystem : MonoBehaviour
{
    [SerializeField] private Transform _startPos;
    [SerializeField] private int _width;
    [SerializeField] private int _hieght;
    [SerializeField] private float distance;

    public static GridSystem Instance;

    [SerializeField] private GameObject _itemPref;
    public GridItem[][] _items { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void GenerateGrid(int hieght = 0, int width = 0)
    {
        if(hieght*width != 0)
        {
            _hieght = hieght;
            _width = width;
        }

        if (_items != null)
        {
            CleanGrid();
        }
        else
        {
            _items = new GridItem[_width][];

            for(int i = 0; i<_items.Length; i++)
            {
                _items[i] = new GridItem[_hieght];
            }
        }
        

        for (int i = 0; i<_width; i++)
        {
            for (int q = 0; q < _hieght; q++)
            {
                Vector3 pos = _startPos.position;
                pos.z += q * distance;
                pos.x += i * distance;
                GameObject item = Instantiate(_itemPref, pos, Quaternion.identity, transform);
                GridItem gridItem = new GridItem { coordinates = new int[] { i, q }, Obj = item, isEmpty = true };
                _items[i][q] = gridItem;
            }
        }
    }


    private void CleanGrid()
    {
        foreach(GridItem[] items in _items)
        {
            foreach(GridItem item in items)
            {
                Destroy(item.Obj);
            }
        }

        _items = new GridItem[_width][];

        for (int i = 0; i < _items.Length; i++)
        {
            _items[i] = new GridItem[_hieght];
        }

    }

    public GridItem GetRandomEmpty()
    {
        GridItem selected = null;
        List<GridItem> empty = new List<GridItem>();

        foreach (GridItem[] items in _items)
        {
            foreach (GridItem item in items)
            {
                if (item.isEmpty)
                {
                    empty.Add(item);
                }
            }
        }

        if (empty.Count == 0)
        {
            return null;
        }

        int index = new System.Random().Next(empty.Count);
        selected = empty[index];
        selected.isEmpty = false;

        return selected;

    }

    public GridItem GetRandomStep(GridItem itemStep)
    {
        GridItem selected = null;
        List<GridItem> empty = new List<GridItem>();

        foreach (GridItem[] items in _items)
        {
            foreach (GridItem item in items)
            {
                if ((item.coordinates[0] == itemStep.coordinates[0] || item.coordinates[1] == itemStep.coordinates[1])
                    && item.isEmpty)
                {
                    empty.Add(item);
                }
            }
        }

        if (empty.Count == 0)
        {
            return null;
        }

        int index = new System.Random().Next(empty.Count);
        selected = empty[index];
        selected.isEmpty = false;

        return selected;
    }

    public bool GridPathClear(GridItem item0, GridItem item1)
    {
        GridItem[] array = new GridItem[] { item0, item1 };

        int diffX = Math.Abs(item1.coordinates[0] - item0.coordinates[0]);
        int diffY = Math.Abs(item1.coordinates[1] - item0.coordinates[1]);

        int numCoordinates = diffX > diffY ? 0 : 1;

        if (array[0].coordinates[numCoordinates] > array[1].coordinates[numCoordinates])
        {
            Array.Reverse(array);
        }

        int start = array[0].coordinates[numCoordinates];
        int end = array[1].coordinates[numCoordinates];
        int length = end - start;
        //Debug.Log($"Length of path = {length}");

        bool isEmpty = true;

        //Debug.Log($"coord0 = {item0.coordinates[0]} | {item0.coordinates[1]}, \n coord2 = {item1.coordinates[0]} | {item1.coordinates[1]},");

        for (int i = 1; i < length; i++)
        {
            if (numCoordinates == 0)
            {
                
                isEmpty = isEmpty && _items[start + i][array[0].coordinates[1]].isEmpty;
                //Debug.Log($"item {start + i} | {array[0].coordinates[0]} if empty = {_items[array[0].coordinates[0]][start + i].isEmpty}");
                Debug.Log("0");
            }
            else
            {
                isEmpty = isEmpty && _items[array[0].coordinates[0]][start + i].isEmpty;
                Debug.Log("1");
                //Debug.Log($"item {array[0].coordinates[0]} | {start + i} if empty = {_items[array[0].coordinates[0]][start + i].isEmpty}");
            }

            if (!isEmpty)
            {
                break;
            }
        }

        return isEmpty;
    }

    public int GetGridLenght()
    {
        int lenght = 0;
        foreach (GridItem[] items in _items)
        {
            foreach (GridItem item in items)
            {
                lenght++;
            }
        }
        return lenght;
    }

    public GridItem GetByCoordinates(int x, int z)
    {
        return _items[x][z];
    }

    public void ReleaseGridItem(GridItem itemChange)
    {
        foreach (GridItem[] items in _items)
        {
            foreach (GridItem item in items)
            {
               if(itemChange == item)
                {
                    item.isEmpty = true;
                }
            }
        }
    }

}
