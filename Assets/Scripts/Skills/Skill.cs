using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float _cooldown;
    protected float _cooldownTimer;

    protected virtual void Update()
    {
        _cooldownTimer -= Time.deltaTime;
    }

    internal virtual bool CanUseSkill()
    {
        if (_cooldownTimer < 0)
        {
            return true;
        }

        return false;
    }

    internal virtual void UseSkill()
    {
        if (!CanUseSkill()) 
        { return; }

        _cooldownTimer = _cooldown;
    }
}
