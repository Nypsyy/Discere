using UnityEngine;

public class Minautor : MonoBehaviour
{
    public float health = 500f;

    public float meleeRage = 0f;
    public float rangedRage = 0f;
    public float magicRage = 0f;

    private Animator _spriteAnimator;
    
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private void Start() {
        _spriteAnimator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if (health <= 0f)
            _spriteAnimator.SetTrigger(IsDead);
    }
}