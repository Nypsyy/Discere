using UnityEngine;
using static Utils;

public class HeroAnim : MonoBehaviour
{
    public HeroMode CurrentMode { get; private set; }
    public int CurrentDirection { get; private set; }
    public int CurrentSlashDirection { get; private set; }


    private Animator _animator;
    private bool _isIdle;
    private bool _wasIdle;
    private int _previousCurrentDirection;

    private HeroMode _previousMode;

    private AudioManager _audio;

    public void UpdateDirection(Vector2 dir) {
        if (CurrentMode != HeroMode.Move && CurrentMode != HeroMode.Jump) return; // allowing direction changing only when moving/jumping

        _isIdle = (dir.x == 0 && dir.y == 0);
        if (_isIdle) return; // when staying in place, keep previous direction

        // find the direction, negative to make it clockwise
        var angleDeg = -Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // currently angle_deg has 0 for Right, so we offset such that 0 is Up
        angleDeg = (angleDeg + 360 + 112.5f) % 360; // +360 to ensure modulo is done on positive operands

        CurrentDirection = (int) (angleDeg / 45);

        _animator.SetInteger(AnimStrings.Direction, CurrentDirection);
    }

    public void UpdateSlashDirection(Vector2 dir) {
        // find the direction, negative to make it clockwise
        var angleDeg = -Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // currently angle_deg has 0 for Right, so we offset such that 0 is Up
        angleDeg = (angleDeg + 360 + 112.5f) % 360; // +360 to ensure modulo is done on positive operands

        CurrentSlashDirection = (int) (angleDeg / 90);
        _animator.SetInteger(AnimStrings.SlashDirection, CurrentSlashDirection);
    }

    public void SwitchMode(HeroMode m) {
        // ensures that the animator is restarted when changing mode
        _animator.speed = 1;
        _wasIdle = false;
        _isIdle = false;

        _previousMode = CurrentMode;

        CurrentMode = m;

        _animator.SetInteger(AnimStrings.Mode, (int) m);
        _animator.SetTrigger(AnimStrings.SwitchMode);

        switch (m) {
            case HeroMode.Jump when _previousMode != HeroMode.Jump:
                _audio.Play(Sounds.Jump);
                break;

            case HeroMode.Slash:
                _audio.Play(Sounds.Slash);
                break;

            case HeroMode.BigSlash:
                _audio.Play(Sounds.HeavySlash);
                break;

            default:
                switch (_previousMode) {
                    case HeroMode.Jump:
                        _audio.Play(Sounds.Land);
                        break;

                    case HeroMode.BigSlash:
                        _audio.Stop(Sounds.HeavySlash);
                        break;
                }

                break;
        }
    }

    public void Fire() {
        _animator.SetTrigger(AnimStrings.HeroFire);
    }

    // Specify a speed for the animation that is kept until we switch modes
    public void SetModeSpeed(float speed) {
        _animator.speed = speed;
    }

    // Start is called before the first frame update
    private void Start() {
        _animator = GetComponent<Animator>();
        CurrentDirection = 0;
        _previousCurrentDirection = 0;
        CurrentSlashDirection = 0;
        _wasIdle = false;
        _isIdle = false;
        CurrentMode = HeroMode.Move;

        _audio = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    private void Update() {
        // in this function, put extra logic that is not handled by the Animator's state machine
        switch (CurrentMode) {
            case HeroMode.Move:
                UpdateModeMove();
                break;
        }
    }

    private void UpdateModeMove() {
        if (_isIdle) {
            if (!_wasIdle) {
                // we may have just transitioned to Mode.Move
                // in this case, we need to update the animator so it makes correct transitions before stopping it
                _animator.Update(0.1f);

                // then we freeze the current animation on its first frame
                _animator.speed = 0;
                var currentAnimation = _animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
                _animator.Play(currentAnimation, 0, 0); // going to the first frame
            }

            // else is_idle && was_idle: do nothing in particular, animation is already stopped
        }
        else {
            if (_wasIdle) {
                _animator.speed = 1;
                // if we were idle, directly starts the animation at the next frame
                // because the animation sample rate is 6 per second,
                // we need to advance by 1/6 ~= 0.16 seconds to skip a frame
                _animator.Update(0.16f);
            }
            else {
                // if we changed direction, let's advance the move animation so the hero does not look static.
                if (_previousCurrentDirection != CurrentDirection)
                    _animator.Update(0.16f);
            }

            _previousCurrentDirection = CurrentDirection;
        }

        _wasIdle = _isIdle;
    }

    public bool IsAttacking() {
        return _animator.GetBool(AnimStrings.HeroAttacking);
    }
}