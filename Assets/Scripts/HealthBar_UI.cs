using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity _entity;
    private RectTransform _rectTransform;
    private Slider _slider;
    private CharacterStats _characterStats;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _entity = GetComponentInParent<Entity>();
        _slider = GetComponentInChildren<Slider>();
        _characterStats = GetComponentInParent<CharacterStats>();

        _entity.Entity_OnFlipped += OnEntityFlipped;
        _characterStats.CharacterStats_OnHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }
    private void OnDestroy()
    {
        _entity.Entity_OnFlipped -= OnEntityFlipped;
        _characterStats.CharacterStats_OnHealthChanged -= UpdateHealthUI;
    }

    private void OnEntityFlipped()
    {
        _rectTransform.Rotate(0, 180, 0);
    }

    private void UpdateHealthUI()
    {
        _slider.value = _characterStats.GetCurrentHealthPercentage();
    }
}