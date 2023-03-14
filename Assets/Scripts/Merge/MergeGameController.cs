using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType
{
    a, b, c
}



public class MergeGameController : MonoBehaviour
{
    private MergeHeroController _hero0;
    private MergeHeroController _hero1;

    [SerializeField] private int _mergeRevenue = 5;

    private int _destroyCounter = 0;

    private void Awake()
    {
        _hero0 = null;
    }

    private void OnEnable()
    {
        GlobalEvents.OnHeroSelected.AddListener(SelectHero);
        GlobalEvents.OnHeroDestroy.AddListener(HeroDestroed);
    }

    private void OnDisable()
    {
        GlobalEvents.OnHeroSelected.RemoveListener(SelectHero);
        GlobalEvents.OnHeroDestroy.RemoveListener(HeroDestroed);
    }

    private void SelectHero(MergeHeroController hero)
    {
        if (_hero0 == null)
        {
            _hero0 = hero;
            _hero0.OutLine(true);
        }
        else if (_hero0 != hero)
        {
            _hero1 = hero;

            int[] coord0 = _hero0._gridItem.coordinates;
            int[] coord1 = _hero1._gridItem.coordinates;

            if (_hero0.Type == _hero1.Type && (coord0[0] == coord1[0] || coord0[1] == coord1[1]))
            {
                if (GridSystem.Instance.GridPathClear(_hero0._gridItem, _hero1._gridItem))
                {
                    _hero1.OutLine(true);
                    Vector3 targetPos = Vector3.Lerp(_hero0.transform.position, _hero1.transform.position, 0.5f);
                    _hero0.MoveToTarget(targetPos);
                    _hero1.MoveToTarget(targetPos);
                }
                else
                {
                    Debug.Log("GridBuissy");

                    _hero0.RejectMove();
                    _hero1.RejectMove();

                }
            }
            else
            {
                Debug.Log("Different types of herose");

                _hero0.RejectMove();
                _hero1.RejectMove();
            }

            _hero0 = null;
            _hero1 = null;
        }
    }

    public void HeroDestroed(Vector3 hitPos, HeroType heroType)
    {
        _destroyCounter++;
        if (_destroyCounter > 1)
        {
            DataHandler.Instance.AddCoins(_mergeRevenue);
            DataHandler.Instance.AddHero(1, heroType);
            VFXHandler.Instance.DamageEffectActivate(hitPos);
            _destroyCounter = 0;

        }
    }
}