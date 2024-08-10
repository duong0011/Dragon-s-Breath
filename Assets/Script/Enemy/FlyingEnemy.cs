using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    protected FlyingEnemyAI AI;

    public bool isUpdatedTarget = false;
    private float tmpStopDistance;
    protected override void Awake()
    {
        base.Awake();
        AI = transform.GetComponent<FlyingEnemyAI>();
        tmpStopDistance = AI.stopDistance;
    }
    private void Update()
    {
        if (isDeath == true)
        {
            UpdateTaget();
            return;
        }
        CallPerFrame();

        AI.stopDistance = AI.target != Player ? 2f : tmpStopDistance;

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
            else if (AI.target == Player || isUpdatedTarget == false)
            {
                enemyState = EnemyState.Walk;
                isUpdatedTarget = true;
                UpdateTaget();
            }
        }
        if (Vector2.Distance(transform.position, AI.target.position) <= AI.stopDistance)
        {
            isUpdatedTarget = false;
        }

        UpdateAnimation();
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        FlipHeathBar();
        LateCallPerFrame();
    }
    private void UpdateTaget()
    {
        int randomX = Random.Range(-6, 6);
        int randomY = Random.Range(0, 6);
        spawnPoint.position = new Vector3(firstPos.x + randomX, firstPos.y + randomY, 0);
        AI.target = spawnPoint;
    }

}
