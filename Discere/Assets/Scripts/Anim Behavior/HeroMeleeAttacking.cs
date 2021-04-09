using UnityEngine;
using static Utils;

public class HeroMeleeAttacking : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool(AnimStrings.HeroAttacking, true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool(AnimStrings.HeroAttacking, false);
    }
}