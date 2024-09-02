using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _skillName;
    [TextArea]
    [SerializeField] private string _skillDescription;
    [SerializeField] private int _skillPrice;
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
    }

    public void UnlockSkillSlot()
    {
        if (!PlayerManager.Instance.HaveEnoughMoney(_skillPrice))
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

        PlayerManager.Instance.SpendCurrency(_skillPrice);
        Unlocked = true;
        _skillImage.color = Color.white;
    }
    private void OnValidate()
    {
        gameObject.name = _skillName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.SkillTooltip.ShowTooltip(_skillName, _skillDescription);

        //Vector2 mousePosition = Input.mousePosition;
        //float xOffset = 0;
        //if (mousePosition.x > 600)
        //{
        //    xOffset = -150;
        //}
        //_ui.SkillTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.SkillTooltip.HideTooltip();
    }
}
