using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnim : MonoBehaviour
{

    public enum Mode {
        Move = 0,
        Slash = 1,
        Jump = 2,
        BigSlash = 3,
        Aim = 4
    }
    
    public Mode CurrentMode { get; private set; }
    public int CurrentDirection { get; private set; }
    public int CurrentSlashDirection { get; private set; }
    
    
    private Animator _animator;
    private bool _isIdle;
    private bool _wasIdle;
    private int _previousCurrentDirection;

    private Mode previousMode;

    private new AudioManager audio;
    
    public void UpdateDirection(Vector2 dir) {
        if (CurrentMode != Mode.Move && CurrentMode != Mode.Jump) return; // allowing direction changing only when moving/jumping
    
        _isIdle = (dir.x == 0 && dir.y == 0);
        if (_isIdle) return; // when staying in place, keep previous direction
        
        // find the direction, negative to make it clockwise
        float angle_deg = -Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // currently angle_deg has 0 for Right, so we offset such that 0 is Up
        angle_deg = (angle_deg + 360 + 112.5f) % 360; // +360 to ensure modulo is done on positive operands
        
        CurrentDirection = (int)(angle_deg / 45);
        
        _animator.SetInteger("Direction(Up:0,Clockwise)", CurrentDirection);
        
    }
    
    public void UpdateSlashDirection(Vector2 dir) {
        // find the direction, negative to make it clockwise
        float angle_deg = -Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // currently angle_deg has 0 for Right, so we offset such that 0 is Up
        angle_deg = (angle_deg + 360 + 112.5f) % 360; // +360 to ensure modulo is done on positive operands
        
        CurrentSlashDirection = (int)(angle_deg / 90);
        _animator.SetInteger("SlashDirection", CurrentSlashDirection);
    }
    
    public void SwitchMode(Mode m) {
        // ensures that the animator is restarted when changing mode
        _animator.speed = 1;
        _wasIdle = false;
        _isIdle = false;

        previousMode = CurrentMode;

        CurrentMode = m;
        
        _animator.SetInteger("Mode", (int)m);
        _animator.SetTrigger("SwitchMode");

        switch (m)
        {
            case Mode.Jump when previousMode != Mode.Jump:
                audio.Play("Jump");
                break;

            case Mode.Slash:
                audio.Play("Slash");
                break;

            case Mode.BigSlash:
                audio.Play("HeavySlash");
                break;

            default:
                switch(previousMode)
                {
                    case Mode.Jump:
                        audio.Play("Land");
                        break;

                    case Mode.BigSlash:
                        audio.Stop("HeavySlash");
                        break;
                }
                break;
        }
    }
    public void Fire()
    {
        _animator.SetTrigger("Fire");
    }

    // Specify a speed for the animation that is kept until we switch modes
    public void SetModeSpeed(float speed) {
        _animator.speed = speed;
    }
    
    // Start is called before the first frame update
    void Start() {
        _animator = GetComponent<Animator>();
        CurrentDirection = 0;
        _previousCurrentDirection = 0;
        CurrentSlashDirection = 0;
        _wasIdle = false;
        _isIdle = false;
        CurrentMode = Mode.Move;

        audio = FindObjectOfType<AudioManager>();
    }
    
    // Update is called once per frame
    void Update() {
        // in this function, put extra logic that is not handled by the Animator's state machine
        switch (CurrentMode) {
            case Mode.Move:
                UpdateModeMove();
                break;
            default:
                break;
        }
    }
    
    void UpdateModeMove() {
        if (_isIdle) {
            if (!_wasIdle) {
                // we may have just transitioned to Mode.Move
                // in this case, we need to update the animator so it makes correct transitions before stopping it
                _animator.Update(0.1f);
                
                // then we freeze the current animation on its first frame
                _animator.speed = 0;
                int current_animation = _animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
                _animator.Play(current_animation, 0, 0); // going to the first frame
            } else {
                // is_idle && was_idle: do nothing in particular, animation is already stopped
            }
        } else {
            if (_wasIdle) {
                _animator.speed = 1;
                // if we were idle, directly starts the animation at the next frame
                // because the animation sample rate is 6 per second,
                // we need to advance by 1/6 ~= 0.16 seconds to skip a frame
                _animator.Update(0.16f);
            } else {
                // if we changed direction, let's advance the move animation so the hero does not look static.
                if (_previousCurrentDirection != CurrentDirection)
                    _animator.Update(0.16f);
            }
            _previousCurrentDirection = CurrentDirection;
        }
        _wasIdle = _isIdle;
    }

    public bool IsAttacking() {
        return _animator.GetBool(Utils.AnimationVariables.IsAttacking);
    }
}
