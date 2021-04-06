using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSword : MonoBehaviour
{
    
    public float small_damage = 5;
    public float big_damage = 15;
    
    private Animator animator;
    private PolygonCollider2D collider;
    
    private Vector2[] position_corrections = {
        new Vector2(0, 0.0f), // right slash
        new Vector2(-0.5f, -0.2f), // up slash
        new Vector2(0, -0.55f), // left slash
        new Vector2(0.5f, -0.1f)  // down slash
    };
    
    private float charging_time = 0; // if positive, remaining time before big slash is fully charged
    private int current_dir = 0;
    private bool is_big_slash = false;
    private bool actually_slashing = false;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<PolygonCollider2D>();
        collider.enabled = false;
    }
    
    // Hero will call GetSpeedForHeroAnimator() and apply its value to HeroAnim.SetModeSpeed
    public float GetSpeedForHeroAnimator() {
        return (charging_time <= 0) ? 1 : 0;
    }
    
    public void TriggerSlash(Vector2 direction, bool is_big) {
        is_big_slash = is_big;
        
        // get direction as angle
        float angle_deg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // ensuring angle is in [0;360[
        angle_deg = (angle_deg + 360 + 67.5f) % 360;
        // we want to have 0=right, 1=down, 2=left, 3=up
        current_dir = (int)(angle_deg / 90);
        // then we modify the sword direction
        // and we modify the position of the sword
        transform.localPosition = position_corrections[current_dir];
        // finally we trigger the animation
        animator.SetInteger("Direction", current_dir);
        
        if (is_big_slash) {
            animator.SetTrigger("BigSlash");
            charging_time = 0.5f;
        } else {
            animator.SetTrigger("Slash");
            charging_time = 0;
        }
        actually_slashing = false; // will be true after 'charging_time'
    }
    
    public bool CancelBigSlash() {
        if (charging_time <= 0) // too late
            return false;
        animator.SetTrigger("Reset");
        transform.parent.localPosition = Vector3.zero;
        charging_time = 0;
        return true;
    }
    
    static float easing(float progress) {
        // from [0;1] to [0:1], fast rising at beginning
        return 1 - (1-progress)*(1-progress);
    }
    
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("HeroSword_NoSlash")) {
            collider.enabled = false;
        }
        //collider.enabled = true;
        if (charging_time > 0) {
            float new_charging_time = charging_time - Time.deltaTime;
            float progress = Mathf.InverseLerp(0.5f, 0, new_charging_time);
            if (new_charging_time % 0.05f > charging_time % 0.05f || Time.deltaTime >= 0.05f) {
                // every 0.05 second = 20fps
                transform.parent.localPosition = 0.15f * easing(progress) * new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            }
            charging_time = new_charging_time;
        } else {
            transform.parent.localPosition = Vector3.zero;
            if (!actually_slashing) {
                actually_slashing = true;
                collider.enabled = true;
            }
        }
    }
    
    void OnTriggerStay2D(Collider2D collider) {
        collider.gameObject.GetComponent<Minotaur>()?.TakeDamage(
            is_big_slash ? big_damage : small_damage, FightingStyle.Style.Melee);
        
        if (is_big_slash) collider.gameObject.GetComponent<Rock>()?.DestroyMe();
    }
}
