using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player _player;

    protected override void Start()
    {
        base.Start();
        _player = GetComponent<Player>();
    }
    internal override void TakeDamage(int damage, bool isShocked)
    {
        base.TakeDamage(damage, isShocked);
    }
    protected override void Die()
    {
        base.Die();
        _player.Die();
    }
}