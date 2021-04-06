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
    public GameObject shockwave;

    // Minotaur's rages
    public Rage meleeRage;    // Melee rage
    public Rage distanceRage; // Ranged rage
    public Rage magicRage;    // Magic rage

    private LightMeleeAttackAction _lightMeleeAttackAction;
    private Animator _spriteAnimator;       // Sprite animator
    private MinotaurSprite _minotaurSprite; // Sprite manager
    private new AudioManager audio;
    private bool _isDead;

    private void Awake() {
        // Get the components
        _spriteAnimator = GetComponentInChildren<Animator>();
        _minotaurSprite = GetComponentInChildren<MinotaurSprite>();
        _lightMeleeAttackAction = GetComponentInChildren<LightMeleeAttackAction>();
        audio = FindObjectOfType<AudioManager>();
    }

    private void Start() {
        // Boss' rages are increasing constantly
        InvokeRepeating(nameof(UpdateRage), 0, 1);

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
    }

    // Increases the boss' rages
    public void UpdateRage() {
        meleeRage.IncreaseRage(0.5f);
        distanceRage.IncreaseRage(0.5f);
        magicRage.IncreaseRage(0.5f);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        HandleCollidingObject(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        HandleCollidingObject(other.gameObject);
    }

    private void HandleCollidingObject(GameObject gameObject) {
        if (gameObject.layer != LayerMask.NameToLayer("HeroProjectile")) return;

        Projectile proj = gameObject.GetComponent<Projectiles>()?.projectile
                          ?? gameObject.GetComponent<MagicProjectile>()?.projectile;

        if (proj == null) return;

        _lightMeleeAttackAction.Cost += 0.05f;

        TakeDamage(proj.damage, proj.style);
    }

    public void TakeDamage(float damage, FightingStyle.Style style) {
        GetComponentInChildren<ShockwaveAttackCost>().Touched();

        if (_minotaurSprite.isBlinking)
            return; // do not apply damage when blinking = invulnerability time

        health.TakeDamage(damage);
        audio.Play("MinautorHurt");

        switch (style) {
            case FightingStyle.Style.Melee:
                meleeRage.IncreaseRage(damage);
                break;

            case FightingStyle.Style.Range:
                distanceRage.IncreaseRage(damage);
                break;

            case FightingStyle.Style.Magic:
                magicRage.IncreaseRage(damage);
                break;
        }

        StartCoroutine(_minotaurSprite.Blink());
    }

    public IEnumerator SpawnFamiliers(int n, float delay) {
        for (var i = 0; i < n; i++) {
            Instantiate(familierModel, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }

        yield return null;
    }

    // angle_width & angle_offset are in degrees
    public void SpawnBulletWall(int nb_bullets, float speed, float angle_width, float angle_offset = 0, float scaling = 0f) {
        angle_width *= Mathf.Deg2Rad;
        angle_offset *= Mathf.Deg2Rad;

        // bullets are launched in [-angle_width/2, +angle_width/2] in direction of the player
        var dir = hero.transform.position - transform.position;
        var base_angle = Mathf.Atan2(dir.y, dir.x) + angle_offset - angle_width / 2;
        for (var i = 0; i < nb_bullets; ++i) {
            var angle = base_angle + i * angle_width / (nb_bullets - 1);
            Instantiate(bulletModel, transform.position,
                        Quaternion.FromToRotation(Vector3.right,
                                                  dir.normalized + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized));
        }
    }
}