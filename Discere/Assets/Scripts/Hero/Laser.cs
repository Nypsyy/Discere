using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer ray;
    public LayerMask obstacleLayers;

    private Vector2 dir = Vector2.up;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetDirection(Vector2 direction)
    {
        dir = direction;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, float.PositiveInfinity, obstacleLayers);
        Vector3 hitCoords = transform.InverseTransformPoint(hit.point);
        Vector3[] points = new Vector3[2];
        points[0] = Vector3.zero;
        points[1] = new Vector3(hitCoords.x, hitCoords.y, 0f);
        ray.SetPositions(points);
    }
}
