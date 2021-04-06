using UnityEngine;
using SGoap;
using static Utils;

public class ShockwaveAttackAction : BasicAction
{
    private ShockwaveAttackCost _costEvaluator;
    private bool AttackDone => !AgentData.Animator.GetBool(AnimationVariables.BossAttacking) && !Cooldown.Active;

    private void Awake() {
        _costEvaluator = GetComponentInChildren<ShockwaveAttackCost>();
    }

    public override bool PrePerform() {
        AgentData.Animator.SetTrigger(AnimationVariables.ShockwaveAttack);
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return AttackDone ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        _costEvaluator.Used();
        // Wander goal priority
        AgentData.Agent.Goals[2].Priority += 10 + Random.Range(0, 30);
        AgentData.Agent.UpdateGoalOrderCache();
        return base.PostPerform();
    }
}