using System;
using Game.Game.Scripts.UI.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetPopupView : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TextMeshProUGUI _upgradeCostText;
    [SerializeField] private GameObject _upgradeCostIcon;
    [SerializeField] private TextMeshProUGUI _populationText;
    [SerializeField] private TextMeshProUGUI _incomeText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private Image _icon;
    private PlanetPopupPresenter _planetPopupPresenter;


    public void BindPresenter(PlanetPopupPresenter planetPopupPresenter)
    {
        _planetPopupPresenter = planetPopupPresenter;
        _planetPopupPresenter.StateChanged += HandleStateChanged;
        _closeButton.onClick.AddListener(CloseButtonClick);
        _upgradeButton.onClick.AddListener(UpgradeButtonClick);
    }

    private void HandleStateChanged()
    {
        _icon.sprite = _planetPopupPresenter.Icon;
        _upgradeButton.interactable = _planetPopupPresenter.IsUpgradeEnabled;
        _populationText.text = _planetPopupPresenter.PopulationText;
        _incomeText.text = _planetPopupPresenter.IncomeText;
        _levelText.text = _planetPopupPresenter.LevelText;
        _titleText.text = _planetPopupPresenter.TitleText;
        _upgradeCostIcon.SetActive(!_planetPopupPresenter.IsMaxLevel);
        _upgradeCostText.text = _planetPopupPresenter.UpgradeCostText;
    }

    public void UnBindPresenter()
    {
        _closeButton.onClick.RemoveListener(CloseButtonClick);
        _upgradeButton.onClick.RemoveListener(UpgradeButtonClick);
        _planetPopupPresenter.StateChanged -= HandleStateChanged;

    }
    private void UpgradeButtonClick()
    {
        _planetPopupPresenter.Upgrade();
    }

    private void CloseButtonClick()
    {
        _planetPopupPresenter.Close();
    }

}