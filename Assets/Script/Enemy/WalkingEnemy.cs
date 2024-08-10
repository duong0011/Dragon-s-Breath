using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{

    protected WalkingEnemyAI AI;

    private bool isUpdatedTarget = false;
    protected override void Awake()
    {
        base.Awake();
        AI = transform.GetComponent<WalkingEnemyAI>();
    }
    void Update()
    {
        if (isDeath == true)
        {
            UpdateTaget();
            return;
        }
        CallPerFrame();

        if (Player == null)
        {
            if (Vector2.Distance(transform.position, AI.target.position) <= AI.stopDistance || isJustBack == true)
            {
                UpdateTaget();
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, Player.position) <= localStats[Stat.DistanceChase])
            {
           
                AI.target = Player;
            }
            else if(AI.target == Player || isUpdatedTarget == false) 
            {
                enemyState = EnemyState.Walk;
                isUpdatedTarget = true;
                UpdateTaget();
            }
        }
        //neu taget la player va nam ngoai tam danh cap nhat lai targer
        if(Vector2.Distance(transform.position, AI.target.position) <= AI.stopDistance)
        {
            isUpdatedTarget = false;
        }
        UpdateAnimation();
    }
    private void LateUpdate()
    {
       
        FlipHeathBar();
        LateCallPerFrame();

    }
    private void UpdateTaget()
    {
        int randomPoint = Random.Range(-6, 6);
        spawnPoint.position = new Vector3(firstPos.x + randomPoint, firstPos.y, 0);
        AI.target = spawnPoint;
    }
   
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
   

}
