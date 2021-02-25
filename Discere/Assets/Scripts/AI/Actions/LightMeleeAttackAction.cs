using SGoap;
using UnityEngine;

public class LightMeleeAttackAction : Action
{
    public override bool PrePerform() {
        throw new System.NotImplementedException();
    }

    public override EActionStatus Perform() {
        throw new System.NotImplementedException();
    }

    public override bool PostPerform() {
        throw new System.NotImplementedException();
    }

    public override void DynamicallyEvaluateCost() {
        Cost = Random.value;
    }
}