using SGoap;

public class RangeSensor : Sensor
{
    public TargetSensor targetSensor;

    public float rangedRange = 10f;
    public float meleeRange = 5f;

    public bool InMeleeRange => targetSensor.HasTarget &&
                                targetSensor.DistanceToTarget <= meleeRange;

    public bool InRangedRange => targetSensor.HasTarget &&
                                 targetSensor.DistanceToTarget <= rangedRange &&
                                 targetSensor.DistanceToTarget > meleeRange;

    public override void OnAwake() {
    }

    private void Update() {
        if (!targetSensor.HasTarget) return;

        if (InRangedRange)
            Agent.States.SetState("InRangedRange", 1);
        else
            Agent.States.RemoveState("InRangedRange");
        
        if (InMeleeRange)
            Agent.States.SetState("InMeleeRange", 1);
        else
            Agent.States.RemoveState("InMeleeRange");
    }
}