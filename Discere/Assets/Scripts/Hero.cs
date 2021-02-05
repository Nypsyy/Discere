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
        
    }

    // Update is called once per frame
    void Update() {
        input_vec = player.GetAxis2D("Move Horizontal", "Move Vertical");
        anim.UpdateDirection(input_vec);
        if (player.GetButtonDown("Light Attack"))
            anim.TriggerSlash();
    }
    
    
    void FixedUpdate() {
        body.velocity = input_vec * speed;
    }
}
