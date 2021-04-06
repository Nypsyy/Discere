using SGoap;
using UnityEngine;

public class ProjectileWallAttackCost : BasicCostEvaluator
{
    public RangeSensor rangeSensor;

    private float _timesUsed;

    public void Used() {
        _timesUsed += 0.1f;
    }

    // f(x) = 
    public override float Evaluate(IContext context) {
        var offset = 1 + Random.value;
        var factor = 1.2f / (rangeSensor.MeleeRange - rangeSensor.DistanceRange);
        var distance = AgentData.DistanceToTarget - rangeSensor.MeleeRange;
        
        return factor * distance + offset + _timesUsed;
    }
}