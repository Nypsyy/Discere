namespace SGoap
{
    public class DistanceCostEvaluator : BasicCostEvaluator
    {
        public override float Evaluate(IContext context)
        {
            return AgentData.DistanceToTarget > 2 ? 0.1f : 20;
        }
    }
}