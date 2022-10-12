using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Pathfinding;
using System;

public class EnemyAI : Enemy
{
    // Field for Pathfinding
    [Header("Pathfinding Field")]
    public Transform target;
    public float speed;
    public Vector2 velocity;
    public float nextWaypointDistance;

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;

    // Ignore Bridgeblocking
    public BoxCollider2D enemyCollider;
    public TilemapCollider2D bridgeColliders;

    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
        bridgeColliders = GameObject.Find("BridgeBlocking").GetComponent<TilemapCollider2D>();
        target = GameObject.Find("FarmCenter").transform;

        // Check if collide with bridge
        Physics2D.IgnoreCollision(enemyCollider, bridgeColliders);

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.transform.position, target.transform.position, OnPathCompleted);
    }
    private void OnPathCompleted(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (path == null)
            return; // if no path generated then retrun.

        if (currentWaypoint >= path.vectorPath.Count) // check if we're in the end of the path.
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; // Make enemy move.
        Vector2 force = speed * Time.deltaTime * direction;
        velocity = rb.velocity;

        // Slowed down or being pushed
        UpdateMotorEffect(force);
        rb.AddForce(force);

        float distance = Vector2.Distance(path.vectorPath[currentWaypoint], rb.position);

        if (distance < nextWaypointDistance) // If distance between current position and target waypoint small enough to move on to next waypoint.
        {
            currentWaypoint++;
        }

        // Flip enemy sprite base on velocity on x direction.
        if (rb.velocity.x >= 0.01f)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (rb.velocity.x <= -0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        // Run animation
        if (Mathf.Abs(rb.velocity.x) >= 0.01 || Mathf.Abs(rb.velocity.y) >= 0.01)
            enemyAnimator.SetFloat("speed", 1);
        else if (Mathf.Abs(rb.velocity.x) < 0.01 || Mathf.Abs(rb.velocity.y) < 0.01)
            enemyAnimator.SetFloat("speed", 0);
    }


    protected void UpdateMotorEffect(Vector2 force)
    {

        // Push enemy
        if (pushDirection != Vector3.zero)
        {
            rb.AddForce(pushDirection, ForceMode2D.Impulse);
            pushDirection = Vector3.zero;
        }

        // Slow down enemy
        if (slowDown && timeBeingSlowed == 0f)
        {
            timeBeingSlowed = Time.time;
        }

        if (slowDown && Time.time - timeBeingSlowed <= slowDownTime)
        {
            force *= slowAmount;
        }
        else
        {
            slowDown = false;
            slowAmount = 0f;
            timeBeingSlowed = 0f;
        }
    }
}
