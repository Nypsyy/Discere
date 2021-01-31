using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Hero : MonoBehaviour {

    public float speed;

    private Player player;
    private Rigidbody2D body;

    void Awake() {
    	player = ReInput.players.GetPlayer(0);
    	body = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector2 vec = new Vector2(player.GetAxis("MoveHorizontal"),
                                  player.GetAxis("MoveVertical"));
        body.velocity = vec * speed;
    }
}
