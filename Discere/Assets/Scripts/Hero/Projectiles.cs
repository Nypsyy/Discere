using System;
using System.Collections;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public Projectile projectile;

    public float Damage => projectile.damage;
    public float Velocity => projectile.velocity;

    private float DestructionTimer => projectile.destructionTime;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start() {
        _spriteRenderer.sprite = projectile.sprite;

        var direction = FindObjectOfType<Hero>().GetComponent<Hero>().ShootingDirection;
        transform.right = direction;

        if (_spriteRenderer.sprite.name == "Dagger")
            transform.Rotate(0.0f, 0.0f, -45.0f);

        _rb.velocity = direction * Velocity;

        StartCoroutine(Destroying(15f));
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            StartCoroutine(Destroying(DestructionTimer));

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0.25f;
            rb.velocity = Vector2.zero;
            Vector3 direction = Quaternion.AngleAxis(45, transform.forward) * new Vector2(transform.right.x, transform.right.y);
            Vector2 forceDirection = new Vector2(-1 * direction.x * 2, 1);

            rb.AddForce(forceDirection, ForceMode2D.Impulse);
        }

        if (other.collider.gameObject.layer == LayerMask.NameToLayer("HeroProjectile")) {
            StartCoroutine(Destroying(DestructionTimer));
        }
    }

    private IEnumerator Destroying(float timer) {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
        yield return null;
    }
}