using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// Responsible for displaying the Minautor / changing its aspect
[RequireComponent(typeof(SpriteRenderer))]
public class MinautorSprite : MonoBehaviour
{
    // String IDs
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int ShaderColor = Shader.PropertyToID("_Color");

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
    }

    private void Update() {
        // Update the running animation
        _animator.SetFloat(Speed, Mathf.Abs(_aiPath.desiredVelocity.x));


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
        _sprite.material.SetColor(ShaderColor, Color.white);
        yield return new WaitForSeconds(.1f);
        _sprite.material.SetColor(ShaderColor, Color.clear);
    }
}