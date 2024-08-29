using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _description;

    public void ShowStatTooltip(string text)
    {
        _description.text = text;
        gameObject.SetActive(true);
    }
    public void HideStatTooltip()
    {
        _description.text = "";
        gameObject.SetActive(false);
    }
}
