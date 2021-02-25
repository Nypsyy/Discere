using SGoap;

public class GoToMeleeAction : Action
{
    public RangeSensor rangeSensor;

    public override bool PrePerform() {
        throw new System.NotImplementedException();
    }

    public override EActionStatus Perform() {
        return rangeSensor.InMeleeRange ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        throw new System.NotImplementedException();
    }
}