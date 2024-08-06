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
    private Transform _closestEnemy;
    private int _direction;

    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMoveToEnemy, float moveSpeed, Transform closestEnemy)
    {
        _crystalExistTimer = crystalDuration;
        _canExplode = canExplode;
        _canMove = canMoveToEnemy;
        _moveSpeed = moveSpeed;
        _closestEnemy = closestEnemy;
        if (_closestEnemy == null)
        {
            _direction = PlayerManager.Instance.Player.GetFacingDirection();
        }
    }

    private void Update()
    {
        _crystalExistTimer -= Time.deltaTime;

        if (_canMove)
        {
            if (_closestEnemy != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, _closestEnemy.position, _moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, _closestEnemy.position) < 1)
                {
                    CrystalExplosionLogic();
                    _canMove = false;
                }
            }
            else
            {
                Vector3 direction = new Vector3(_direction, 0);
                transform.Translate(direction * _moveSpeed * Time.deltaTime);
            }
        }

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