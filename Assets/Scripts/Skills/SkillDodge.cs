using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDodge : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot _unlockDodgeButton;
    [SerializeField] private int _evasionAmount;
    private bool _dodgeUnlocked;

    [Header("Mirage Dodge")]
    [SerializeField] private UI_SkillTreeSlot _unlockMirageDodgeButton;
    private bool _dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();
        _unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        _unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    private void UnlockDodge()
    {
        if (_unlockDodgeButton.Unlocked && !_dodgeUnlocked)
        {
            _player.Stats.Evasion.AddModifier(_evasionAmount);
            _dodgeUnlocked = true;
            Inventory.Instance.UpdateStatsUI();
        }
    }
    private void UnlockMirageDodge()
    {
        if (_unlockMirageDodgeButton.Unlocked)
            _dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (_dodgeMirageUnlocked)
            SkillManager.Instance.GetSkillClone().CreateClone(_player.transform, new Vector3(Random.Range(-0.5f, 0.5f), 0, 0));
    }
}
