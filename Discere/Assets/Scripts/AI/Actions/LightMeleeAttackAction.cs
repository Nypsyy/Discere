using SGoap;
using static Utils.AnimationVariables;

public class LightMeleeAttackAction : BasicAction
{
    private bool AttackDone => !AgentData.Animator.GetBool(Attacking) && !Cooldown.Active;

    public override bool PrePerform() {
        AgentData.Animator.SetTrigger(LightMeleeAttack);

        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return AttackDone ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        Cost++;
        return base.PostPerform();
    }
}