using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBlackhole : Skill
{
    [SerializeField] private UI_SkillTreeSlot _blackHoleUnlockButton;
    public bool BlackholeUnlocked { get; private set; }
    [SerializeField] private GameObject _blackholePrefab;
    [SerializeField] private float _maxSize;
    [SerializeField] private float _growSpeed;
    [SerializeField] private float _shrinkSpeed;
    [SerializeField] private int _attackAmount;
    [SerializeField] private float _cloneAttackCooldown;
    [SerializeField] private float _blackholeDuration;

    SkillBlackholeController currentBlackholeController;



    protected override void Start()
    {
        base.Start();
        _blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }
    protected override void CheckUnlock()
    {
        UnlockBlackhole();
    }

    protected override void Update()
    {
        base.Update();
    }
    private void UnlockBlackhole()
    {
        if (_blackHoleUnlockButton.Unlocked)
            BlackholeUnlocked = true;
    }
    internal override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    internal override void UseSkill()
    {
        base.UseSkill();
        GameObject blackhole = Instantiate(_blackholePrefab, _player.transform.position, Quaternion.identity);
        currentBlackholeController = blackhole.GetComponent<SkillBlackholeController>();
        currentBlackholeController.SetupBlackhole(_maxSize, _growSpeed, _shrinkSpeed, _attackAmount, _cloneAttackCooldown, _blackholeDuration);
        AudioManager.Instance.PlaySFX(32, _player.transform);
        AudioManager.Instance.PlaySFX(29, _player.transform);
    }

    public bool BlackholeFinished()
    {
        if (!currentBlackholeController)
            return false;

        if (currentBlackholeController.PlayerCanExit)
        {
            currentBlackholeController = null;
            return true;
        }

        return false;
    }

    internal float GetBlackholeRadius()
    {
        return _maxSize / 2f;
    }
}