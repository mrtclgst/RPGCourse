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
        GameManager.Instance.LostCurrencyAmount = PlayerManager.Instance.Currency;
        PlayerManager.Instance.Currency = 0;
        base.Die();
    }
    protected override void DecreaseHealthBy(int damage)
    {
        if (damage > GetMaxHealth() * 0.3f)
        {
            _player.EntityFX.ScreenShake(_player.EntityFX._shakeHighDamage);
        }

        base.DecreaseHealthBy(damage);
        ItemDataEquipment currentArmor = Inventory.Instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null)
        {
            currentArmor.ExecuteItemEffect(transform);
        }
    }
    public override void OnEvasion()
    {
        SkillManager.Instance.GetSkillDodge().CreateMirageOnDodge();
        Debug.Log("Player avoided attack");
    }
}