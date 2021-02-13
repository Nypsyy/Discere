using UnityEngine;

namespace SGoap
{
    public class RunAround : BasicAction
    {
        private Vector3 _lastPosition;
        private float _minimumElapsedTime = 5;
        private int _dir;

        public override float CooldownTime => 3;
        public override float StaggerTime => 0;

        public override bool CanAbort() => AgentData.DistanceToTarget <= 1.0f;

        public override bool PrePerform()
        {
            _minimumElapsedTime = Random.Range(3, 8);
            _dir = Random.Range(0, 2) == 0 ? 1 : -1;
            return base.PrePerform();
        }

        public override EActionStatus Perform()
        {
            var agentTransform = AgentData.Agent.transform;
            var target = AgentData.Target;

            AgentData.Animator.SetFloat("MoveVelocity", 0.6f);
            _lastPosition = AgentData.Position;
            
            agentTransform.RotateAround(target.position, Vector3.up, (100 * Time.deltaTime / AgentData.DistanceToTarget) * _dir);
            
            agentTransform.position += AgentData.DirectionToTarget * Time.deltaTime * 1;

            var runDirection = AgentData.Position - _lastPosition;
            runDirection.Normalize();
            
            agentTransform.forward = Vector3.Lerp(agentTransform.forward, runDirection, 2 * Time.deltaTime);
            agentTransform.forward =
                Vector3.Lerp(agentTransform.forward, AgentData.DirectionToTarget, 0.5f * Time.deltaTime);

            if (Physics.Raycast(agentTransform.position, runDirection, 1))
                return EActionStatus.Failed;

            if (TimeElapsed > _minimumElapsedTime)
                return EActionStatus.Success;

            return EActionStatus.Running;
        }

        public override void OnFailed()
        {
            Cooldown.Run(3);
            base.OnFailed();
        }
    }
}