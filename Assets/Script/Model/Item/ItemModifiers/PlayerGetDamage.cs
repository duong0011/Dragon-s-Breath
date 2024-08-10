using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName ="My Game/DamagePersecond")]
public class PlayerGetDamage : PlayerStatsModifierSO
{
    public override void AffectPlayer(GameObject player, float val)
    {
        CoroutineHelper.Instance.StartCoroutine(TriggerPoison(player, val));
    }

    private IEnumerator TriggerPoison(GameObject player, float damage)
    {
        player.GetComponent<SpriteRenderer>().color = Color.green;
        for (int i = 0; i < 5; i++)
        {
            player.GetComponent<Health>().DamageTaken(damage);
            yield return new WaitForSeconds(1f);
        }
        player.GetComponent<SpriteRenderer>().color = Color.white;
    }
}