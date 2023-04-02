using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleLevelProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _levelTMP;

    private int _maxProgress;
    private int _currentProgress = 0;

    private void Start()
    {
        BattleLevelData data = DataHandler.Instance.CurrentBattleLevelData;
        _maxProgress = data.TotalEnemies;
        _levelTMP.text = $"Level {data.Level}";
        UpdateSlider();
    }

    private void OnEnable()
    {
        GlobalEvents.OnEnemyDead.AddListener(AddProgress);
    }

    private void OnDisable()
    {
        GlobalEvents.OnEnemyDead.RemoveListener(AddProgress);
    }

    private void AddProgress()
    {
        _currentProgress++;
        Debug.Log($"new progress total = {_maxProgress}, dead emeies = {_currentProgress}");
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        _slider.value = (float)_currentProgress / (float)_maxProgress;
    }
}
