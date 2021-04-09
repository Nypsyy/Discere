using SGoap;
using static Utils;
using Random = UnityEngine.Random;

public class LightMeleeAttackAction : BasicAction
{
    private LightMeleeAttackCost _costEvaluator;
    private bool AttackDone => !AgentData.Animator.GetBool(AnimStrings.BossAttacking) && !Cooldown.Active;


    private void Awake() {
        _costEvaluator = GetComponentInChildren<LightMeleeAttackCost>();
    }

    public override bool PrePerform() {
        AgentData.Animator.SetTrigger(AnimStrings.LightMeleeAttack);
        CinemachineEffects.Instance.Zoom(1.2f);
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return AttackDone ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        _costEvaluator.Used();
        // Wander goal priority
        AgentData.Agent.Goals[2].Priority += 10 + Random.Range(0,10);
        AgentData.Agent.UpdateGoalOrderCache();
        CinemachineEffects.Instance.UnZoom();
        return base.PostPerform();
    }

    public override void OnFailed() {
        _costEvaluator.Failed();
        Cooldown.Run(2);
    }
}