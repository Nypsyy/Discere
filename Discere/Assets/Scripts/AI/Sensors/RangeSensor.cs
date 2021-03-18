using SGoap;
using UnityEngine;

public class RangeSensor : Sensor
{
    [Header("Values")] [SerializeField]
    private float distanceRange = 30f;

    [SerializeField]
    private float meleeRange = 5f;

    [Header("References")] [SerializeField]
    private StringReference inMeleeRangeState;

    [SerializeField]
    private StringReference inDistanceRangeState;

    public bool InMeleeRange => AgentData.DistanceToTarget <= meleeRange;

    public bool InDistanceRange => AgentData.DistanceToTarget <= distanceRange &&
                                   AgentData.DistanceToTarget > meleeRange;

    public bool OutOfRange => AgentData.DistanceToTarget > distanceRange;

    public override void OnAwake() {
    }

    private void Update() {
        if (!Agent.States.HasState("HasTarget")) return;

        if (InDistanceRange)
            Agent.States.SetState(inDistanceRangeState.Value, 1);
        else
            Agent.States.RemoveState(inDistanceRangeState.Value);

        if (InMeleeRange)
            Agent.States.SetState(inMeleeRangeState.Value, 1);
        else
            Agent.States.RemoveState(inMeleeRangeState.Value);
    }
}