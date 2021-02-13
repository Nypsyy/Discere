using UnityEngine;

namespace SGoap
{
    public class Dash : BasicAction
    {
        public bool Forward = true;

        public override float CooldownTime => Random.Range(5, 20);
        public override float StaggerTime => 0.2f;

        public Vector3 DashToPosition()
        {
            var dir = Forward ? AgentData.DirectionToTarget : -AgentData.DirectionToTarget;
            var dashToPosition = AgentData.Agent.transform.position + dir * 3;
            return dashToPosition;
        }

        public override bool PrePerform()
        {
            AgentData.EffectsController.PlayDash(0.2f);

            AgentData.Agent.transform.forward = AgentData.DirectionToTarget;
            AgentData.Agent.transform.GoTo(DashToPosition(), 0.1F);

            AgentData.Animator.SetTrigger("Dash");

            return base.PrePerform();
        }
    }
}