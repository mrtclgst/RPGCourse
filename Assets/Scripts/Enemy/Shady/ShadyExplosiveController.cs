using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyExplosiveController : MonoBehaviour
{
    private Animator _animator;
    private CharacterStats _stats;
    private float _growSpeed = 15;
    private float _maxSize = 6;
    private float _explosionRadius;

    private bool _canGrow = true;

    private void Update()
    {
        if (_canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one * _maxSize, _growSpeed * Time.deltaTime);
        }

        if (_maxSize - transform.localScale.x < 0.5f)
        {
            _canGrow = false;
            _animator.SetTrigger("Explode");
        }
    }

    internal void SetupExplosive(CharacterStats stats, float growSpeed, float maxSize, float radius)
    {
        _animator = GetComponent<Animator>();
        _stats = stats;
        _growSpeed = growSpeed;
        _maxSize = maxSize;
        _explosionRadius = radius;
        Debug.Log("size " + _maxSize + "speed " + _growSpeed);
    }

    private void AnimationEventExplosion()
    {
        Collider2D[] damageableArray = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (var damageable in damageableArray)
        {
            if (damageable.GetComponent<CharacterStats>() != null)
            {
                //damageable.GetComponent<Enemy>().TakeDamage(_damage, false);
                damageable.GetComponent<Entity>().SetKnockbackDirection(this.transform);
                _stats.DealMagicalDamage(damageable.GetComponent<CharacterStats>());

                //ItemDataEquipment equippedAmulet = Inventory.Instance.GetEquipment(EquipmentType.Amulet);
                //if (equippedAmulet != null)
                //    equippedAmulet.ExecuteItemEffect(damageable.transform);
            }
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}