using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal")]
public class HealEffect : ItemEffect
{
    [Range(0, 1f)]
    [SerializeField] private float _healPercentage;
    public override void ExecuteEffect(Transform targetTransform)
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.Stats as PlayerStats;
        int healthAmount = Mathf.RoundToInt(playerStats.GetMaxHealth() * _healPercentage);
        playerStats.IncreaseHealthBy(healthAmount);
    }
}