namespace SGoap
{
    public class Attack : BasicAction
    {
        public float Range = 2.5f;

        public override float CooldownTime => 2;
        public override float StaggerTime => 1.5f;

        public bool OutOfRange => AgentData.DistanceToTarget > Range;
        public bool AttackIsDone => !AgentData.Animator.GetBool("Attacking") && !Cooldown.Active;

        public override bool PrePerform()
        {
            if (OutOfRange)
                return false;

            AgentData.Agent.transform.LookAt(AgentData.Target);
            AgentData.Animator.SetTrigger("Attack");

            return base.PrePerform();
        }

        public override EActionStatus Perform()
        {
            return AttackIsDone ? EActionStatus.Success : EActionStatus.Running;
        }
    }
}