using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    [SerializeField] private string _skillName;
    [TextArea]
    [SerializeField] private string _skillDescription;
    [SerializeField] private int _skillCost;
    [SerializeField] private Color _skillLockedColor;

    public bool Unlocked;

    [SerializeField] private UI_SkillTreeSlot[] _shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] _shouldBeLocked;

    private Image _skillImage;

    private UI _ui;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        _skillImage = GetComponent<Image>();
        _skillImage.color = _skillLockedColor;
        _ui = GetComponentInParent<UI>();

        if (Unlocked)
            _skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (!PlayerManager.Instance.HaveEnoughMoney(_skillCost))
            return;

        for (int i = 0; i < _shouldBeUnlocked.Length; i++)
        {
            if (_shouldBeUnlocked[i].Unlocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }
        for (int i = 0; i < _shouldBeLocked.Length; i++)
        {
            if (_shouldBeLocked[i].Unlocked == true)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        PlayerManager.Instance.SpendCurrency(_skillCost);
        Unlocked = true;
        _skillImage.color = Color.white;
    }
    private void OnValidate()
    {
        gameObject.name = _skillName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.SkillTooltip.ShowTooltip(_skillName, _skillDescription, _skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.SkillTooltip.HideTooltip();
    }

    public void LoadData(GameData gameData)
    {
        if(gameData.SkillTree.TryGetValue(_skillName,out bool value))
        {
            Unlocked = value;
        }
    }

    public void SaveData(ref GameData gameData)
    {
        if (gameData.SkillTree.TryGetValue(_skillName, out bool value))
        {
            gameData.SkillTree.Remove(_skillName);
            gameData.SkillTree.Add(_skillName, Unlocked);
        }
        else
        {
            gameData.SkillTree.Add(_skillName, Unlocked);
        }
    }
}
