using SGoap;
using UnityEngine;

public class TargetSensor : Sensor
{
    public Transform target;
    private bool HasTarget => target != null;

    public override void OnAwake() {
        if (HasTarget) {
            AgentData.Target = target.transform;
            AgentData.Agent.States.AddState("HasTarget", 1);
        }
        else
            AgentData.Agent.States.RemoveState("HasTarget");
    }
}