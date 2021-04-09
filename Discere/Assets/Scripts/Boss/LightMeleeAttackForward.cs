using UnityEngine;
using static Utils;

public class LightMeleeAttackForward : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.GetComponent<Hero>().TakeDamage(Damages.BossLightMelee);
        }
    }
}