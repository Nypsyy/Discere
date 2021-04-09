using UnityEngine;

[CreateAssetMenu(fileName = "Data",menuName = "ScriptableObject/BulletData")]
public class BulletData : ScriptableObject
{
    public Color color;
    public float damage;
    public float speed;
    public float lifeTime;
}
