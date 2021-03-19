using SGoap;
using UnityEngine;

public class DashToMeleeCost : BasicCostEvaluator
{
    public RangeSensor rangeSensor;
    private float _timesUsed;

    public void Used() {
        _timesUsed += 0.1f;
    }

    public override float Evaluate(IContext context) {
        var offset = 3 + Random.value;
        var factor = 1.5f / (rangeSensor.MeleeRange - rangeSensor.DistanceRange);
        var dashableDistance = AgentData.DistanceToTarget - rangeSensor.MeleeRange;

        Debug.Log(factor * dashableDistance + offset + _timesUsed);

        return factor * dashableDistance + offset + _timesUsed;
    }
}