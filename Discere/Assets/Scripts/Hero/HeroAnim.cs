using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnim : MonoBehaviour
{

    public enum Mode {
        Move = 0,
        Slash = 1,
    }
    
    public Mode mode { get; private set; }
    public int current_direction { get; private set; }
    public int current_slashdir { get; private set; }
    
    
    private Animator animator;
    private bool is_idle;
    private bool was_idle;
    private int previous_current_direction;
    
    public void UpdateDirection(Vector2 dir) {
        if (mode != Mode.Move) return; // allowing direction changing only when moving
    
        is_idle = (dir.x == 0 && dir.y == 0);
        if (is_idle) return; // when staying in place, keep previous direction
        
        // find the direction, negative to make it clockwise
        float angle_deg = -Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // currently angle_deg has 0 for Right, so we offset such that 0 is Up
        angle_deg = (angle_deg + 360 + 112.5f) % 360; // +360 to ensure modulo is done on positive operands
        
        current_direction = (int)(angle_deg / 45);
        
        animator.SetInteger("Direction(Up:0,Clockwise)", current_direction);
        
    }
    
    public void UpdateSlashDirection(Vector2 dir) {
        // find the direction, negative to make it clockwise
        float angle_deg = -Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // currently angle_deg has 0 for Right, so we offset such that 0 is Up
        angle_deg = (angle_deg + 360 + 112.5f) % 360; // +360 to ensure modulo is done on positive operands
        
        current_slashdir = (int)(angle_deg / 90);
        animator.SetInteger("SlashDirection", current_slashdir);
    }
    
    public void SwitchMode(Mode m) {
        // ensures that the animator is restarted when changing mode
        animator.speed = 1;
        was_idle = false;
        is_idle = false;
        
        mode = m;
        animator.SetInteger("Mode", (int)m);
        animator.SetTrigger("SwitchMode");
        
    }
    
    // Specify a speed for the animation that is kept until we switch modes
    public void SetModeSpeed(float speed) {
        animator.speed = speed;
    }
    
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        current_direction = 0;
        previous_current_direction = 0;
        current_slashdir = 0;
        was_idle = false;
        is_idle = false;
        mode = Mode.Move;
    }
    
    // Update is called once per frame
    void Update() {
        // in this function, put extra logic that is not handled by the Animator's state machine
        switch (mode) {
            case Mode.Move:
                UpdateModeMove();
                break;
            default:
                break;
        }
    }
    
    void UpdateModeMove() {
        if (is_idle) {
            if (!was_idle) {
                // we may have just transitioned to Mode.Move
                // in this case, we need to update the animator so it makes correct transitions before stopping it
                animator.Update(0.1f);
                
                // then we freeze the current animation on its first frame
                animator.speed = 0;
                int current_animation = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
                animator.Play(current_animation, 0, 0); // going to the first frame
            } else {
                // is_idle && was_idle: do nothing in particular, animation is already stopped
            }
        } else {
            if (was_idle) {
                animator.speed = 1;
                // if we were idle, directly starts the animation at the next frame
                // because the animation sample rate is 6 per second,
                // we need to advance by 1/6 ~= 0.16 seconds to skip a frame
                animator.Update(0.16f);
            } else {
                // if we changed direction, let's advance the move animation so the hero does not look static.
                if (previous_current_direction != current_direction)
                    animator.Update(0.16f);
            }
            previous_current_direction = current_direction;
        }
        was_idle = is_idle;
    }
    
}
