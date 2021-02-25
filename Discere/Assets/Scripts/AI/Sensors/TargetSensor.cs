using SGoap;
using UnityEngine;

public class TargetSensor : Sensor
{
    public Transform target;
    public float DistanceToTarget => Vector2.Distance(Agent.transform.position, target.position);
    public bool HasTarget => target != null;

    public override void OnAwake() {
        if (!HasTarget) return;
        Agent.States.AddState("HasTarget", 1);
    }
}