using SGoap;
using UnityEngine;

public class LightMeleeAttackCost : BasicCostEvaluator
{
    private float _timesUsed;

    public void Failed() {
        _timesUsed += .25f;
    }
    
    public void Used() {
        _timesUsed += .1f;
    }

    // f(x) = [1-2] + used
    public override float Evaluate(IContext context) {
        return 1 + _timesUsed + Random.value;
    }
}