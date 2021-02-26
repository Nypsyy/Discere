using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : MonoBehaviour
{

    public GameObject[] Arrows;
    public GameObject[] VisualArrows;
    public float timeBetweenArrows;

    private GameObject arrow;
    private GameObject firedArrow;
    private float creationTimer;
    private int arrowNumber = 1;
    private bool firing = false;

    
    // Start is called before the first frame update
    void Start()
    {
        arrow = Instantiate(VisualArrows[arrowNumber - 1], transform.position, transform.rotation, transform);
        creationTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(arrowNumber < VisualArrows.Length && !firing && Time.time> creationTimer + timeBetweenArrows * arrowNumber)
        {
            Destroy(arrow);
            arrowNumber++;
            arrow = Instantiate(VisualArrows[arrowNumber - 1], transform.position, transform.rotation, transform);
        }
    }

    public void Shoot()
    {
        firedArrow = Instantiate(Arrows[arrowNumber - 1], transform.position, transform.rotation);
        firing = true;
        firedArrow.GetComponent<Rigidbody2D>().velocity = transform.right * firedArrow.GetComponent<Projectiles>().velocity;
        Destroy(gameObject);
    }
}
