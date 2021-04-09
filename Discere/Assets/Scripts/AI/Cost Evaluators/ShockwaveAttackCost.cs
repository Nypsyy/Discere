using SGoap;
using UnityEngine;

public class ShockwaveAttackCost : BasicCostEvaluator
{
    protected float TimesAttacked;
    private float _timesUsed;

    public virtual void Touched() {
        TimesAttacked += .2f;
    }

    public void Used() {
        _timesUsed += 1f;
        TimesAttacked = 0f;
    }

    public override float Evaluate(IContext context) {
        return 2.5f + Random.value + _timesUsed - TimesAttacked;
    }
}