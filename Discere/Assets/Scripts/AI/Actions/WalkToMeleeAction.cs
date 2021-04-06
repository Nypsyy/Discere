using Pathfinding;
using SGoap;
using UnityEngine;

public class WalkToMeleeAction : BasicAction
{
    public StringReference heroKeepingDistanceState;
    public AIPath aiPath;
    public LightMeleeAttackCost lightMeleeAttackCost;
    public RangeSensor rangeSensor;
    public float abortTime;
    
    private bool TookTooLong =>  TimeElapsed > abortTime;

    public override bool PrePerform() {
        aiPath.canMove = true;
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        if (TookTooLong)
            return EActionStatus.Failed;

        return rangeSensor.InMeleeRange ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        aiPath.canMove = false;
        return base.PostPerform();
    }

    public override void OnFailed() {
        aiPath.canMove = false;
        lightMeleeAttackCost.Failed();
        AgentData.Agent.States.SetState(heroKeepingDistanceState.Value, 1);
    }
}