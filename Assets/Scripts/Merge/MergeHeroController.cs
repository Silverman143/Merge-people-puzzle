using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct HeroPrefab
{
    public GameObject Obj;
    public HeroType Type;
}

public class MergeHeroController : MonoBehaviour
{
    public HeroType Type;
    [SerializeField] private HeroPrefab[] _herosPrefabs;

    public GridItem _gridItem;
    [SerializeField] private Outline _outline;
    public bool _isActive = true;

    private Vector3 _moveTarget;

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
        foreach (HeroPrefab hero in _herosPrefabs)
        {
            if (hero.Type == type)
            {
                hero.Obj.SetActive(true);
            }
        }

        _gridItem = gridItem;
        Type = type;
        _outline = GetComponentInChildren<Outline>();
    }

    private void OnMouseDown()
    {
        if (_isActive)
        {
            GlobalEvents.HeroSelectedInvoke(this);
        }

    }

    public void MoveToTarget(Vector3 pos)
    {
        GridSystem.Instance.ReleaseGridItem(_gridItem);
        _moveTarget = pos;
        StartCoroutine(MoveCoroutine());
    }

    public void RejectMove()
    {
        StartCoroutine(RejectAnimation());
    }

    IEnumerator MoveCoroutine()
    {
        transform.DOMove(_moveTarget, 0.3f);
        yield return new WaitForSeconds(0.3f);
        GlobalEvents.HeroDestroyInvoke(transform.position, Type);
        Destroy(gameObject);
    }

    IEnumerator RejectAnimation()
    {
        _outline.enabled = true;

        _outline.OutlineColor = Color.red;

        transform.DOShakePosition(0.2f, 0.4f, 5, 45, false, false);

        yield return new WaitForSeconds(0.2f);

        _outline.enabled = false;
        _outline.OutlineColor = Color.green;

    }
}
