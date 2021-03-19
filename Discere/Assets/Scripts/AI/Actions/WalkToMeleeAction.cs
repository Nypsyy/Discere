using Pathfinding;
using SGoap;
using UnityEngine;

public class WalkToMeleeAction : BasicAction
{
    public StringReference heroKeepingDistanceState;
    public AIPath aiPath;
    public RangeSensor rangeSensor;
    public float abortTime;

    private float _startTime;

    private bool TookTooLong => TimeElapsed - _startTime > abortTime;

    public override bool PrePerform() {
        aiPath.canMove = true;
        _startTime = TimeElapsed;
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
        AgentData.Agent.States.SetState(heroKeepingDistanceState.Value, 1);
    }
}