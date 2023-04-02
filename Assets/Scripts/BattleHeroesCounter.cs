using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleHeroesCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _TMP;
    private int _heroesAmount;

    private void Start()
    {
        _heroesAmount = DataHandler.Instance.HeroesTotal;
        _TMP.text = $"x{_heroesAmount}";
    }

    private void OnEnable()
    {
        GlobalEvents.OnChangedHeroesAmount.AddListener(UpdateData);
    }

    private void OnDisable()
    {
        GlobalEvents.OnChangedHeroesAmount.RemoveListener(UpdateData);
    }

    private void UpdateData(int amount, HeroType type)
    {
        _heroesAmount = DataHandler.Instance.HeroesTotal;
        _TMP.text = $"x{_heroesAmount}";
    }
}
