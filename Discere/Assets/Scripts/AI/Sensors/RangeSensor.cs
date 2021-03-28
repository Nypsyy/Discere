using System;
using SGoap;
using UnityEngine;

public class RangeSensor : Sensor
{
    public float DistanceRange => 13f;

    public float MeleeRange => 2.3f;

    [Header("References")] [SerializeField]
    private StringReference inMeleeRangeState;

    [SerializeField]
    private StringReference inDistanceRangeState;

    [SerializeField]
    private StringReference outOfRangeState;

    private float DistanceToTarget => Vector2.Distance(AgentData.Target.position, transform.position);

    public bool InMeleeRange => DistanceToTarget <= MeleeRange;

    public bool InDistanceRange => DistanceToTarget <= DistanceRange &&
                                   DistanceToTarget > MeleeRange;

    public bool OutOfRange => DistanceToTarget > DistanceRange;

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

        if (OutOfRange)
            Agent.States.SetState(outOfRangeState.Value, 1);
        else
            Agent.States.RemoveState(outOfRangeState.Value);
    }

    private void OnDrawGizmos() {
           Gizmos.DrawSphere(transform.position,MeleeRange);
    }
}