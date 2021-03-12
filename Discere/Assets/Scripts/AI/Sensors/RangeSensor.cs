using SGoap;
using UnityEngine;

public class RangeSensor : Sensor
{
    [Header("Values")]
    public float rangedRange = 30f;
    public float meleeRange = 5f;

    [Header("References")]
    public StringReference inMeleeRangeState;
    public StringReference inRangedRangeState;

    public bool InMeleeRange => AgentData.DistanceToTarget <= meleeRange;

    private bool InRangedRange => AgentData.DistanceToTarget <= rangedRange &&
                                  AgentData.DistanceToTarget > meleeRange;

    public bool OutOfRange => AgentData.DistanceToTarget > rangedRange;

    public override void OnAwake() {
    }

    private void Update() {
        if (!Agent.States.HasState("HasTarget")) return;

        if (InRangedRange)
            Agent.States.SetState(inRangedRangeState.Value, 1);
        else
            Agent.States.RemoveState(inRangedRangeState.Value);

        if (InMeleeRange)
            Agent.States.SetState(inMeleeRangeState.Value, 1);
        else
            Agent.States.RemoveState(inMeleeRangeState.Value);
    }
}