using UnityEngine;
using SGoap;

public class CloseWanderCost : BasicCostEvaluator
{
    public override float Evaluate(IContext context) {
        return  Random.Range(0, AgentData.DistanceToTarget) < 2.5f ? 0 : 2;
    }
}