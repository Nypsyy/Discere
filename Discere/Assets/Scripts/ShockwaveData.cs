using UnityEngine;

[CreateAssetMenu(fileName = "ShockwaveData", menuName = "ScriptableObject/Shockwave")]
public class ShockwaveData : ScriptableObject
{
    public Color color;
    public float lifeTime;
    public float speed;
    public float damage;
}