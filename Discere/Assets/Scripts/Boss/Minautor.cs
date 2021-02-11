using UnityEngine;

public class Minautor : MonoBehaviour
{
    // String hashes
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public Health health; // Minautor's health

    // Minautor's rages
    public Rage meleeRage;  // Melee rage
    public Rage rangedRage; // Ranged rage
    public Rage magicRage;  // Magic rage

    private Animator _spriteAnimator; // Sprite animator
    private bool _isDead;

    private void Start() {
        // Get the components
        _spriteAnimator = GetComponentInChildren<Animator>();

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
            return;
        }

        // Debug.Log(meleeRage.value + "\t" + rangedRage.value + "\t" + magicRage.value);
    }

    // Increases the boss' rages
    public void UpdateRage() {
        meleeRage.IncreaseRage();
        rangedRage.IncreaseRage();
        magicRage.IncreaseRage();
    }
}