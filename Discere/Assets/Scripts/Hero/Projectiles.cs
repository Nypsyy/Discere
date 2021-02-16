using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public float velocity;
    public float damage;
    public int destructionTime;
    private bool destructing;
    private int destructionTimer;

    protected void Start()
    {
        destructing = false;
        destructionTimer = destructionTime;
    }

    protected void Update()
    {
        if (destructing)
        {
            destructionTimer--;

        }

        if(destructionTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Boss")) {
            Debug.Log("HIT");
        }
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            destructing = true;
        }
    }
}