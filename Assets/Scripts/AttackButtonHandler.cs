using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AttackButtonHandler : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _button;
    private bool _isActive = false;
    private BattleLevelData _currentLevelData;

    private void Start()
    {
        _currentLevelData = DataHandler.Instance.CurrentBattleLevelData;
        UpdateData();
    }

    private void OnEnable()
    {
        GlobalEvents.OnChangedHeroesAmount.AddListener(UpdateData);
    }
    private void OnDisable()
    {
        GlobalEvents.OnChangedHeroesAmount.RemoveListener(UpdateData);
    }

    private void UpdateData(int amount = 0, HeroType type = HeroType.a)
    {
        float needHealth = _currentLevelData.TotalHealth;
        float curentHeroesHealth = DataHandler.Instance.GetTotalHeroesHealth();

        float value = curentHeroesHealth / needHealth;
        _slider.value = value;

        Debug.Log($"Update attack button value = {value}, need = {needHealth}, current = {curentHeroesHealth}");

        if (_slider.value == 1)
        {
            _isActive = true;
            _button.interactable = _isActive;
        }
        else
        {
            _isActive = false;
            _button.interactable = _isActive;
        }
    }

    public void Button()
    {
        SceneManager.LoadScene(1);
    }
}
