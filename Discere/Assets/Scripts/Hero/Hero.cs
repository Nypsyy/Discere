using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Hero : MonoBehaviour {

    public float speed;
    public HeroAnim anim;
    public HeroSword sword;
    public GameObject daggerPrefab;
    public float firingFrequency;

    private Player player;
    private Vector2 input_vec;
    private Vector2 facing_vec;
    private Vector2 aiming_vec_Joystick;
    private Vector2 aiming_vec_Mouse;
    private Rigidbody2D body;
    private float firingCooldown;

    // Components
    private Health health;
    private Mana mana;
    private FightingStyle fightingStyle;

    void Awake() {
        player = ReInput.players.GetPlayer(0);
        body = GetComponent<Rigidbody2D>();
        
        // Controller map in gameplay mode
        player.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "Gameplay").enabled = true;
        player.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "UI").enabled = false;
        player.controllers.maps.mapEnabler.Apply();
    }

    // Start is called before the first frame update
    void Start() {
        health = GetComponent<Health>();
        mana = GetComponent<Mana>();
        fightingStyle = GetComponent<FightingStyle>();
        facing_vec = new Vector2(1.0f, 0);
        firingCooldown = -1;
    }



    // Update is called once per frame
    void Update() {
        input_vec = player.GetAxis2D("Move Horizontal", "Move Vertical");


        if (anim.mode == HeroAnim.Mode.Move && !(input_vec.x == 0 && input_vec.y == 0)) {
            // only updating facing_vec when actually moving
            facing_vec = input_vec;
        }
        anim.UpdateDirection(input_vec);


        switch (fightingStyle.currentStyle) {
            case FightingStyle.Style.Melee:
                UpdateAttackMelee();
                break;
                
            case FightingStyle.Style.Range:
                UpdateAttackRange();
                break;
                
            default:
                break;
        }

        // DEBUG : hurt hero when dashing, and switch fighting style
        if (player.GetButtonDown("Dash"))
        {
            health.TakeDamage(30f);
            mana.UseMana(50f);
            fightingStyle.SwitchStyle();
        }

    }
    
    void UpdateAttackMelee() {
        if (player.GetButtonDown("Light Attack") && anim.mode == HeroAnim.Mode.Move) {
            anim.UpdateSlashDirection(facing_vec);
            anim.SwitchMode(HeroAnim.Mode.Slash);
            sword.TriggerSlash(facing_vec);
        }
    }
    
    
    void UpdateAttackRange() {
        if (firingCooldown >= 0) return; // ensures cooldown has expired
        
        Vector2 shootingDirection = getAimingDirection();
        if (shootingDirection.x == 0 && shootingDirection.y == 0) return;
        
        shootingDirection = shootingDirection.normalized;
        
        // shooting the dagger
        GameObject dagger = Instantiate(daggerPrefab, transform.position - new Vector3(0.0f, 0.0f), Quaternion.identity);
        dagger.GetComponent<Rigidbody2D>().velocity = shootingDirection * dagger.GetComponent<Projectiles>().velocity;
        dagger.transform.Rotate(0.0f, 0.0f, -45.0f);
        dagger.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        firingCooldown = (1 / firingFrequency) * 60;
        
        // triggering the animation
        anim.UpdateDirection(shootingDirection);
        anim.UpdateSlashDirection(shootingDirection);
        anim.SwitchMode(HeroAnim.Mode.Slash);
        anim.SetModeSpeed(3); // Slash animation 3 times faster
    }
    
    Vector2 getAimingDirection() {
        Controller controller = player.controllers.GetLastActiveController();
        if (controller == null) return Vector2.zero;
        
        if (controller.type == ControllerType.Joystick) {
            return player.GetAxis2D("Aim Horizontal", "Aim Vertical");
        }
        else {
            if (!player.GetButton("Light Attack")) return Vector2.zero;
            
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            return worldPosition - new Vector2(transform.position.x, transform.position.y);
        }
    }
    
    void FixedUpdate() {
        if (firingCooldown >= 0)
            firingCooldown--;
        switch (anim.mode) {
            case HeroAnim.Mode.Move:
                body.velocity = input_vec * speed;
                break;
            case HeroAnim.Mode.Slash:
                body.velocity = input_vec * speed * 0.2f;
                break;
            default:
                body.velocity = Vector2.zero;
                break;
        }
    }

    public void OnHealthEmpty()
    {
        Debug.Log("Should die");
    }
}
