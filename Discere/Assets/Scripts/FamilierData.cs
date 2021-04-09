using UnityEngine;

[CreateAssetMenu(fileName = "FamilierData", menuName = "ScriptableObject/Familier", order = 0)]
public class FamilierData : ScriptableObject
{
    public float fireFrequency;
    public float lifeTime;
    public float orbitRadius;
    public float orbitTime;
    public float radiusEvolutionFactor;
}