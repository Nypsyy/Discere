using SGoap;
using UnityEngine;
using static Utils;

public class InvokeAttackAction : BasicAction
{
    public ProjectileWallAttackCost costEvaluator;
    private bool AttackDone => !AgentData.Animator.GetBool(AnimStrings.BossAttacking) && !Cooldown.Active;

    public override bool PrePerform() {
        AgentData.Animator.SetTrigger(AnimStrings.InvokeAttack);
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return AttackDone ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        costEvaluator.Used();
        // Wander goal priority
        AgentData.Agent.Goals[2].Priority += 100;
        AgentData.Agent.UpdateGoalOrderCache();
        return base.PostPerform();
    }
}