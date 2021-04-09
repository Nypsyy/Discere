using System.Collections;
using UnityEngine;
using Pathfinding;
using static Utils;

// Responsible for displaying the Minautor / changing its aspect
[RequireComponent(typeof(SpriteRenderer))]
public class MinotaurSprite : MonoBehaviour
{
    public float dashInFactor;
    public bool isBlinking { get; private set; }


    private Minotaur minotaurBehavior;
    private Animator _animator;                       // Sprite animator
    private SpriteRenderer _sprite;                   // Sprite renderer
    private AIPath _aiPath;                           // Pathfinding script
    private AIDestinationSetter _aiDestinationSetter; // Pathfinding script
    private CircleCollider2D _hitboxForward;
    private CapsuleCollider2D _hitboxBelow;
    private AudioManager _audio;

    private bool _isLookingRight = true; // Looking state of the sprite

    private void Awake() {
        // Get the components
        minotaurBehavior = GetComponentInParent<Minotaur>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _aiPath = GetComponentInParent<AIPath>();
        _aiDestinationSetter = GetComponentInParent<AIDestinationSetter>();
        _hitboxForward = GetComponentInChildren<CircleCollider2D>();
        _hitboxBelow = GetComponentInChildren<CapsuleCollider2D>();
        _audio = FindObjectOfType<AudioManager>();
    }

    private void Start() {
        isBlinking = false;
        _hitboxForward.enabled = false;
        _hitboxBelow.enabled = false;
    }


    private void Update() {
        // Update the running animation
        _animator.SetFloat(AnimStrings.Speed, Mathf.Abs(_aiPath.desiredVelocity.x));

        Flip(); // Flip sprite
    }

    // Changes the sprite rendering direction
    private void Flip() {
        // If the boss' target is on the left and the boss is looking right
        if (_aiDestinationSetter.target.position.x < transform.position.x && _isLookingRight) {
            transform.parent.localScale =
                new Vector3(transform.parent.localScale.x * -1, transform.parent.localScale.y, transform.parent.localScale.z);
            _isLookingRight = false;
        }
        // If the boss' target is on the right and the boss is looking left
        else if (_aiDestinationSetter.target.position.x > transform.position.x && !_isLookingRight) {
            transform.parent.localScale =
                new Vector3(transform.parent.localScale.x * -1, transform.parent.localScale.y, transform.parent.localScale.z);
            _isLookingRight = true;
        }


        // FLip based on the boolean
        //_sprite.flipX = !_isLookingRight;
    }

    public IEnumerator Blink() {
        isBlinking = true;
        for (var i = 0; i < 3; ++i) {
            _sprite.material.SetColor(ShaderColor, Color.white);
            yield return new WaitForSeconds(0.1f);
            _sprite.material.SetColor(ShaderColor, Color.clear);
            yield return new WaitForSeconds(0.1f);
        }

        isBlinking = false;
    }

    #region ANIMATION METHODS

    public void InvokeAttack() {
        var familierNb = 1 + Mathf.Max(minotaurBehavior.distanceRage.FillRatio, minotaurBehavior.magicRage.FillRatio) / 34;
        StartCoroutine(minotaurBehavior.SpawnFamiliers(familierNb, 1f));
    }

    public void DashIn() {
        var rb = GetComponentInParent<Rigidbody2D>();
        _audio.Play("BossDash");
        rb.isKinematic = false;
        Vector2 toTarget = GetComponentInParent<AIDestinationSetter>().target.position - transform.position;
        rb.AddForce(toTarget * dashInFactor, ForceMode2D.Impulse);
        _animator.SetBool(AnimStrings.PrepareDash, false);
    }

    public void ShockwaveAttack() {
        CinemachineEffects.Instance.Shake(3f, 0.3f);
        _audio.Play("BossWave");
        Instantiate(minotaurBehavior.shockwave, transform.position, Quaternion.identity);
    }

    public void ProjectileWallAttack() {
        FreezeFrame.Instance.Freeze(0.07f);
        var progress = Mathf.Max(1 - minotaurBehavior.magicRage.progress, 1 - minotaurBehavior.distanceRage.progress);
        var angleWidth = 45 + 135 * progress;
        var nbBullets = (int) angleWidth / 5;
        var speed = 5 + 15 * progress;
        
        minotaurBehavior.SpawnBulletWall(nbBullets, speed, angleWidth);
    }

    public void LightMeleeAttackForward() {
        _audio.Play("BossAttack");
        _hitboxForward.enabled = true;
    }

    public void LightMeleeAttackBelow() {
        _audio.Play("BossAttackHeavy");
        _hitboxBelow.enabled = true;
    }

    public void StopLightMeleeAttack() {
        _hitboxForward.enabled = false;
        _hitboxBelow.enabled = false;
    }

    #endregion
}