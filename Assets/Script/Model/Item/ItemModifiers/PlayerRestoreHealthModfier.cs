using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Game/HealingModifier")]
public class PlayerRestoreHealthModfier : PlayerStatsModifierSO
{
    public override void AffectPlayer(GameObject player, float val)
    {
        if (player.TryGetComponent<Health>(out var playerHealth))
        {
            playerHealth.Healing(val);
        }
    }
}
