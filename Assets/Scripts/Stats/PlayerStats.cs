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
        _player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
        base.Die();
    }
}