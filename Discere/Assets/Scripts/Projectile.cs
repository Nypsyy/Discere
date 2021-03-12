using UnityEngine;

[CreateAssetMenu(fileName = "Data",menuName = "ScriptableObject/Projectile")]
public class Projectile : ScriptableObject
{
    public string projectileName;
    public Sprite sprite;
    public float damage;
    public float velocity;
    public float destructionTime;
}