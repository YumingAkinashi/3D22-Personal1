using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Movement
    public float playerSpeed = 10f;

    // Monitored stats in inspector
    public Vector2 velocity;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Animator playerAnimator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 force = direction * playerSpeed * Time.fixedDeltaTime;
        rb.AddForce(force);
        velocity = rb.velocity;

        // Flip player sprite
        if (rb.velocity.x >= 0.01f)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (rb.velocity.x <= -0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        // Set running animation
        if (Mathf.Abs(rb.velocity.x) >= 0.1 || Mathf.Abs(rb.velocity.y) >= 0.1)
            playerAnimator.SetFloat("speed", 1);
        else if(Mathf.Abs(rb.velocity.x) < 0.1 || Mathf.Abs(rb.velocity.y) < 0.1)
            playerAnimator.SetFloat("speed", 0);
    }
}
