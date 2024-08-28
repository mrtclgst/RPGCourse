using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/Item Effect/Freeze Enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float _duration;

    public override void ExecuteEffect(Transform targetTransform)
    {
        if (!Inventory.Instance.CanUseArmor())
            return;
        if (PlayerManager.Instance.Player.Stats.GetCurrentHealthPercentage() > 0.1f)
            return;


        Collider2D[] enemies = Physics2D.OverlapCircleAll(targetTransform.position, 2f);
        foreach (Collider2D enemyCollider in enemies)
        {
            enemyCollider.GetComponent<Enemy>()?.FreezeTimeFor(_duration);
        }
    }
}