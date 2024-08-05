using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillCrystalController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CircleCollider2D _collider;
    private float _crystalExistTimer;
    private bool _canExplode;
    private bool _canMove;
    private float _moveSpeed;

    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMoveToEnemy, float moveSpeed)
    {
        _crystalExistTimer = crystalDuration;
        _canExplode = canExplode;
        _canMove = canMoveToEnemy;
        _moveSpeed = moveSpeed;
    }

    private void Update()
    {
        _crystalExistTimer -= Time.deltaTime;
        if (_crystalExistTimer < 0)
        {
            CrystalExplosionLogic();
        }
    }

    internal void CrystalExplosionLogic()
    {
        if (_canExplode)
        {
            _animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    private void AnimationEventExplosion()
    {
        Collider2D[] damageableArray = Physics2D.OverlapCircleAll(transform.position, _collider.radius);
        foreach (var damageable in damageableArray)
        {
            if (damageable.GetComponent<Enemy>() != null)
            {
                damageable.GetComponent<Enemy>().TakeDamage();
            }
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}