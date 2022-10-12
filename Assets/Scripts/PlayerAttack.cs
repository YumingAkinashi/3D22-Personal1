using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    // Attack point
    [Header("Attack Point")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // Attack struct
    [Header("Attack Struct")]
    public int damage = 1;
    public float pushForce = 2;
    private float cooldown = 0.5f;
    private float lastAttack;

    // Claw FX
    public GameObject clawPrefab;
    private Animator clawFX;

    // Attack Animator
    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        InvokeRepeating("CleanClawFX", 0f, 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e"))
        {
            if (Time.time - lastAttack > cooldown)
            {
                lastAttack = Time.time;
                playerAnimator.SetTrigger("attack");

                if(transform.localScale.x == 1)
                {
                    GameObject claw = Instantiate(clawPrefab, attackPoint.position, Quaternion.identity);
                    clawFX = claw.GetComponent<Animator>();
                    clawFX.SetTrigger("right");
                }
                else if(transform.localScale.x == -1)
                {
                    GameObject claw = Instantiate(clawPrefab, attackPoint.position, Quaternion.identity);
                    clawFX = claw.GetComponent<Animator>();
                    clawFX.SetTrigger("left");
                }

                Damage dmg = new Damage
                {
                    damageAmount = damage,
                    origin = transform.position, // position of which having the dmg object
                    pushForce = pushForce,

                };

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                foreach (Collider2D enemy in hitEnemies)
                {
                        enemy.SendMessage("ReceiveDamage", dmg);
                        Debug.Log("hit " + enemy.name);
                }
            }
        }
    }

    private void CleanClawFX()
    {
        foreach (GameObject clone in GameObject.FindGameObjectsWithTag("FX"))
        {
            if (clone.name == "Claw(Clone)")
            {
                Destroy(clone);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
