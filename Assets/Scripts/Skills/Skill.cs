using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float _cooldown;
    protected float _cooldownTimer;

    protected Player _player;

    protected virtual void Start()
    {
        _player = PlayerManager.Instance.Player;
    }
    protected virtual void Update()
    {
        _cooldownTimer -= Time.deltaTime;
    }

    internal virtual bool CanUseSkill()
    {
        if (_cooldownTimer <= 0)
        {
            return true;
        }

        return false;
    }

    internal virtual void UseSkill()
    {
        if (CanUseSkill())
        {
            _cooldownTimer = _cooldown;
        }
        else
        {
            Debug.Log("can not use skill");
            return;
        }
    }

    protected virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkTransform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        return closestEnemy;
    }

    public float GetCooldown()
    {
        return _cooldown;
    }
}
