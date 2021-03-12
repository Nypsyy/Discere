using UnityEngine;

public static class Utils
{
    // Animation IDs
    public struct AnimationVariables
    {
        // Boss
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int LightMeleeAttack = Animator.StringToHash("Light Melee Attack");
        public static readonly int HeavyMeleeAttack = Animator.StringToHash("Heavy Melee Attack");
        public static readonly int Attacking = Animator.StringToHash("Attacking");
        // Bow
        public static readonly int ChargingShot = Animator.StringToHash("ChargingShot");
        public static readonly int FiringShot = Animator.StringToHash("FiringShot");
        // Hero
        public static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    }

    // Material IDs
    public static readonly int Progress = Shader.PropertyToID("_Progress");
    
    // Shader IDs
    public static readonly int ShaderColor = Shader.PropertyToID("_Color");
}