using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    [SerializeField] Player player;

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
}
