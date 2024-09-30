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

    [SerializeField] private bool _isFlipped;
    [SerializeField] private float _xVelocity = 2f;
    [SerializeField] private float _yVelocity = 2f;

    private void Start()
    {
        _targetLayer = _player;
    }
    private void Update()
    {
        _rigidbody.velocity = new Vector2(_xVelocity, _yVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _targetLayer)
        {
            Debug.Log("hit");
            collision.GetComponent<CharacterStats>().TakeDamage(_damage, false);
            Destroy(gameObject);
        }
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
}
