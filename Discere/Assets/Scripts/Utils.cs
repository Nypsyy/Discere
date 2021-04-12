using UnityEngine;

public static class Utils
{
    public enum HeroMode
    {
        Move = 0,
        Slash = 1,
        Jump = 2,
        BigSlash = 3,
        Aim = 4
    }

    public struct Sounds
    {
        public const string Jump = "Jump";
        public const string Slash = "Slash";
        public const string HeavySlash = "HeavySlash";
        public const string Land = "Land";
        public const string Throw = "Throw";
        public const string CastSpell = "CastSpell";
        public const string LaserBuildup = "LaserBuildup";
        public const string LaserShoot = "LaserShoot";
        public const string Dash = "Dash";
        public const string MinotaurHurt = "MinotaurHurt";
        public const string HeroHurt = "HeroHurt";
        public const string BowShoot = "BowShoot";
        public const string BowCharge = "BowCharge";
        public const string BowChange = "BowChange";
        public const string RockFall = "RockFall";
        public const string BossShockwave = "BossWave";
        public const string BossAttack = "BossAttack";
        public const string BossAttackHeavy = "BossAttackHeavy";
        public const string BossDash = "BossDash";
        public const string FamilierSpawn = "FamilierSpawn";
    }

    public struct Inputs
    {
        public const string GameplayMap = "Gameplay";
        public const string UIMap = "UI";
        public const string MoveHorizontal = "Move Horizontal";
        public const string MoveVertical = "Move Vertical";
        public const string AimHorizontal = "Aim Horizontal";
        public const string AimVertical = "Aim Vertical";
        public const string AttackStyle = "Attack Style";
        public const string Jump = "Jump";
        public const string Dash = "Dash";
        public const string LightAttack = "Light Attack";
        public const string HeavyAttack = "Heavy Attack";
        public const string UICancel = "UI Cancel";
    }

    public struct Damages
    {
        // Boss
        public const float BossLightMelee = 5f;
    }

    // Animation IDs
    public struct AnimStrings
    {
        // Boss
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int InvokeAttack = Animator.StringToHash("Invoke Attack");
        public static readonly int LightMeleeAttack = Animator.StringToHash("Light Melee Attack");
        public static readonly int ProjectileWallAttack = Animator.StringToHash("Projectile Wall Attack");
        public static readonly int ShockwaveAttack = Animator.StringToHash("Shockwave Attack");
        public static readonly int BossAttacking = Animator.StringToHash("Boss Attacking");
        public static readonly int BossDash = Animator.StringToHash("Dash");
        public static readonly int BossRockFallAttack = Animator.StringToHash("Rock Fall Attack");
        public static readonly int IsDead = Animator.StringToHash("IsDead");

        public static readonly int PrepareDash = Animator.StringToHash("Prepare Dash");

        // Bow
        public static readonly int ChargingShot = Animator.StringToHash("ChargingShot");

        public static readonly int FiringShot = Animator.StringToHash("FiringShot");

        // Hero
        public static readonly int HeroAttacking = Animator.StringToHash("Is Attacking");
        public static readonly int HeroFire = Animator.StringToHash("Fire");
        public static readonly int SwitchMode = Animator.StringToHash("Switch Mode");
        public static readonly int Mode = Animator.StringToHash("Mode");
        public static readonly int SlashDirection = Animator.StringToHash("Slash Direction");
        public static readonly int Direction = Animator.StringToHash("Direction(Up:0,Clockwise)");
    }

    // Material IDs
    public static readonly int Progress = Shader.PropertyToID("_Progress");

    // Shader IDs
    public static readonly int ShaderColor = Shader.PropertyToID("_Color");
}