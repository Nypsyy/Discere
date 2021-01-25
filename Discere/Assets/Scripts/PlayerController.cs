using UnityEngine;
using System.Collections;
using Rewired;

public class PlayerController : MonoBehaviour
{
    Player player;
    Rigidbody2D rb2D;
    [SerializeField] float speed;
    bool canControl = true;

    Vector2 movement;
    bool dash;


    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        // Init player
        player = ReInput.players.GetPlayer(0);
        // Controller map in gameplay mode
        player.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "Gameplay").enabled = true;
        player.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "UI").enabled = false;
        player.controllers.maps.mapEnabler.Apply();
    }

    void Update()
    {
        GetInputs();
        ProcessInputs();
    }

    void FixedUpdate()
    {
        if (canControl)
            rb2D.velocity = movement * Time.fixedDeltaTime;
    }

    void GetInputs()
    {
        movement = player.GetAxis2D("Move Horizontal", "Move Vertical") * 10f * speed;

        dash = player.GetButtonDown("Dash");
    }

    void ProcessInputs()
    {
        if (dash)
            StartCoroutine(Dash());

    }

    IEnumerator Dash()
    {
        canControl = false;
        rb2D.AddForce(movement.normalized * 15, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        canControl = true;
    }
}