using SGoap;
using UnityEngine;
using static Utils;

public class ProjectileWallAttackAction : BasicAction
{
    private ProjectileWallAttackCost _costEvaluator;
    private bool AttackDone => !AgentData.Animator.GetBool(AnimStrings.BossAttacking) && !Cooldown.Active;

    private void Awake() {
        _costEvaluator = GetComponentInChildren<ProjectileWallAttackCost>();
    }

    public override bool PrePerform() {
        AgentData.Animator.SetTrigger(AnimStrings.ProjectileWallAttack);
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return AttackDone ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        _costEvaluator.Used();
        // Wander goal priority
        AgentData.Agent.Goals[2].Priority += 10 + Random.Range(0, 60);
        AgentData.Agent.UpdateGoalOrderCache();
        return base.PostPerform();
    }
}