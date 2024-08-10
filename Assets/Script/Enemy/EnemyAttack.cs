using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy m_Enemy;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Enemy.DealDamageToPlayer(collision);
    }
}
