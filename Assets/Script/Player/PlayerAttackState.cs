using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinding.Util.RetainedGizmos;

public class PlayerAttackState : MonoBehaviour
{
    private Rigidbody2D body;
    private PlayerMovement playerMovement;
    private Animator animator;
    private PlayerStatsControl playerData;

    [SerializeField] private Transform pointCast;
    [SerializeField] private GameObject[] skills;

    private int countAttack;
    private float cooldownTimer = Mathf.Infinity;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerData = GetComponent<PlayerStatsControl>();
    }
  
    void Update()
    {
        if (playerMovement.IsGrounded() || playerMovement.IsTouchingWall()) countAttack = 0;
        if (Input.GetMouseButtonDown(0) && cooldownTimer >= playerData.data.GetStat(Stat.AttackCooldown) && playerMovement.CanAttack() && countAttack <= 3)
        {
            if (!playerMovement.IsGrounded()) 
            {
                body.velocity = new Vector2(0, 2f);
                playerMovement.jumpCount++;
                countAttack++;
            }
            Attack();
            playerMovement.canMove = false;
        }
        cooldownTimer += Time.deltaTime;
    }
    private void Attack()
    {  
        animator.SetTrigger("attack");
        cooldownTimer = 0;

    }
    public void DealDamageToEnemy(Collider2D collider)
    {
        bool crit = Random.Range(1, 100) <= 40;
        collider.GetComponent<Enemy>().TakenDamage(transform, playerData.data.stats[Stat.Damage], crit);
    }
  
    public void CanMoveNow()
    {
        playerMovement.canMove = true;
    }

    public void DealDamageToEnemy(Transform enemy, float damage, bool isCritHit)
    {
        float filnalDmg = isCritHit ? damage * 2 : damage;
        enemy.GetComponent<Enemy>().TakenDamage(transform, filnalDmg, isCritHit);
    }
}
