using UnityEngine;

namespace SGoap
{
    public class Chase : NavMeshAction
    {
        public bool UseAgentTarget = true;

        public override bool PrePerform()
        {
            if (UseAgentTarget)
                Target = AgentData.Target.gameObject;

            Agent.isStopped = false;
            AgentData.Animator.SetFloat("MoveVelocity", 0.3f);
            return base.PrePerform();
        }

        public override bool PostPerform()
        {
            AgentData.Animator.SetFloat("MoveVelocity", 0);
            Agent.isStopped = true;

            return base.PostPerform();
        }

        public override void OnFailed()
        {
            Agent.isStopped = true;
            base.OnFailed();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, StopDistance);
        }
    }
}