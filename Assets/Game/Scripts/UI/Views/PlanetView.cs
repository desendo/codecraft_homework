using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Modules.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IPlanetView
{
    Transform CoinProjectileAnimationAnchor { get; }
    event Action OnHold;
    event Action OnClick;
    void SetLocked(bool locked);
    void SetReady(bool ready);
    void SetIcon(Sprite icon);
    void SetTimer(string value);
    void SetProgress(float value);
    void SetPrice(string value);
}

public class PlanetView : MonoBehaviour, IPlanetView
{
    [SerializeField] private SmartButton _button;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private Image _progress;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private GameObject _lock;
    [SerializeField] private GameObject _unlockPriceObject;
    [SerializeField] private GameObject _timerObject;
    [SerializeField] private Image _planetImage;
    [SerializeField] private GameObject _coinObject;
    private bool _locked;
    private bool _ready;

    public Transform CoinProjectileAnimationAnchor => _coinObject.transform;

    public event Action OnHold
    {
        add => _button.OnHold += value;
        remove => _button.OnHold -= value;
    }

    public event Action OnClick
    {
        add => _button.OnClick += value;
        remove => _button.OnClick -= value;
    }


    public void SetLocked(bool locked)
    {
        _locked = locked;
        _lock.gameObject.SetActive(locked);
        _unlockPriceObject.gameObject.SetActive(locked);
        _timerObject.SetActive(!_locked && !_ready);

        if (locked)
            SetReady(false);
    }

    public void SetReady(bool ready)
    {
        _ready = ready;
        _timerObject.SetActive(!_locked && !_ready);
        _coinObject.gameObject.SetActive(ready);

    }


    public void SetIcon(Sprite icon)
    {
        _planetImage.sprite = icon;
    }
    public void SetTimer(string value)
    {
        _timer.text = value;
    }
    public void SetProgress(float value)
    {
        _progress.fillAmount = value;
    }
    public void SetPrice(string value)
    {
        _price.text = value;
    }

 
}
