using UnityEngine;
using static Utils;

public class MinotaurPrepareDashBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool(AnimationVariables.PrepareDash, true);
    }
}