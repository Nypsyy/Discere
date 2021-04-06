using Pathfinding;
using SGoap;

public class WalkToDistanceAction : BasicAction
{
    public AIPath aiPath;
    public RangeSensor rangeSensor;
    public float abortTime;
    
    private bool TookTooLong => TimeElapsed > abortTime;

    public override bool PrePerform() {
        aiPath.canMove = true;
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        if (TookTooLong)
            return EActionStatus.Failed;

        return rangeSensor.InDistanceRange ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        aiPath.canMove = false;
        return base.PostPerform();
    }

    public override void OnFailed() {
        aiPath.canMove = false;
    }
}