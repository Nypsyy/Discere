using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Hero : MonoBehaviour {

    public float speed;
    public HeroAnim anim;

    private Player player;
    private Vector2 input_vec;
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
    }

    // Update is called once per frame
    void Update() {
        input_vec = player.GetAxis2D("Move Horizontal", "Move Vertical");

        anim.UpdateDirection(input_vec);
        if (player.GetButtonDown("Light Attack"))
            anim.TriggerSlash();

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
    }

    public void OnHealthEmpty()
    {
        Debug.Log("Should die");
    }
}
