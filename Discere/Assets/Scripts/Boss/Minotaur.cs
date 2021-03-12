using UnityEngine;

public class Minotaur : MonoBehaviour
{
    // String hashes
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public Health health; // Minotaur's health

    // Minotaur's rages
    public Rage meleeRage;  // Melee rage
    public Rage rangedRage; // Ranged rage
    public Rage magicRage;  // Magic rage

    private Animator _spriteAnimator;       // Sprite animator
    private MinotaurSprite _minotaurSprite; // Sprite manager
    private bool _isDead;

    private void Awake() {
        // Get the components
        _spriteAnimator = GetComponentInChildren<Animator>();
        _minotaurSprite = GetComponentInChildren<MinotaurSprite>();
    }

    private void Start() {
        // Boss' rages are increasing constantly
        InvokeRepeating(nameof(UpdateRage), 0, 1);
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
        meleeRage.IncreaseRage();
        rangedRage.IncreaseRage();
        magicRage.IncreaseRage();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        var dmgAmount = 0f;
        if (other.gameObject.layer != LayerMask.NameToLayer("HeroProjectile")) return;

        if (other.gameObject.CompareTag("Dagger"))
            dmgAmount = other.gameObject.GetComponent<Projectiles>().Damage;
        else if (other.gameObject.CompareTag("MagicBall"))
            dmgAmount = other.gameObject.GetComponent<MagicProjectile>().Damage;

        TakeDamage(dmgAmount);
    }

    public void TakeDamage(float damage) {
        health.TakeDamage(damage);
        StartCoroutine(_minotaurSprite.Blink());
    }
}