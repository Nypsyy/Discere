using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator Animator;
    public float MovementFactor = 3;

    public bool Attacking => Animator.GetBool("Attacking");
    public bool Guarding => Animator.GetBool("Guarding");

    public void Charge(float chargeTime)
    {
        Animator.SetFloat("AttackTime", chargeTime);
        SetMovement(0);
    }

    public void SetAttackDirection(float x)
    {
        Animator.SetFloat("X", x);
    }

    public void Attack(EAttackDirection direction, float chargeTime = 0)
    {
        if (Attacking)
            return;

        if (chargeTime <= 0.5f)
            chargeTime = 0;

        Animator.SetFloat("X", direction == EAttackDirection.Left ? -1 : 1);
        Animator.SetTrigger("Attack");
        Animator.SetFloat("HoldAttackDuration", chargeTime);
    }

    public void SetMovement(float speed)
    {
        if (Animator.GetFloat("MoveVelocity") != 0 && speed == 0)
            Animator.SetTrigger("Idle");

        Animator.SetFloat("MoveVelocity", speed * MovementFactor);
    }

    public void Death()
    {
        Animator.SetTrigger("Death");
    }

    public void Guard(bool state) 
    {
        Animator.SetBool("Guarding", state);
    }

    public void Idle() 
    { 
        Animator.SetTrigger("Idle");
    }

    public void SetGrounded(bool state) 
    {
        Animator.SetBool("Grounded", state);
    }

    public void Jump() 
    {
        SetMovement(0);
        Animator.SetTrigger("Jump");
    }

    public void PlayImpact()
    {
        Animator.SetTrigger("Impact");
        SetMovement(0);
    }
}

public enum EAttackDirection
{
    Left,
    Right
}
