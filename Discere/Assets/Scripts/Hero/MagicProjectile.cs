using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 10;
    public float destructionTime = 5f;
    public float autoSeekRadius = 2f;
    public float autoSeekStrength = 4f;
    public LayerMask autoSeekLayer;

    private Rigidbody2D rb;
    private Vector2 heading;
    private float destructionTimer;
    private Animator anim;
    private bool isDestroying = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        destructionTimer = destructionTime;
    }

    private void Update()
    {
        if (isDestroying) return;

        // Destruction timer
        if (destructionTimer <= 0f)
        {
            Vanish();
            return;
        }
        destructionTimer -= Time.deltaTime;

        // Auto Seek system
        Collider2D target = Physics2D.OverlapCircle(transform.position, autoSeekRadius, autoSeekLayer);
        if (target != null)
        {
            heading = Vector2.MoveTowards(heading, (target.transform.position - transform.position).normalized, autoSeekStrength * Time.deltaTime).normalized;
        }
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
        rb.velocity = Vector2.zero;
        heading = Vector2.zero;
        isDestroying = true;
        anim.SetTrigger("Destroy");
        //Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, autoSeekRadius);
    }
}
