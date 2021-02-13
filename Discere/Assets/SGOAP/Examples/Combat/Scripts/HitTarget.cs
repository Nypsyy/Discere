using UnityEngine;

namespace SGoap
{
    public class HitTarget : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            var hitCollider = other.GetComponentInParent<HitCollider>();

            if (hitCollider != null && hitCollider.Hit())
            {
                var damagable = GetComponentInParent<IDamagable>();
                var attacker = other.GetComponentInParent<IAttacker>();
                damagable.TakeDamage(attacker: attacker);
            }
        }
    }
}
