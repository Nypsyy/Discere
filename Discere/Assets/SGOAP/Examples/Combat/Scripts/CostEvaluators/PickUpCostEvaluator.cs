using UnityEngine;

namespace SGoap
{
    public class PickUpCostEvaluator : BasicCostEvaluator
    {
        public override float Evaluate(IContext context)
        {
            var pickUpAction = context.Get<IPickUp>();
            var closestGrenade = pickUpAction.GetClosest();
            var distanceToGrenade = Vector3.Distance(AgentData.Position, closestGrenade.position);
            var cost = distanceToGrenade - 4;

            if(pickUpAction.Action.States != null)
                if (pickUpAction.Action.States.HasState(pickUpAction.StateReference.Value))
                    cost += 1;

            if (AgentData.DistanceToTarget <= 2 && distanceToGrenade > 0.5f)
                cost = 20;

            return cost;
        }
    }
}