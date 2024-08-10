using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;



public class Enemy : MonoBehaviour
{

    [SerializeField] private Canvas healthBar;
    [SerializeField] private Image fillHeathBar;
    public float distanceAttack ;
    [Header("Stat Data")]
    public Stats data; 
    [Header("Enemy")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private EnemyAttackType enemyAttackType;
   
    [SerializeField] private GameObject[] baseAttack;
    
    [SerializeField] protected Transform spawnPoint;

    [SerializeField]
    private EnemyDropItemSO itemsDrop;
    
    public float Damage {  get; private set; }
    
    public Transform Player { private set; get;}
    public event Action<EnemyDropItemSO, Collider2D> OnDropItem;

    public float PercentCurentHeal {  get; private set;}
    protected Vector3 firstPos;
    
    protected enum EnemyState
    {
        Idle,
        Attack,
        Chase,
        Walk,
        Die
    }
    private enum EnemyAttackType
    {
       ShortAttack,
       LongAttack
    }

    protected EnemyState enemyState = EnemyState.Idle;
    protected bool canAttack;
    private float priviousX;
    private float cooldownTimer;
    private Animator animator;
    private Rigidbody2D rb;

    private RigidbodyType2D baseBodyType;

    public Dictionary<Stat, float> localStats = new();
    public float CurrentHealth { get; set; }
    protected bool isJustBack = false;
    public bool isDeath = false;
    


    protected virtual void Awake()
    {
        localStats = new Dictionary<Stat, float>(data.stats);
        CurrentHealth = localStats[Stat.MaxHealth];
        priviousX = transform.localScale.x;
        animator = GetComponent<Animator>();
        cooldownTimer = localStats[Stat.AttackCooldown];
        firstPos = spawnPoint.position;
        rb = GetComponent<Rigidbody2D>();
        distanceAttack = localStats[Stat.DistanceAttack];
        Damage = localStats[Stat.Damage];
        baseBodyType = rb.bodyType;
        SetBaseAttack();
        
    }
    

    protected void CallPerFrame()
    {
       
        //kiem tra vi tri nhan vat
        Player = CheckObjectsInCircle();
        //flip theo huong nhan vat
        Flip();
        if (Vector2.Distance(transform.position, firstPos) >= localStats[Stat.DistanceBack] || transform.position.y - firstPos.y <= -localStats[Stat.DistanceBack] / 2f)
        {
            CurrentHealth = localStats[Stat.MaxHealth];
            transform.position = firstPos;
            isJustBack = true; 
        }
        if (Player != null)
        {
            canAttack = Vector3.Distance(transform.position, Player.position) <= localStats[Stat.DistanceAttack];
        }
        if (Player == null)
        {
            canAttack = false;
            CurrentHealth = localStats[Stat.MaxHealth];
        }
        PercentCurentHeal = CurrentHealth /localStats[Stat.MaxHealth];
        fillHeathBar.fillAmount = PercentCurentHeal;
        cooldownTimer = Mathf.Clamp(Time.deltaTime + cooldownTimer, 0, localStats[Stat.AttackCooldown] + 1f);
        UpdateState();

    }

    protected void FlipHeathBar()
    {
        if (transform.localScale.x != priviousX) healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x * -1, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        priviousX = transform.localScale.x;
    }
    protected void LateCallPerFrame()
    {
        isJustBack = false;
    }


    //flip
    protected void Flip()
    {
      
        if (enemyState == EnemyState.Attack)
        {
            Vector3 direction = Player.position - transform.position;
            direction.y = 0; // Keep only the horizontal direction

            if (direction.x < 0)
            {
                // Face right
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x > 0)
            {
                // Face left
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

    }
  
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
    }

    //nhan sat thuong
    public void TakenDamage(Transform player, float _dmg, bool critHit)
    {
        if (isDeath == true) return;
        if (_dmg >= CurrentHealth)
        {
            KilledByPlayer(player, critHit);
            DropItem();
            return;
        }
        CurrentHealth -=  _dmg;
        DamagePopup.Create(transform.position, -_dmg, critHit);
    }
    private void KilledByPlayer(Transform player, bool critHit)
    {
        DamagePopup.Create(transform.position, -CurrentHealth, critHit);
        CurrentHealth = 0;
        isDeath = true;
        PercentCurentHeal = CurrentHealth / localStats[Stat.MaxHealth];
        fillHeathBar.fillAmount = PercentCurentHeal;
        player.GetComponent<PlayerStatsControl>().AddExperience(localStats[Stat.ExperienceWhenDie]);
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
        UpdateState();
        UpdateAnimation();
        return;
    }
    private void DropItem()
    {
        OnDropItem?.Invoke(itemsDrop, GetComponent<Collider2D>());
    }

   

    //check player trong ban kinh
    protected Transform CheckObjectsInCircle()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, localStats[Stat.CheckObjInRadius], layerMask);

        foreach (var hitCollider in hitColliders)
        {
            return hitCollider.transform;
            
        }
        return null;
    }
    //cap nhat trang thai hien tai
    protected virtual void UpdateState()
    {

        if(CurrentHealth <= 0)
        {
            enemyState = EnemyState.Die;
            return;
        }
        if (Player != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && cooldownTimer >= localStats[Stat.AttackCooldown] && canAttack)
        {
            enemyState = EnemyState.Attack;
        }
        else if (Player != null && !canAttack && Vector2.Distance(transform.position, Player.position) <= localStats[Stat.DistanceChase])
        {
            enemyState = EnemyState.Chase;
        }
        else if(Math.Abs(rb.velocity.x) >= 0.05f)
        {
            enemyState = EnemyState.Walk;
        }
        else enemyState = EnemyState.Idle;
    }
    //Attack
    private void Attack()
    {
        cooldownTimer = 0;
        
        if (enemyAttackType == EnemyAttackType.LongAttack)
        {
            int skillPossiblePos = BaseAttack();
            if (skillPossiblePos != -1)
            {
                var attack = baseAttack[skillPossiblePos];
                attack.transform.position = new Vector2(transform.GetChild(0).position.x, transform.GetChild(0).position.y);
                attack.GetComponent<EnergyBall>().SetDirection(Mathf.Sign(Player.position.x - transform.position.x));
            }
        }
    }
    private int BaseAttack()
    {
        for (int i = 0; i < baseAttack.Length; i++)
        {
            if (!baseAttack[i].activeInHierarchy)
            {
                return i;
            }
        }
        return -1;
    }
    protected void UpdateAnimation()
    {
        if (enemyState == EnemyState.Die)
        {
            animator.SetTrigger("Die");
        }
        if (enemyState == EnemyState.Attack)
        {
            animator.SetTrigger("Attack");
            return;
        }
        if (enemyState == EnemyState.Chase)
        {
            animator.SetBool("Chase", true);
            return;
        }
        animator.SetBool("Walk", true);
    }
    //Set base attack damage
    private void SetBaseAttack()
    {
        for (int i = 0; i < baseAttack.Length; i++)
        {
            baseAttack[i].GetComponent<EnergyBall>().dmgDeal = -Damage;
        }
    }

    public void DealDamageToPlayer(Collider2D col)
    {
        if (col.gameObject.layer == 9)
        {
            var playerHealth = col.gameObject.GetComponent<Health>();
            playerHealth.DamageTaken(localStats[Stat.Damage]);
        }
    }
    //checkGround
    

    //Die 

    private void DeActive()
    {
        transform.gameObject.SetActive(false);
    }
    public void Respanw()
    {
        isDeath = false;
        transform.position = firstPos;
        CurrentHealth = localStats[Stat.MaxHealth];
        rb.bodyType = baseBodyType;
        GetComponent<Collider2D>().enabled = true;
        if (transform.TryGetComponent<BossLv1>(out BossLv1 boss))
        {
            boss.ResetSkill();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, data.statsInfo[^1].statValue);
        
    }
}

