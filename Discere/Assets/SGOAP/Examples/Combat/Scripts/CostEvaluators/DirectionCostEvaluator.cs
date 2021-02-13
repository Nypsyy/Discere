using UnityEngine;

namespace SGoap
{
    public class DirectionCostEvaluator : BasicCostEvaluator
    {
        public float Min = 0.1f;
        public float Max = 5.0f;

        public override float Evaluate(IContext context)
        {
            if (AgentData?.Target == null)
                return Max;

            var dot = (Vector3.Dot(AgentData.Target.forward, AgentData.DirectionToTarget) + 1) / 2;
            return Mathf.Lerp(Max, Min, dot);
        }
    }
}