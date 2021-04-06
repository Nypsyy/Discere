using UnityEngine;
using System.Collections;

public class Minotaur : MonoBehaviour
{
    // String hashes
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public Health health; // Minotaur's health
    public GameObject hero;
    
    public Familier familierModel;
    public Bullet bulletModel;
    public Rock rockModel;

    // Minotaur's rages
    public Rage meleeRage;  // Melee rage
    public Rage rangedRage; // Ranged rage
    public Rage magicRage;  // Magic rage

    private LightMeleeAttackAction _lightMeleeAttackAction;
    private Animator _spriteAnimator;       // Sprite animator
    private MinotaurSprite _minotaurSprite; // Sprite manager
    private Rigidbody2D _rigidbody;
    private new AudioManager audio;
    private bool _isDead;
    private bool _isDashing;

    private void Awake() {
        // Get the components
        _spriteAnimator = GetComponentInChildren<Animator>();
        _minotaurSprite = GetComponentInChildren<MinotaurSprite>();
        _lightMeleeAttackAction = GetComponentInChildren<LightMeleeAttackAction>();
        audio = FindObjectOfType<AudioManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _isDashing = false;
    }

    private void Start() {
        // Boss' rages are increasing constantly
        InvokeRepeating(nameof(UpdateRage), 0, 1);
        StartCoroutine(_SpawnRocks(30, 0.2f));
        // For testing BulletWall only: StartCoroutine(_test_BulletWall());
    }

    private void Update() {
        // If the boss is dead then do nothing
        if (_isDead) return;

        // If the boss' health reaches 0
        if (health.value <= 0f) {
            _spriteAnimator.SetTrigger(IsDead); // Trigger death animation
            _isDead = true;
        }
        
        _isDashing = _rigidbody.velocity.sqrMagnitude > 10;
    }

    // Increases the boss' rages
    public void UpdateRage() {
        meleeRage.IncreaseRage(0.5f);
        rangedRage.IncreaseRage(0.5f);
        magicRage.IncreaseRage(0.5f);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        HandleCollidingObject(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        HandleCollidingObject(other.gameObject);
    }

    private void HandleCollidingObject(GameObject gameObject) {
        Rock rock = gameObject.GetComponent<Rock>();
        if (rock) {
            if (_isDashing)
                Destroy(gameObject);
            return;
        }
    
        if (gameObject.layer != LayerMask.NameToLayer("HeroProjectile")) return;

        Projectile proj = gameObject.GetComponent<Projectiles>()?.projectile
                          ?? gameObject.GetComponent<MagicProjectile>()?.projectile;

        if (proj == null) return;

        _lightMeleeAttackAction.Cost += 0.05f;

        TakeDamage(proj.damage, proj.style);
    }

    public void TakeDamage(float damage, FightingStyle.Style style) {
        if (_minotaurSprite.isBlinking)
            return; // do not apply damage when blinking = invulnerability time

        health.TakeDamage(damage);
        audio.Play("MinautorHurt");

        switch (style) {
            case FightingStyle.Style.Melee:
                meleeRage.IncreaseRage(damage);
                break;

            case FightingStyle.Style.Range:
                rangedRage.IncreaseRage(damage);
                break;

            case FightingStyle.Style.Magic:
                magicRage.IncreaseRage(damage);
                break;
        }

        StartCoroutine(_minotaurSprite.Blink());
    }
    
    private IEnumerator _SpawnFamiliers(int n, float delay_between, float bullet_delay, float lifetime) {
        for (int i = 0; i < n; ++i) {
            familierModel.Create(hero, transform.position, bullet_delay, lifetime);
            yield return new WaitForSeconds(delay_between);
        }
        yield return null;
    }
    
    // angle_width & angle_offset are in degrees
    private void _SpawnBulletWall(int nb_bullets, float speed, float angle_width, float angle_offset = 0) {
        angle_width *= Mathf.Deg2Rad;
        angle_offset *= Mathf.Deg2Rad;
        // bullets are launched in [-angle_width/2, +angle_width/2] in direction of the player
        Vector2 dir = hero.transform.position - transform.position;
        float base_angle = Mathf.Atan2(dir.y, dir.x) + angle_offset - angle_width / 2;
        for (int i = 0; i < nb_bullets; ++i) {
            float angle = base_angle + i * angle_width / (nb_bullets-1);
            bulletModel.Create(transform.position, 0.6f, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), speed);
        }
    }
    
    private IEnumerator _SpawnRocks(int n, float delay_between) {
        for (int i = 0; i < n; ++i) {
            rockModel.Create((Vector2)hero.transform.position + 15 * Random.insideUnitCircle);
            yield return new WaitForSeconds(delay_between);
        }
        yield return null;
    }
    
    private IEnumerator _test_BulletWall() {
        yield return new WaitForSeconds(3);
        for (float t = 0; t < 20; t += 0.5f) {
            _SpawnBulletWall(15, 3, 60, 10*Mathf.Sin(t));
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < 20; ++i) {
            _SpawnBulletWall(30, 10, 90);
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(3f);
        for (float a = 0; a < 360*5; a += 30) {
            _SpawnBulletWall(60, 6, 120, a);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
