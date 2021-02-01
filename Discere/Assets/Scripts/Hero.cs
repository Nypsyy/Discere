using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Hero : MonoBehaviour {

    public float speed;

    private Player player;
    private Rigidbody2D body;
    private Animator animator;
    private Vector2 input_vec;
    private bool is_idle;

    // Components
    private Health health;

    void Awake() {
        player = ReInput.players.GetPlayer(0);
        body = GetComponent<Rigidbody2D>();
        animator = transform.Find("Hero").GetComponent<Animator>();
        input_vec = new Vector2(0,0);
        is_idle = true;
        
        // Controller map in gameplay mode
        player.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "Gameplay").enabled = true;
        player.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "UI").enabled = false;
        player.controllers.maps.mapEnabler.Apply();
    }

    // Start is called before the first frame update
    void Start() {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update() {
        input_vec = player.GetAxis2D("Move Horizontal", "Move Vertical");

        // DEBUG : hurt hero when dashing
        if (player.GetButtonDown("Dash"))
        {
            health.TakeDamage(30f);
        }
        
        UpdateAnimation();
    }
    
    void UpdateAnimation() {
        if (input_vec.x == 0 && input_vec.y == 0) {
                // if no movements, we stay with the first frame of the current animation.
                animator.speed = 0;
                is_idle = true;
                int current_animation = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
                animator.Play(current_animation, 0, 0); // going to the first frame
            }
            else {
                // if movements, restart animation
                animator.speed = 1;
                // find the direction, negative to make it clockwise
                float angle_deg = -Mathf.Atan2(input_vec.y, input_vec.x) * Mathf.Rad2Deg;
                // currently angle_deg has 0 for Right, so we offset such that 0 is Up
                angle_deg = (angle_deg + 360 + 90) % 360; // +360 to ensure modulo is done on positive operands
                
                // Special case for direction = Up = 0
                if (angle_deg < 22.5f || angle_deg >= 337.5f) {
                    animator.SetInteger("Direction(Up:0,Clockwise)", 0);
                }
                else {
                    int dir = (int)((angle_deg + 22.5f) / 45); // [22.5;67.5[ gives 1, [67.5;112.5[ gives 2, etc
                    animator.SetInteger("Direction(Up:0,Clockwise)", dir);
                }
                if (is_idle) {
                    // if we were idle, directly starts the animation at the next frame
                    // because the animation sample rate is 6 per second,
                    // we need to advance by 1/6 ~= 0.16 seconds to skip a frame
                    animator.Update(0.16f);
                    is_idle = false;
                }
            }
    }
    
    void FixedUpdate() {
        body.velocity = input_vec * speed;
    }

    public void OnHealthEmpty()
    {
        Debug.Log("Should die");
    }
}
