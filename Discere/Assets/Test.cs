using UnityEngine;

public class Test : MonoBehaviour
{
    public float radius = 2f;

    void Update() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        var angle = Vector2.SignedAngle(Vector2.right, mousePos) * Mathf.Deg2Rad;
        transform.position = transform.parent.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        transform.right = mousePos - (Vector2)transform.parent.position;
    }
}