using UnityEngine;
using static Utils;

public class LightMeleeAttackBelow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.GetComponent<Hero>().TakeDamage(DamageVariables.BossLightMelee);
        }
    }
}