using SGoap;
using UnityEngine;
using static Utils;

public class DashToMeleeAction : BasicAction
{
    public Rigidbody2D rb;
    private DashToMeleeCost cost;

    private bool DashDone => !AgentData.Animator.GetBool(AnimationVariables.PrepareDash) && rb.velocity.magnitude < 3;

    private void Awake() {
        cost = GetComponentInChildren<DashToMeleeCost>();
    }

    public override bool PrePerform() {
        AgentData.Animator.SetTrigger(AnimationVariables.BossDash);
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return DashDone ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        cost.Used();
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        return base.PostPerform();
    }
}