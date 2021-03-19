using SGoap;

public class WalkToDistanceUsable : UsableEvaluator
{
    public RangeSensor rangeSensor;

    public override bool Evaluate(IContext context) {
        return rangeSensor.OutOfRange;
    }
}