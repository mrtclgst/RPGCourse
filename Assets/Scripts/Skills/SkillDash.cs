using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDash : Skill
{
    [Header("Dash")]
    public bool DashUnlocked;
    [SerializeField] private UI_SkillTreeSlot _dashUnlockButton;

    [Header("Clone on Dash")]
    public bool CloneOnDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot _cloneOnDashUnlockButton;

    [Header("Clone on Arrival")]
    public bool CloneOnArrivalUnlocked;
    [SerializeField] private UI_SkillTreeSlot _cloneOnArrivalUnlockButton;

    protected override void Start()
    {
        base.Start();
        _dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        _cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        _cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }
    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }
    internal override void UseSkill()
    {
        base.UseSkill();
    }

    private void UnlockDash()
    {
        if (_dashUnlockButton.Unlocked)
            DashUnlocked = true;
    }
    private void UnlockCloneOnDash()
    {
        if (_cloneOnDashUnlockButton.Unlocked)
            CloneOnDashUnlocked = true;
    }
    private void UnlockCloneOnArrival()
    {
        if (_cloneOnArrivalUnlockButton.Unlocked)
            CloneOnArrivalUnlocked = true;
    }

    public void CloneOnDash()
    {
        if (CloneOnDashUnlocked)
            SkillManager.Instance.GetSkillClone().CreateClone(_player.transform, Vector3.zero);
    }
    public void CloneOnArrival()
    {
        if (CloneOnArrivalUnlocked)
        {
            SkillManager.Instance.GetSkillClone().CreateClone(_player.transform, Vector3.zero);
        }
    }
}