using UnityEngine;

public static class Utils
{
    // Animation IDs
    public struct AnimationVariables
    {
        // Boss
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int LightMeleeAttack = Animator.StringToHash("Light Melee Attack");
        public static readonly int ProjectileWallAttack = Animator.StringToHash("Projectile Wall Attack");
        public static readonly int HeavyMeleeAttack = Animator.StringToHash("Heavy Melee Attack");
        public static readonly int BossAttacking = Animator.StringToHash("Boss Attacking");
        public static readonly int BossDash = Animator.StringToHash("Dash");
        public static readonly int PrepareDash = Animator.StringToHash("Prepare Dash");
        // Bow
        public static readonly int ChargingShot = Animator.StringToHash("ChargingShot");
        public static readonly int FiringShot = Animator.StringToHash("FiringShot");
        // Hero
        public static readonly int HeroAttacking = Animator.StringToHash("IsAttacking");
    }

    // Material IDs
    public static readonly int Progress = Shader.PropertyToID("_Progress");
    
    // Shader IDs
    public static readonly int ShaderColor = Shader.PropertyToID("_Color");
}