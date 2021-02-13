using UnityEngine;

namespace SGoap
{
    /// <summary>
    /// Evaluate true if the target is in its attacking state.
    /// </summary>
    public class TargetAttackingUsableEvaluator : UsableEvaluator
    {
        public override bool Evaluate(IContext context)
        {
            if (AgentData?.Target == null)
                return false;

            var target = AgentData.Target;
            var animator = target.GetComponentInChildren<Animator>();
            var attacking = animator.GetBool("Attacking");

            return attacking;
        }
    }
}