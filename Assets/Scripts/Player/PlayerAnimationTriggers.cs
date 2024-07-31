using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    [SerializeField] Player _player;

    private void AnimationTrigger()
    {
        _player.AnimationTrigger();
    }
    private void AttackTrigger()
    {
        _player.DealDamage();
    }
    private void ThrowSword()
    {
        SkillManager.Instance.GetSkillSword().CreateSword();
    }
}
