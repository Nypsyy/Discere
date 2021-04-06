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
    }

    private void Start() {
        isBlinking = false;
        _hitboxForward.enabled = false;
        _hitboxBelow.enabled = false;
    }
    

    private void Update() {
        // Update the running animation
        _animator.SetFloat(AnimationVariables.Speed, Mathf.Abs(_aiPath.desiredVelocity.x));

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

        rb.isKinematic = false;
        Vector2 toTarget = GetComponentInParent<AIDestinationSetter>().target.position - transform.position;
        rb.AddForce(toTarget * dashInFactor, ForceMode2D.Impulse);
        _animator.SetBool(AnimationVariables.PrepareDash, false);
    }

    public void ShockwaveAttack() {
        Instantiate(minotaurBehavior.shockwave, transform.position, Quaternion.identity);
    }

    public void ProjectileWallAttack() {
        minotaurBehavior.SpawnBulletWall(6, 10, 70);
    }

    public void LightMeleeAttackForward() {
        _hitboxForward.enabled = true;
    }

    public void LightMeleeAttackBelow() {
        _hitboxBelow.enabled = true;
    }

    public void StopLightMeleeAttack() {
        _hitboxForward.enabled = false;
        _hitboxBelow.enabled = false;
    }

    #endregion
}