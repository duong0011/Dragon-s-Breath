using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    [SerializeField] float speed;
    public float dmgDeal;
    [SerializeField] private Transform caster;

    protected Animator ani;
    protected CircleCollider2D circleColider;
    

    protected bool hit;
    protected float direction;
    protected float lifeTime;
    private string casterName;
    private bool isGotPlayerPos = false;
    private Vector3 directionToPlayer;

    protected virtual void Awake()
    {
        ani = GetComponent<Animator>();
        circleColider = GetComponent<CircleCollider2D>();
        dmgDeal *= -1;
        casterName = LayerMask.LayerToName(caster.gameObject.layer);
    }

    protected virtual void Update()
    {
        if (hit) return;
        string casterName = LayerMask.LayerToName(caster.gameObject.layer);
        if(casterName != "Enemy"){
            float movementSpeed = speed * Time.deltaTime * direction;
            transform.Translate(movementSpeed, 0, 0);
        }
        else
        {
            if(!isGotPlayerPos)
            {
                Vector3 castPoint = caster.transform.GetChild(0).position;
                Vector3 playerPos = caster.GetComponent<Enemy>().Player.position;
                directionToPlayer = -(castPoint - playerPos).normalized;
                isGotPlayerPos = true;
            }
            float xMovement = directionToPlayer.x * speed* Time.deltaTime;
            float yMovement = directionToPlayer.y * speed * Time.deltaTime;
            transform.position += new Vector3(xMovement, yMovement, 0);
            
        }
        lifeTime += Time.deltaTime;
        if(lifeTime >= 2)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == 10 && caster.gameObject.layer == 9)
        {
            hit = true;
            circleColider.enabled = false;
            bool isCritHit = Random.Range(0, 100) <= 40;
            caster.GetComponent<PlayerAttackState>().DealDamageToEnemy(collision.GetComponent<Transform>(), dmgDeal, isCritHit);
            ani.SetTrigger("explode");
        }
        if (collision.gameObject.layer == 9 && caster.gameObject.layer == 10)
        {
            hit = true;
            circleColider.enabled = false;
            collision.GetComponent<Health>().DamageTaken(dmgDeal);
            ani.SetTrigger("explode");
        }

    }
    public void SetDirection(float _direction)
    {
        gameObject.SetActive(true);
        direction = _direction;
        circleColider.enabled = true;
        hit = false;
        lifeTime = 0;
        isGotPlayerPos = false;

        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != _direction && casterName != "Enemy")
        {
            localScaleX *= -1;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

    }

    protected virtual void DeActivate()
    {
        gameObject.SetActive(false);
    }
 
}
