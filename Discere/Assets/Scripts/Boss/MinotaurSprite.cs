using System.Collections;
using UnityEngine;
using Pathfinding;
using static Utils;

// Responsible for displaying the Minautor / changing its aspect
[RequireComponent(typeof(SpriteRenderer))]
public class MinotaurSprite : MonoBehaviour
{
    
    public bool isBlinking { get; private set; }
    
    private Animator _animator;                       // Sprite animator
    private SpriteRenderer _sprite;                   // Sprite renderer
    private AIPath _aiPath;                           // Pathfinding script
    private AIDestinationSetter _aiDestinationSetter; // Pathfinding script
    private bool _isLookingRight = true;              // Looking state of the sprite

    private void Start() {
        // Get the components
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _aiPath = GetComponentInParent<AIPath>();
        _aiDestinationSetter = GetComponentInParent<AIDestinationSetter>();
        isBlinking = false;
    }

    private void Update() {
        // Update the running animation
        _animator.SetFloat(AnimationVariables.Speed, Mathf.Abs(_aiPath.desiredVelocity.x));

        Flip(); // Flip sprite
    }

    // Changes the sprite rendering direction
    private void Flip() {
        // If the boss' target is on the left and the boss is looking right
        if (_aiDestinationSetter.target.position.x < transform.position.x && _isLookingRight)
            _isLookingRight = false;
        // If the boss' target is on the right and the boss is looking left
        else if (_aiDestinationSetter.target.position.x > transform.position.x && !_isLookingRight)
            _isLookingRight = true;

        // FLip based on the boolean
        _sprite.flipX = !_isLookingRight;
    }

    public IEnumerator Blink() {
        isBlinking = true;
        for (int i = 0; i < 3; ++i) {
            _sprite.material.SetColor(ShaderColor, Color.white);
            yield return new WaitForSeconds(0.1f);
            _sprite.material.SetColor(ShaderColor, Color.clear);
            yield return new WaitForSeconds(0.1f);
        }
        isBlinking = false;
    }
}
