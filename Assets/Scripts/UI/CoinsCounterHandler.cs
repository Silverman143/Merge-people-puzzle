using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsCounterHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmp;

    private void Start()
    {
        _tmp.text = "0";
    }

    private void OnEnable()
    {
        GlobalEvents.OnChangeCoinsAmount.AddListener(UpdateData);
    }

    private void OnDisable()
    {
        GlobalEvents.OnChangeCoinsAmount.RemoveListener(UpdateData);
    }

    private void UpdateData()
    {
        _tmp.text = DataHandler.Instance.Coins.ToString();
    }
}
