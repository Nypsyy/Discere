using SGoap;
using UnityEngine;


public class WorldSensor : Sensor
{
    [SerializeField]
    private StringReference enoughSpaceForRocksState;

    private float rockNb;

    public override void OnAwake() {
    }

    private void Update() {
        rockNb = FindObjectsOfType<Rock>().Length;
        Agent.States.SetState(enoughSpaceForRocksState.Value, rockNb);
    }
}