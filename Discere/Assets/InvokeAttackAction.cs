using SGoap;
using static Utils;

public class InvokeAttackAction : BasicAction
{
    public ProjectileWallAttackCost costEvaluator;
    private bool AttackDone => !AgentData.Animator.GetBool(AnimationVariables.BossAttacking) && !Cooldown.Active;
    
    public override bool PrePerform() {
        AgentData.Animator.SetTrigger(AnimationVariables.InvokeAttack);
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return AttackDone ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        costEvaluator.Used();
        return base.PostPerform();
    }
}