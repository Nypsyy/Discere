using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData",menuName = "ScriptableObject/Projectile")]
public class ProjectileData : ScriptableObject
{
    public string projectileName;
    public Sprite sprite;
    public float damage;
    public float velocity;
    public float destructionTime;
    public FightingStyle.Style style;
}
