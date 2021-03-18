using SGoap;
using UnityEngine;

public class DashToMeleeAction : BasicAction
{
    public float dashSpeed;
    public RangeSensor rangeSensor;

    private Rigidbody2D _rb;

    public override bool IsUsable() => !rangeSensor.OutOfRange;

    public override bool PrePerform() {
        _rb = AgentData.Agent.gameObject.GetComponent<Rigidbody2D>();
        _rb.isKinematic = false;

        return _rb;
    }

    public override EActionStatus Perform() {
        _rb.AddForce(AgentData.DirectionToTarget * dashSpeed, ForceMode2D.Impulse);
        return EActionStatus.Success;
    }

    public override void DynamicallyEvaluateCost() {
        // DASH > WALK if DISTANCE > 10
        Cost = AgentData.DistanceToTarget < 15 ? 30 : 10 / AgentData.DistanceToTarget;
        Debug.Log(Cost);
    }
}