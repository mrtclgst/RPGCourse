using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private LayerMask _player;
    [SerializeField] private LayerMask _enemy;
    private LayerMask _targetLayer;

    [SerializeField] private int _damage;
    [SerializeField] Rigidbody2D _rigidbody;

    [SerializeField] private bool _canMove = true;
    [SerializeField] private bool _isFlipped;
    [SerializeField] private float _xVelocity = 2f;
    [SerializeField] private float _yVelocity = 2f;

    private CharacterStats _archerStats;

    private void Start()
    {
        _targetLayer = _player;
        _canMove = true;
    }
    private void Update()
    {
        if (_canMove)
            _rigidbody.velocity = new Vector2(_xVelocity, _yVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _targetLayer) != 0)
        {
            //collision.GetComponent<CharacterStats>().TakeDamage(_damage, false);
            _archerStats.DealDamage(collision.GetComponent<CharacterStats>());
            StuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
        }
    }

    private void StuckInto(Collider2D collision)
    {
        _canMove = false;
        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponentInChildren<TrailRenderer>().enabled = false;
        Destroy(gameObject, Random.Range(2, 3));
    }

    public void FlipArrow()
    {
        if (_isFlipped)
            return;

        _xVelocity *= -1f; _yVelocity *= -1f;
        transform.Rotate(0, 180, 0);
        _isFlipped = true;
        _targetLayer = _enemy;
    }

    public void SetupArrow(float speed,CharacterStats characterStats)
    {
        _xVelocity = speed;
        _archerStats = characterStats;
    }
}