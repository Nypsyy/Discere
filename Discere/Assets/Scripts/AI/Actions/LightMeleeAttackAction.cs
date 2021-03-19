using SGoap;
using static Utils;

public class LightMeleeAttackAction : BasicAction
{
    private bool AttackDone => !AgentData.Animator.GetBool(AnimationVariables.BossAttacking) && !Cooldown.Active;

    public override bool PrePerform() {
        AgentData.Animator.SetTrigger(AnimationVariables.LightMeleeAttack);

        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return AttackDone ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        Cost += 0.1f;
        return base.PostPerform();
    }

    public override void OnFailed() {
        Cooldown.Run(2);
    }
}