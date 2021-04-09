using UnityEngine;

public class RockCollisionBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Rock"))
            Destroy(other.gameObject);
    }
}