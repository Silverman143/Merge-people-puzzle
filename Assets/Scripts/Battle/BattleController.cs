using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BattleController : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _heroPrefab;

    private BattleHeroController _activeHero;
    private float _yPos;
    private float _zPos;

    public bool Test = false;


    private void Start()
    {
        _yPos = _startPoint.position.y;
        _zPos = _startPoint.position.z;

        CreateHero();
    }


    private void OnEnable()
    {
        GlobalEvents.OnInputPositionChanged.AddListener(MovePoint);
        GlobalEvents.OnInputPositionEnded.AddListener(ActivateHero);
    }

    private void OnDisable()
    {
        GlobalEvents.OnInputPositionChanged.RemoveListener(MovePoint);
        GlobalEvents.OnInputPositionEnded.RemoveListener(ActivateHero);
    }

    private void CreateHero()
    {
        //if(!Test && DataHandler.Instance.HasHeroes(out HeroType type))
        //{
        //    GameObject newHero = Instantiate(_heroPrefab, _startPoint.position, Quaternion.identity, _startPoint);
        //    BattleHeroController heroController = newHero.GetComponent<BattleHeroController>();
        //    _activeHero = heroController;
        //    heroController.Activate(type);
        //}
        if (Test)
        {
            GameObject newHero = Instantiate(_heroPrefab, _startPoint.position, Quaternion.identity, _startPoint);
            BattleHeroController heroController = newHero.GetComponent<BattleHeroController>();
            _activeHero = heroController;
            heroController.Activate(HeroType.a);
        }
        else
        {
            Debug.Log("Heroes ended");
        }
    }

    private void ActivateHero()
    {
        _activeHero.Attack();
        StartCoroutine(CreateHeroCoroutine());
    }

    private void MovePoint(Vector3 pos)
    {
        Vector3 newPos = new Vector3(pos.x, _yPos, _zPos);

        _startPoint.DOMove(newPos, 0.3f);
    }

    IEnumerator CreateHeroCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        CreateHero();
    }
}
