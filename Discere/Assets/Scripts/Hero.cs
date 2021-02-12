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
        aiming_vec_Joystick = player.GetAxis2D("Aim Horizontal", "Aim Vertical");
        Controller controller = player.controllers.GetLastActiveController();
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        aiming_vec_Mouse = worldPosition - new Vector2(transform.position.x, transform.position.y);

        if (input_vec.x != 0 || input_vec.y != 0)
        {
            facing_vec = input_vec;
        }

        if(firingCooldown < 0) 
            anim.UpdateDirection(input_vec);
        else if (controller != null && controller.type != ControllerType.Joystick)
            anim.UpdateDirection(aiming_vec_Mouse);
        else if (controller != null && controller.type == ControllerType.Joystick)
            anim.UpdateDirection(aiming_vec_Joystick);

        if (player.GetButtonDown("Light Attack") && !anim.is_slashing)
        {
            anim.TriggerSlash();
            /*if (fightingStyle.currentStyle == FightingStyle.Style.Range && controller != null && controller.type != ControllerType.Joystick) {
                Vector2 shootingDirection = aiming_vec.normalized;
                GameObject dagger = Instantiate(daggerPrefab, transform.position - new Vector3(0.0f,0.5f), Quaternion.identity);
                dagger.GetComponent<Rigidbody2D>().velocity = shootingDirection * dagger.GetComponent<Projectiles>().velocity;
                dagger.transform.Rotate(0.0f, 0.0f, -45.0f);
                dagger.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
            }*/
            if (fightingStyle.currentStyle == FightingStyle.Style.Melee) {
                sword.TriggerSlash(facing_vec);
            }
        }

        if (player.GetButton("Light Attack"))
        {
            if (fightingStyle.currentStyle == FightingStyle.Style.Range && controller != null && controller.type != ControllerType.Joystick)
            {
                if (firingCooldown < 0)
                {
                    
                    anim.TriggerSlash();
                    Vector2 shootingDirection = aiming_vec_Mouse.normalized;
                    GameObject dagger = Instantiate(daggerPrefab, transform.position - new Vector3(0.0f, 0.0f), Quaternion.identity);
                    dagger.GetComponent<Rigidbody2D>().velocity = shootingDirection * dagger.GetComponent<Projectiles>().velocity;
                    dagger.transform.Rotate(0.0f, 0.0f, -45.0f);
                    dagger.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
                    firingCooldown = (1 / firingFrequency) * 60;
                }
            }
        }
        
        if(fightingStyle.currentStyle == FightingStyle.Style.Range && controller != null && controller.type == ControllerType.Joystick)
        {
            if (aiming_vec_Joystick.x != 0 || aiming_vec_Joystick.y != 0)
            {
                if (firingCooldown < 0)
                {
                    anim.TriggerSlash();
                    Vector2 shootingDirection = aiming_vec_Joystick.normalized;
                    GameObject dagger = Instantiate(daggerPrefab, transform.position - new Vector3(0.0f, 0.0f), Quaternion.identity);
                    dagger.GetComponent<Rigidbody2D>().velocity = shootingDirection * dagger.GetComponent<Projectiles>().velocity;
                    dagger.transform.Rotate(0.0f, 0.0f, -45.0f);
                    dagger.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
                    firingCooldown = (1 / firingFrequency) * 60;
                }
            }
            
        }

        // DEBUG : hurt hero when dashing, and switch fighting style
        if (player.GetButtonDown("Dash"))
        {
            health.TakeDamage(30f);
            mana.UseMana(50f);
            fightingStyle.SwitchStyle();
        }

    }
    
    
    void FixedUpdate() {
        if (anim.is_slashing)
            body.velocity = input_vec * speed * 0.0f;
        else
            body.velocity = input_vec * speed;
        if (firingCooldown >= 0)
            firingCooldown--;
    }

    public void OnHealthEmpty()
    {
        Debug.Log("Should die");
    }
}
