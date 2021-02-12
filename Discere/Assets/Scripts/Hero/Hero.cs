using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Hero : MonoBehaviour {

    public float speed;
    public HeroAnim anim;
    public HeroSword sword;
    public GameObject daggerPrefab;

    private Player player;
    private Vector2 input_vec;
    private Vector2 facing_vec;
    private Rigidbody2D body;

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
    }

    // Update is called once per frame
    void Update() {
        input_vec = player.GetAxis2D("Move Horizontal", "Move Vertical");

        if (input_vec.x != 0 || input_vec.y != 0)
        {
            facing_vec = input_vec;
        }

        anim.UpdateDirection(input_vec);
        if (player.GetButtonDown("Light Attack") && anim.mode == HeroAnim.Mode.Move)
        {
            anim.SwitchMode(HeroAnim.Mode.Slash);
            if (fightingStyle.currentStyle == FightingStyle.Style.Range) {
                Vector2 shootingDirection = facing_vec.normalized;
                GameObject dagger = Instantiate(daggerPrefab, transform.position - new Vector3(0.0f,0.5f), Quaternion.identity);
                dagger.GetComponent<Rigidbody2D>().velocity = shootingDirection * dagger.GetComponent<Projectiles>().velocity;
                dagger.transform.Rotate(0.0f, 0.0f, -45.0f);
                dagger.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
            }
            else if (fightingStyle.currentStyle == FightingStyle.Style.Melee) {
                sword.TriggerSlash(facing_vec);
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
        if (anim.mode == HeroAnim.Mode.Move)
            body.velocity = input_vec * speed;
        else
            body.velocity = Vector2.zero;
    }

    public void OnHealthEmpty()
    {
        Debug.Log("Should die");
    }
}
