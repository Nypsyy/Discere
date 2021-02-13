using UnityEngine;

namespace SGoap
{
    /// <summary>
    /// A very specific evaluator for picking up objects.
    /// </summary>
    public class PickUpUsableEvaluator : UsableEvaluator
    {
        public override bool Evaluate(IContext context)
        {
            if (AgentData?.Target == null)
                return false;

            var action = context.Get<IPickUp>();
            var closestGrenade = action.GetClosest();
            if (closestGrenade == null)
                return false;

            var distanceToGrenade = Vector3.Distance(AgentData.Position, closestGrenade.position);

            if (AgentData.DistanceToTarget <= 2 && distanceToGrenade > 0.5f)
                return false;

            if (action.OtherAgentAlsoPicking())
                return false;

            return true;
        }
    }
}
