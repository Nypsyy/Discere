using SGoap;

public class DashToMeleeUsable : UsableEvaluator
{
    public RangeSensor rangeSensor;
    public override bool Evaluate(IContext context) {
        return !rangeSensor.InMeleeRange;
    }
}