using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetPopupView : MonoBehaviour
{
    private const string MAX_LEVEL_STRING = "Max level";

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TextMeshProUGUI _upgradeCostText;
    [SerializeField] private GameObject _upgradeCostIcon;
    [SerializeField] private TextMeshProUGUI _populationText;
    [SerializeField] private TextMeshProUGUI _incomeText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private Image _icon;

    private void Awake()
    {
        _closeButton.onClick.AddListener(() => OnClose?.Invoke());
        _upgradeButton.onClick.AddListener(() => OnUpgrade?.Invoke());
    }

    public event Action OnClose;
    public event Action OnUpgrade;

    public void SetUpgradeEnabled(bool value)
    {
        _upgradeButton.interactable = value;
    }

    public void SetTitle(string value)
    {
        _titleText.text = value;
    }

    public void SetPopulation(string value)
    {
        _populationText.text = value;
    }

    public void SetLevel(string value)
    {
        _levelText.text = value;
    }

    public void SetIncome(string value)
    {
        _incomeText.text = value;
    }

    public void SetUpgradeCost(string value)
    {
        _upgradeCostText.text = value;
        _upgradeCostIcon.SetActive(true);
    }

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
    }

    public void SetIsMaxLevel()
    {
        _upgradeCostText.text = MAX_LEVEL_STRING;
        _upgradeCostIcon.SetActive(false);
    }
}