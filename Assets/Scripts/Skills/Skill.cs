using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float _cooldown;
    protected float _cooldownTimer;

    protected Player _player;

    protected virtual void Start()
    {
        _player = PlayerManager.Instance.Player;
        Invoke("CheckUnlock", 0.1f);
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

        _player.PlayerFX.CreatePopupText("Cooldown");
        return false;
    }

    internal virtual void UseSkill()
    {
        if (CanUseSkill())
        {
            _cooldownTimer = _cooldown;
            Debug.Log("skill used");
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

    protected virtual void CheckUnlock()
    {

    }

    public virtual float GetCooldown()
    {
        return _cooldown;
    }

    public float GetCooldownTimer()
    { return _cooldownTimer; }
}
