using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer ray;
    public Color readyRayColor;
    public float readyRayWidth = 0.1f;
    private float initialRayWidth;

    public GameObject chargingCircle;
    public LayerMask obstacleLayers;

    public float chargingTime = 1f;
    private float chargingTimer = 0f;
    private Vector3 initialChargingCircleScale;

    private Vector2 dir = Vector2.up;
    private bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {
        initialChargingCircleScale = chargingCircle.transform.localScale;
        initialRayWidth = ray.startWidth;
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
        // Update timer
        chargingTimer += Time.deltaTime;

        // If charging
        if (chargingTimer <= chargingTime)
        {
            // Update charging circle 
            chargingCircle.transform.localScale = initialChargingCircleScale * (chargingTime - chargingTimer) / chargingTime;

            // Update ray width
            float newWidth = (initialRayWidth - readyRayWidth) * (chargingTime - chargingTimer) / chargingTime + readyRayWidth;
            ray.startWidth = newWidth;
            ray.endWidth = newWidth;
        }
        // If transitionning to "ready" state
        else if (!isReady)
        {
            chargingCircle.SetActive(false);
            StartCoroutine(ReadyRay());
            isReady = true;
        }

        // Raycast in laser's direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, float.PositiveInfinity, obstacleLayers);
        Vector3 hitCoords = transform.InverseTransformPoint(hit.point);
        Vector3[] points = new Vector3[2];

        // Update ray's points
        points[0] = Vector3.zero;
        points[1] = new Vector3(hitCoords.x, hitCoords.y, 0f);
        ray.SetPositions(points);
    }

    IEnumerator ReadyRay()
    {
        ray.startColor = Color.white;
        ray.endColor = Color.white;
        yield return new WaitForSeconds(0.05f);
        ray.startColor = readyRayColor;
        ray.endColor = readyRayColor;
    }
}
