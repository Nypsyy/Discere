using UnityEngine;

namespace SGoap
{
    public class KeepDistance : BasicAction
    {
        public override bool CanAbort() => TimeElapsed > 3;

        public override float CooldownTime => Random.Range(4, 10);
        public override float StaggerTime => 0;

        public override bool PrePerform()
        {
            AgentData.Animator.SetBool("Guarding", true);
            return base.PrePerform();
        }

        public override bool PostPerform()
        {
            AgentData.Animator.SetFloat("MoveVelocity", 0.0f);
            AgentData.Animator.SetBool("Guarding", false);

            return base.PostPerform();
        }

        public override EActionStatus Perform()
        {
            AgentData.Agent.transform.forward = Vector3.Lerp(AgentData.Agent.transform.forward, AgentData.DirectionToTarget, 4 * Time.deltaTime);

            if (AgentData.DistanceToTarget <= 5)
            {
                AgentData.Position += Time.deltaTime * -AgentData.DirectionToTarget;
                AgentData.Animator.SetFloat("MoveVelocity", 0.4f);
            }

            return CanAbort() ? EActionStatus.Success : EActionStatus.Running;
        }

        public override void OnFailed()
        {
            AgentData.Animator.SetFloat("MoveVelocity", 0.0f);
            AgentData.Animator.SetBool("Guarding", false);
            base.OnFailed();
        }
    }
}