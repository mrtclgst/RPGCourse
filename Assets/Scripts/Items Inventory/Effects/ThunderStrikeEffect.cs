using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effect/Thunder Strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject _thunderStrikePrefab;

    public override void ExecuteEffect(Transform enemyTransform)
    {
        GameObject newThunderStrike = Instantiate(_thunderStrikePrefab, enemyTransform.position, Quaternion.identity);
        Destroy(newThunderStrike, 1f);
    }
}
