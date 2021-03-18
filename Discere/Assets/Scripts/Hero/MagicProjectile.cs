using System.Collections;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public Projectile projectile;

    public float Damage => projectile.damage;
    public float Velocity => projectile.velocity;
    public float DestructionTime => projectile.destructionTime;

    public float autoSeekRadius = 2f;
    public float autoSeekStrength = 4f;
    public LayerMask autoSeekLayer;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _heading;
    private Animator _anim;


    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        _spriteRenderer.sprite = projectile.sprite;

        var direction = FindObjectOfType<Hero>().GetComponent<Hero>().ShootingDirection;
        SetDirection(direction);

        _anim = GetComponent<Animator>();
        StartCoroutine(Destroying());
    }

    private void Update() {
        // Auto Seek system
        Collider2D target = Physics2D.OverlapCircle(transform.position, autoSeekRadius, autoSeekLayer);
        if (target != null) {
            _heading = Vector2.MoveTowards(_heading, (target.transform.position - transform.position).normalized,
                                           autoSeekStrength * Time.deltaTime).normalized;
        }
    }

    private void FixedUpdate() {
        if (_heading.magnitude <= 0f) return;
        _rb.velocity = _heading * Velocity;
    }

    public void SetDirection(Vector2 dir) {
        if (dir.magnitude <= 0f) return;

        _heading = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacle") &&
            collision.gameObject.layer != LayerMask.NameToLayer("Boss")) return;
        StopCoroutine(Destroying());
        Vanish();
    }

    private void Vanish() {
        _rb.velocity = Vector2.zero;
        _heading = Vector2.zero;
        _anim.SetTrigger("Destroy");
    }

    private IEnumerator Destroying() {
        yield return new WaitForSeconds(DestructionTime);
        Destroy(gameObject);
        yield return null;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, autoSeekRadius);
    }
}
