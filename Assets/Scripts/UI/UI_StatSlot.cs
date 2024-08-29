using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _statName;
    [SerializeField] private StatType _statType;
    [SerializeField] private TextMeshProUGUI _statValueText;
    [SerializeField] private TextMeshProUGUI _statNameText;

    [TextArea]
    [SerializeField] private string _statDescription;

    private UI _ui;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + _statName;
        if (_statNameText != null)
            _statNameText.text = _statName;
    }

    private void Start()
    {
        UpdateStatValueUI();
        _ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.Stats as PlayerStats;

        if (playerStats != null)
        {
            _statValueText.text = playerStats.StatOfType(_statType).GetValue().ToString();

            if (_statType == StatType.Health)
                _statValueText.text = playerStats.GetMaxHealth().ToString();

            if (_statType == StatType.Damage)
                _statValueText.text = (playerStats.Strength.GetValue() + playerStats.Damage.GetValue()).ToString();

            if (_statType == StatType.CritDamage)
                _statValueText.text = (playerStats.CritDamage.GetValue() + playerStats.Strength.GetValue()).ToString();

            if (_statType == StatType.CritChance)
                _statValueText.text = (playerStats.CritChance.GetValue() + playerStats.Agility.GetValue()).ToString();

            if (_statType == StatType.Evasion)
                _statValueText.text = (playerStats.Evasion.GetValue() + playerStats.Agility.GetValue()).ToString();

            if (_statType == StatType.MagicResistance)
                _statValueText.text = (playerStats.MagicResistance.GetValue() + playerStats.Intelligence.GetValue() * 3).ToString();

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.StatTooltip.ShowStatTooltip(_statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.StatTooltip.HideStatTooltip();
    }
}
