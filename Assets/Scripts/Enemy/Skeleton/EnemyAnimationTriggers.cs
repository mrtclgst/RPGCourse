using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void AnimationTrigger()
    {
        _enemy.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        _enemy.DealDamage();
    }
    private void OpenCounterWindow()
    {
        _enemy.OpenCounterAttackWindow();
    }
    private void CloseCounterWindow()
    {
        _enemy.CloseCounterAttackWindow();
    }
    private void SpecialAttackTrigger()
    {
        _enemy.AnimationSpecialAttackTrigger();
    }
}