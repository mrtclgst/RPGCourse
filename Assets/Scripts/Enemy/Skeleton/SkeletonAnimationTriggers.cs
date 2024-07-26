using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    [SerializeField] private EnemySkeleton _enemySkeleton;

    private void AnimationTrigger()
    {
        _enemySkeleton.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        _enemySkeleton.DealDamage();
    }
    private void OpenCounterWindow()
    {
        _enemySkeleton.OpenCounterAttackWindow();
    }
    private void CloseCounterWindow()
    {
        _enemySkeleton.CloseCounterAttackWindow();
    }
}