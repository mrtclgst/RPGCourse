using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class BuffEffect : ItemEffect
{
    private PlayerStats _playerStats;
    [SerializeField] private StatType _buffType = StatType.Health;
    [SerializeField] private int _buffAmount;
    [SerializeField] private int _buffDuration;

    public override void ExecuteEffect(Transform targetTransform)
    {
        _playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();
        _playerStats.IncreaseStatBy(_buffAmount, _buffDuration, _playerStats.StatOfType(_buffType));
    }
}