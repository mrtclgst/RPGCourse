using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillSwordController : MonoBehaviour
{
    [SerializeField] private float _returnSpeed = 12;
    [SerializeField] private float _destroyDistanceBtwPlayer = 1f;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    private Player _player;
    private bool _canRotate = true;
    private bool _isReturning;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
    }
    public void SetupSword(Vector2 direction, float gravityScale, Player player)
    {
        _rigidbody.velocity = direction;
        _rigidbody.gravityScale = gravityScale;
        _player = player;
        _animator.SetBool("Rotation", true);
    }
    private void Update()
    {
        if (_canRotate)
        { transform.right = _rigidbody.velocity; }

        if (_isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _player.transform.position) < _destroyDistanceBtwPlayer)
            {
                _player.ClearSword();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _animator.SetBool("Rotation", false);
        _canRotate = false;
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
    }
    public void ReturnSword()
    {
        _rigidbody.isKinematic = false;
        transform.parent = null;
        _isReturning = true;
    }
}