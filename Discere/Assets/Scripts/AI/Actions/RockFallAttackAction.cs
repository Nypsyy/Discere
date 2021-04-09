using SGoap;
using static Utils;

public class RockFallAttackAction : BasicAction
{
    private RockFallAttackCost _costEvaluator;
    private bool AttackDone => !AgentData.Animator.GetBool(AnimStrings.BossAttacking) && !Cooldown.Active;

    private void Awake() {
        _costEvaluator = GetComponentInChildren<RockFallAttackCost>();
    }

    public override bool PrePerform() {
        AgentData.Animator.SetTrigger(AnimStrings.BossRockFallAttack);
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return AttackDone ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        _costEvaluator.Used();
        // Wander goal priority
        AgentData.Agent.Goals[2].Priority += 100;
        AgentData.Agent.UpdateGoalOrderCache();
        return base.PostPerform();
    }
}