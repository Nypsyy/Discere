using UnityEngine;
using SGoap;

public class WalkToMeleeCost : CostEvaluator
{
    public override float Evaluate(IContext context) {
        return 1 + Random.value;
    }
}