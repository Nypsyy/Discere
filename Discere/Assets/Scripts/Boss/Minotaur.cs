using UnityEngine;
using System.Collections;

public class Minotaur : MonoBehaviour
{
    // String hashes
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public Health health; // Minotaur's health
    public GameObject hero;
    public Familier familierModel;

    // Minotaur's rages
    public Rage meleeRage;  // Melee rage
    public Rage rangedRage; // Ranged rage
    public Rage magicRage;  // Magic rage

    private LightMeleeAttackAction _lightMeleeAttackAction;
    private Animator _spriteAnimator;       // Sprite animator
    private MinotaurSprite _minotaurSprite; // Sprite manager
    private bool _isDead;

    private void Awake() {
        // Get the components
        _spriteAnimator = GetComponentInChildren<Animator>();
        _minotaurSprite = GetComponentInChildren<MinotaurSprite>();
        _lightMeleeAttackAction = GetComponentInChildren<LightMeleeAttackAction>();
    }

    private void Start() {
        // Boss' rages are increasing constantly
        InvokeRepeating(nameof(UpdateRage), 0, 1);
        
        StartCoroutine(_SpawnFamiliers(5, 1, 2, 8));
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
}
