using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public bool Hit()
    {
        var animationMessages = GetComponentInParent<AnimationMessages>();
        if (animationMessages.Attacking)
        {
            animationMessages.AttackProcess.State = false;
            return true;
        }

        return false;
    }
}
