using SGoap;
using UnityEngine;

public class ShockwaveAttackCost : BasicCostEvaluator
{
    private float _timesAttacked;
    private float _timesUsed;

    public void Touched() {
        _timesAttacked += 0.2f;
    }

    public void Used() {
        _timesUsed += 1f;
        _timesAttacked = 0f;
    }

    public override float Evaluate(IContext context) {
        return 2.5f + Random.value + _timesUsed - _timesAttacked;
    }
}