using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats _targetStats;
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;

    private bool _triggered;
    private int _damage;

    private void Update()
    {
        if (_triggered || !_targetStats)
            return;

        transform.position = Vector2.MoveTowards(transform.position, _targetStats.transform.position, _speed * Time.deltaTime);
        transform.up = (_targetStats.transform.position - transform.position) * -1f;

        if (Vector2.Distance(transform.position, _targetStats.transform.position) < 0.1f)
        {
            _animator.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(2, 2);

            _triggered = true;
            _targetStats.TakeDamage(_damage, true);
            _animator.SetTrigger("Hit");
            Destroy(gameObject, 0.4f);
        }
    }

    public void Setup(int damage, CharacterStats targetStats)
    {
        _damage = damage;
        _targetStats = targetStats;
    }
}