using SGoap;
using UnityEngine;

public class DashToMeleeCost : BasicCostEvaluator
{
    public RangeSensor rangeSensor;
    private float _timesUsed;

    public void Used() {
        _timesUsed += 0.1f;
    }

    // f(x) = 4/(melee - dist) * X + [4-5]
    public override float Evaluate(IContext context) {
        var offset = 3 + Random.value;
        var factor = 1.5f / (rangeSensor.MeleeRange - rangeSensor.DistanceRange);
        var dashableDistance = AgentData.DistanceToTarget - rangeSensor.MeleeRange;
        
        return factor * dashableDistance + offset + _timesUsed;
    }
}