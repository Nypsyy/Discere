using UnityEngine;
using static Utils.AnimStrings;

public class MinotaurAttackBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool(BossAttacking, true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool(BossAttacking, false);
    }
}