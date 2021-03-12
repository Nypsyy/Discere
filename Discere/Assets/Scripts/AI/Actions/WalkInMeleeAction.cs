using Pathfinding;
using SGoap;

public class WalkInMeleeAction : BasicAction
{
    public AIPath aiPath;
    public RangeSensor rangeSensor;
    private float _startTime;

    private bool TookToolong => TimeElapsed - _startTime > 10;

    public override bool PrePerform() {
        aiPath.canMove = true;
        _startTime = TimeElapsed;
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        if (TookToolong) return EActionStatus.Failed;
        
        return rangeSensor.InMeleeRange ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        aiPath.canMove = false;
        return base.PostPerform();
    }
}