using SGoap;
using UnityEngine;

public class HeavyMeleeAttackAction : Action
{
    public override bool PrePerform() {
        throw new System.NotImplementedException();
    }
    
    

    public override bool PostPerform() {
        throw new System.NotImplementedException();
    }

    public override void DynamicallyEvaluateCost() {
        Cost = Random.value;
    }
}
