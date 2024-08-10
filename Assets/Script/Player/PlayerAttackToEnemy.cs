using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackToEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.TryGetComponent<Enemy>(out var _))
        {
            transform.root.GetComponent<PlayerAttackState>().DealDamageToEnemy(collision);
        }
    }
}
