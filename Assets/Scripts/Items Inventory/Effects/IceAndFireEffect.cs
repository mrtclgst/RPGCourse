using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/Item Effect/Ice and Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject _iceAndFirePrefab;
    [SerializeField] private float _xVelocity;

    public override void ExecuteEffect(Transform respawnTransform)
    {
        Player player = PlayerManager.Instance.Player;
        bool thirdAttack = player.PrimaryAttackState.GetComboCount() == 2;

        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(_iceAndFirePrefab, respawnTransform.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(_xVelocity * player.GetFacingDirection(), 0);
            Destroy(newIceAndFire, 7);
        }
    }
}