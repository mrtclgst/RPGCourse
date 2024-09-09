using System.Collections.Generic;
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
    private float _freezeTimeDuration;

    [Header("Bounce Info")]
    private float _bounceSpeed = 1f;
    private bool _isBouncing;
    private int _bounceAmount = 0;
    private int _targetIndex = 0;
    private List<Transform> _targetList = new();

    [Header("Pierce Info")]
    private float _pierceAmount;

    [Header("Spin Info")]
    private float _maxTravelDistance;
    private float _spinDuration;
    private float _spinTimer;
    private bool _wasStopped;
    private bool _isSpinning;
    private float _spinDirection;


    private float _damageTimer;
    private float _damageCooldown;
    private int _damage;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
    }
    internal void SetupSword(Vector2 direction, float gravityScale, Player player, float freezeTimeDuration, float returnSpeed, int damage)
    {
        Destroy(gameObject, 7);
        _rigidbody.velocity = direction;
        _rigidbody.gravityScale = gravityScale;
        _player = player;
        _freezeTimeDuration = freezeTimeDuration;
        _returnSpeed = returnSpeed;
        _damage = damage;

        if (_pierceAmount <= 0)
        {
            _animator.SetBool("Rotation", true);
        }

        _spinDirection = Mathf.Clamp(_rigidbody.velocity.x, -1, 1);
    }
    internal void SetupBounce(bool canBounce, int bounceAmount, float bounceSpeed)
    {
        _isBouncing = canBounce;
        _bounceAmount = bounceAmount;
        _bounceSpeed = bounceSpeed;
    }
    internal void SetupPierce(int pierceAmount)
    {
        _pierceAmount = pierceAmount;
    }
    internal void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float damageCooldown)
    {
        _isSpinning = isSpinning;
        _spinDuration = spinDuration;
        _maxTravelDistance = maxTravelDistance;
        _damageCooldown = damageCooldown;
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
                _player.CatchSword();
            }
        }

        BounceLogic();
        SpinLogic();
    }
    private void SpinLogic()
    {
        if (_isSpinning)
        {
            if (Vector2.Distance(_player.transform.position, transform.position) > _maxTravelDistance && !_wasStopped)
            {
                StopWhenSpinning();
            }

            if (_wasStopped)
            {
                _spinTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x + _spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                if (_spinTimer < 0)
                {
                    _isSpinning = false;
                    _isReturning = true;
                }
            }

            _damageTimer -= Time.deltaTime;
            if (_damageTimer < 0)
            {
                _damageTimer = _damageCooldown;
                Collider2D[] targetArray = Physics2D.OverlapCircleAll(transform.position, 1);
                foreach (Collider2D target in targetArray)
                {
                    if (target.GetComponent<Enemy>() != null)
                        SwordSkillDamage(target.GetComponent<Enemy>());
                }
            }
        }
    }
    private void StopWhenSpinning()
    {
        _wasStopped = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;

        if (_spinTimer <= 0)
            _spinTimer = _spinDuration;
    }
    private void BounceLogic()
    {
        if (_isBouncing && _targetList.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetList[_targetIndex].position, _bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _targetList[_targetIndex].position) < 0.2f)
            {
                SwordSkillDamage(_targetList[_targetIndex].GetComponent<Enemy>());
                _targetIndex++;
                _bounceAmount--;

                if (_bounceAmount <= 0)
                {
                    _isBouncing = false;
                    _isReturning = true;
                }

                if (_targetIndex >= _targetList.Count)
                    _targetIndex = 0;

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetsForBouncing(collision);
        StuckInto(collision);
    }
    private void SwordSkillDamage(Enemy enemy)
    {
        if (SkillManager.Instance.GetSkillSword().TimeStopUnlocked)
        {
            enemy.FreezeTimeFor(_freezeTimeDuration);
        }

        if (SkillManager.Instance.GetSkillSword().VulnurabilityUnlocked)
            enemy.GetComponent<EnemyStats>()?.MakeVulnurableFor(_freezeTimeDuration);

        enemy.TakeDamage(_damage, false);

        ItemDataEquipment equippedAmulet = Inventory.Instance.GetEquipment(EquipmentType.Amulet);
        if (equippedAmulet != null)
            equippedAmulet.ExecuteItemEffect(enemy.transform);
    }
    private void SetupTargetsForBouncing(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (_isBouncing && _targetList.Count <= 0)
            {
                Collider2D[] targetArray = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (Collider2D target in targetArray)
                {
                    if (target.GetComponent<Enemy>() != null)
                    {
                        _targetList.Add(target.transform);
                    }
                }
            }
        }
    }
    private void StuckInto(Collider2D collision)
    {
        if (_pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            _pierceAmount--;
            return;
        }
        if (_isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        _canRotate = false;
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        if (_isBouncing && _targetList.Count > 0)
            return;

        _animator.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
    public void ReturnSword()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        //_rigidbody.isKinematic = false;
        transform.parent = null;
        _isReturning = true;
    }
    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}