using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 10;
    public float destructionTime = 5f;

    private Rigidbody2D rb;
    private Vector2 heading;
    private float destructionTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        destructionTimer = destructionTime;
    }

    private void Update()
    {
        if (destructionTimer <= 0f)
        {
            Vanish();
            return;
        }
        destructionTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (heading.magnitude <= 0f) return;
        rb.velocity = heading * speed;
    }

    public void SetDirection(Vector2 dir)
    {
        if (dir.magnitude <= 0f) return;

        heading = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO : Apply damage

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")
            || collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            Vanish();
        }
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}
