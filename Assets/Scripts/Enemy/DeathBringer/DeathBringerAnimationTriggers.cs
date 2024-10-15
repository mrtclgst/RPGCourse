using System.Collections;
using UnityEngine;

public class DeathBringerAnimationTriggers : EnemyAnimationTriggers
{
    private EnemyDeathBringer _enemyDeathBringer;
    private void Start()
    {
        _enemyDeathBringer = GetComponentInParent<EnemyDeathBringer>();
    }
    private void Relocate()
    {
        _enemyDeathBringer.FindPosition();
    }
    private void MakeInvisible()
    {
        _enemyDeathBringer.EntityFX.MakeTransparent(true);
    }
    private void MakeVisible()
    {
        _enemyDeathBringer.EntityFX.MakeTransparent(false);
    }
}