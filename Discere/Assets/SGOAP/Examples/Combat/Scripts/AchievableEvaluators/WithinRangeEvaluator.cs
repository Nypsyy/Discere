namespace SGoap
{
    public class WithinRangeEvaluator : UsableEvaluator
    {
        public float Range = 7;

        public override bool Evaluate(IContext context)
        {
            if (AgentData?.Target == null)
                return false;

            return AgentData.DistanceToTarget <= Range;
        }
    }
}