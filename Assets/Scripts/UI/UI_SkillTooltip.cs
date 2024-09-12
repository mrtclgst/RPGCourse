using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillDescription;
    [SerializeField] private TextMeshProUGUI _skillCost;

    public void ShowTooltip(string skillName, string skillDescription, int skillCost)
    {
        _skillName.text = skillName;
        _skillDescription.text = skillDescription;
        _skillCost.text = "Cost: " + skillCost;
        gameObject.SetActive(true);
    }
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}