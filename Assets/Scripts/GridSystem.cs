using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridItem
{
    public GameObject Obj;
    public int[] coordinates;
}

public class GridSystem : MonoBehaviour
{
    [SerializeField] private Transform _startPos;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float distance;

    public static GridSystem Instance;

    [SerializeField] private GameObject _itemPref;
    public GridItem[] _items { get; private set; }

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
            _height = hieght;
            _width = width;
        }

        if (_items != null)
        {
            CleanGrid();
        }
        else
        {
            _items = new GridItem[_height * _width];
        }
        

        for (int i = 0; i<_width; i++)
        {
            for (int q = 0; q < _height; q++)
            {
                Vector3 pos = _startPos.position;
                pos.z += q * distance;
                pos.x += i * distance;
                GameObject item = Instantiate(_itemPref, pos, Quaternion.identity, transform);
                GridItem gridItem = new GridItem { coordinates = new int[] {i, q }, Obj = item };
                _items[i*_height + q] = gridItem;
            }
        }
    }


    private void CleanGrid()
    {
        foreach(GridItem item in _items)
        {
            DestroyImmediate(item.Obj);
        }
        _items = new GridItem[_height*_width];
    }


}
