using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HeroPrefab
{
    public GameObject Obj;
    public HeroType Type;
}

public class HeroController : MonoBehaviour
{
    public HeroType Type;
    [SerializeField] private HeroPrefab[] _herosPrefabs;

    public GridItem _gridItem;
    [SerializeField] private Outline _outline;

    private void Awake()
    {
        _outline = GetComponentInChildren<Outline>();
    }

    public void OutLine(bool value)
    {
        _outline.enabled = value;
    }

    public void Activate(HeroType type, GridItem gridItem)
    {
        foreach(HeroPrefab hero in _herosPrefabs)
        {
            if(hero.Type == type)
            {
                hero.Obj.SetActive(true);
            }
        }

        _gridItem = gridItem;

        _outline = GetComponentInChildren<Outline>();
    }

    private void OnMouseDown()
    {
        OutLine(true);
    }
}
