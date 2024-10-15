using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastController : MonoBehaviour
{
    [SerializeField] private Transform _checkTransform;
    [SerializeField] private Vector2 _boxSize;
    [SerializeField] private LayerMask _whatIsPlayer;

    private CharacterStats _stats;
    internal void SetupSpell(CharacterStats stats)
    {
        _stats = stats;
    }

    private void AnimationTrigger()
    {
        Collider2D[] damageableArray = Physics2D.OverlapBoxAll(_checkTransform.position, _boxSize, 0, _whatIsPlayer);
        foreach (var damageable in damageableArray)
        {
            if (damageable.GetComponent<Player>() != null)
            {
                //damageable.GetComponent<Enemy>().TakeDamage(_damage, false);
                damageable.GetComponent<Entity>().SetKnockbackDirection(this.transform);
                _stats.DealDamage(damageable.GetComponent<CharacterStats>());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_checkTransform.position, _boxSize);
    }
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
