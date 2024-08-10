using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;


public class FlyingEnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;
    public float stopDistance;


    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
 

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;


    Seeker seeker;
    Rigidbody2D rb;

    public void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        CallUpdatePath();

    }

    private void CallUpdatePath()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {

        followEnabled = Vector2.Distance(transform.position, target.position) >= stopDistance;
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }

    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        //Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }


        //Direction Calculation
        // Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 newPosition = Vector2.MoveTowards(transform.position, (Vector2)path.vectorPath[currentWaypoint], speed * Time.deltaTime);
        rb.MovePosition(newPosition);
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        //NextWaypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //Direction Graphics Handling
        
        if (directionLookEnabled)
        {
            if (direction.x > 0.01f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < -0.01f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}