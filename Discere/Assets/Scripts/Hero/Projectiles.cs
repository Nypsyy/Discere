using System;
using System.Collections;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public ProjectileData projectileData;

    public float Damage => projectileData.damage;
    public float Velocity => projectileData.velocity;

    private float DestructionTimer => projectileData.destructionTime;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start() {
        _spriteRenderer.sprite = projectileData.sprite;

        var direction = FindObjectOfType<Hero>().GetComponent<Hero>().ShootingDirection;
        transform.right = direction;

        if (_spriteRenderer.sprite.name == "Dagger")
            transform.Rotate(0.0f, 0.0f, -45.0f);

        _rb.velocity = direction * Velocity;

        StartCoroutine(Destroying(3f));
    }

    private void OnCollisionEnter2D(Collision2D other) {
        GameObject gameObject = other.collider.gameObject;
        
        Rock rock = gameObject.GetComponent<Rock>();
        if (projectileData.projectileName == "Strong Arrow")
            gameObject.GetComponent<Rock>()?.DestroyMe();
        
        if (gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            StartCoroutine(Destroying(DestructionTimer));

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0.25f;
            rb.velocity = Vector2.zero;
            Vector3 direction = Quaternion.AngleAxis(45, transform.forward) * new Vector2(transform.right.x, transform.right.y);
            Vector2 forceDirection = new Vector2(-1 * direction.x * 2, 1);

            rb.AddForce(forceDirection, ForceMode2D.Impulse);
        }

        else if (gameObject.layer == LayerMask.NameToLayer("HeroProjectile")) {
            StartCoroutine(Destroying(DestructionTimer));
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            StartCoroutine(Destroying(DestructionTimer));
        }
    }

    private IEnumerator Destroying(float timer) {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
        yield return null;
    }
}
