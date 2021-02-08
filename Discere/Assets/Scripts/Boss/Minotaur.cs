using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Minotaur : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");

    private Animator _animator;
    private SpriteRenderer _sprite;
    private AIPath _aiPath;
    private AIDestinationSetter _aiDestinationSetter;
    private bool _isLookingRight = true;

    private void Start() {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _aiPath = GetComponentInParent<AIPath>();
        _aiDestinationSetter = GetComponentInParent<AIDestinationSetter>();
    }

    private void Update() {
        _animator.SetFloat(Speed, Mathf.Abs(_aiPath.desiredVelocity.x));

        Flip();
    }

    private void Flip() {
        if (_aiDestinationSetter.target.position.x < transform.position.x && _isLookingRight)
            _isLookingRight = false;
        else if (_aiDestinationSetter.target.position.x > transform.position.x && !_isLookingRight)
            _isLookingRight = true;

        _sprite.flipX = !_isLookingRight;
    }
}