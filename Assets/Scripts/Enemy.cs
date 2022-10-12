using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Components
    protected Animator enemyAnimator;

    // Stats
    [Header("Stats")]
    public int hitpoint;
    public int maxHitpoint;
    public int enemyScore;
    private bool isDead;

    // Immunity
    [Header("Immunity")]
    public float immuneTime = 0.5f;
    protected float lastImmune;

    // Push
    [Header("Push")]
    protected Vector3 pushDirection;

    // Slowdown
    [Header("Slowdown")]
    public float slowAmount;
    public float slowDownTime;
    public bool slowDown = false;
    protected float timeBeingSlowed;

    protected virtual void Start()
    {
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    // All enemies can receive damage or die
    public void ReceiveDamage(Damage dmg)
    {

        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            slowDown = true;

            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            }
            else
            {
                enemyAnimator.SetTrigger("damaged");
                Debug.Log("damaged");
            }
        }

    }
    public void Death()
    {
        isDead = true;
        ScoreManager.instance.score += enemyScore;
        enemyAnimator.SetTrigger("died");
        Destroy(gameObject, 0.5f);
    }
    public void TouchedFarm()
    {
        if (!isDead)
        {
            enemyAnimator.SetTrigger("touchedFarm");
            ScoreManager.instance.FarmInvaded();
            Destroy(gameObject, 1f);
        }
    }
}
