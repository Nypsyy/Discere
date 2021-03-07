using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public float velocity;
    public float damage;
    public int destructionTime;

    private bool destructing;
    private int destructionTimer;

    protected void Start()
    {
        destructing = false;
        destructionTimer = destructionTime;
    }

    protected void Update()
    {
        if (destructing)
        {
            destructionTimer--;
        }

        if (destructionTimer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            destructing = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0.25f;
            rb.velocity = Vector2.zero;
            Vector3 direction = Quaternion.AngleAxis(45, transform.forward) * new Vector2(transform.right.x, transform.right.y);
            Vector2 forceDirection = new Vector2(-1 * direction.x * 2, 1);
            Debug.Log(Quaternion.AngleAxis(45, transform.forward) * new Vector2(transform.right.x, transform.right.y));
            rb.AddForce(forceDirection, ForceMode2D.Impulse);
        }

        if (other.collider.gameObject.layer == LayerMask.NameToLayer("HeroProjectile")) {
            destructing = true;
        }
    }
}