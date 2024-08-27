using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class ThunderStrikeEffectController : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            playerStats.DealMagicalDamage(enemyStats);
        }
    }
}
