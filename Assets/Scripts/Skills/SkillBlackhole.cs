using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBlackhole : Skill
{
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
    }

    protected override void Update()
    {
        base.Update();
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
}