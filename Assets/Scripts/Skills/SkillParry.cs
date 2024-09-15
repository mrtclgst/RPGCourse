using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillParry : Skill
{
    [Header("Just Parry")]
    [SerializeField] private UI_SkillTreeSlot _parryUnlockButton;
    public bool ParryUnlocked;

    [Header("Parry Restore")]
    [SerializeField] private UI_SkillTreeSlot _parryWithRestoreButton;
    [Range(0,1f)]
    [SerializeField] private float _restorePercentage;
    public bool RestoringParryUnlocked;

    [Header("Parry With Mirage")]
    [SerializeField] private UI_SkillTreeSlot _parryWithMirageUnlockButton;
    public bool ParryWithMirageUnlocked;


    protected override void Start()
    {
        base.Start();
        _parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        _parryWithRestoreButton.GetComponent<Button>().onClick.AddListener(UnlockRestoringParry);
        _parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }
    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockRestoringParry();
        UnlockParryWithMirage();
    }
    internal override void UseSkill()
    {
        base.UseSkill();
        if (RestoringParryUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(_player.Stats.GetMaxHealth() * _restorePercentage);
            _player.Stats.IncreaseHealthBy(restoreAmount);
        }
    }
    private void UnlockParry()
    {
        if (_parryUnlockButton.Unlocked)
            ParryUnlocked = true;
    }
    private void UnlockRestoringParry()
    {
        if (_parryWithRestoreButton.Unlocked)
            RestoringParryUnlocked = true;
    }
    private void UnlockParryWithMirage()
    {
        if (_parryWithMirageUnlockButton.Unlocked)
            ParryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform mirageSpawnTransform)
    {
        if(ParryWithMirageUnlocked)
        {
            SkillManager.Instance.GetSkillClone().CreateCloneOnCounterAttack(mirageSpawnTransform,0.3f);
        }
    }
}