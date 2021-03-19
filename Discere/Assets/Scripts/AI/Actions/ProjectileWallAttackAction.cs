using SGoap;
using static Utils;

public class ProjectileWallAttackAction : BasicAction
{
    private bool AttackDone => !AgentData.Animator.GetBool(AnimationVariables.BossAttacking) && !Cooldown.Active;

    public override bool PrePerform() {
        AgentData.Animator.SetTrigger(AnimationVariables.ProjectileWallAttack);
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return AttackDone ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        Cost += 0.1f;
        return base.PostPerform();
    }
}