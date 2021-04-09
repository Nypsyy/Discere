using UnityEngine;

[CreateAssetMenu(fileName = "LaserData", menuName = "ScriptableObject/Laser", order = 3)]
public class LaserData : ScriptableObject
{
    public float damage;
    public float chargeTime;
    public float beamDuration;
    public float readyRayWidth;
    public Color readyRayColor;
}