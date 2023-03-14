using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZombiesCounterHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterTMP;

    private void Start()
    {
        _counterTMP.text = "x0";
    }

    private void OnEnable()
    {
        GlobalEvents.OnChangedHeroesAmount.AddListener(UpdateCounter);
    }

    public void OnDisable()
    {
        GlobalEvents.OnChangedHeroesAmount.RemoveListener(UpdateCounter);
    }

    private void UpdateCounter(int amount, HeroType type)
    {
        _counterTMP.text = $"x{DataHandler.Instance.HeroesTotal}";
    }
     
}
