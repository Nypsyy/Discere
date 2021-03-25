using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familier : MonoBehaviour
{
    public Bullet bulletModel;

    public GameObject orbitEntity; // entity to target
    public SpriteRenderer spriteRenderer;
    public float orbitRadius = 5; // radius of orbit
    public float orbitTime = 5; // number of seconds to do a complete orbit
    public float lifetime = 10; // number of seconds until Familier is destroyed
    public float speed = 10;
    public float radiusEvolutionFactor = 3;
    public float spawnDelayMean = 0;
    
    private float _time = 0;
    private float _radFactor = 0;
    private float _radPerSecond => _radFactor * 2 * Mathf.PI / orbitTime;
    private float _spawnDelay = 0;
    
    public Familier Create(GameObject orbitEntity, Vector2 position, float spawnDelay = 1, float lifetime = 30) {
        GameObject obj = Instantiate(this.gameObject);
        obj.transform.position = position;
        obj.SetActive(true);
        Familier familier = obj.GetComponent<Familier>();
        familier.orbitEntity = orbitEntity;
        familier.lifetime = lifetime;
        familier.spawnDelayMean = spawnDelay;
        
        familier._spawnDelay = spawnDelayMean;
        return familier;
    }
    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 entityPos = orbitEntity.transform.position;
        Vector2 targetDir = entityPos - (Vector2)transform.position;
        spriteRenderer.flipX = (targetDir.x < 0);
        
        _radFactor = Mathf.Clamp01(1f - 0.1f*(targetDir.magnitude - orbitRadius));
        
        float radius = Mathf.MoveTowards(targetDir.magnitude, orbitRadius, radiusEvolutionFactor * Time.deltaTime);
        float angle = Mathf.Atan2(-targetDir.y, -targetDir.x);
        angle -= _radPerSecond * Time.deltaTime;
        transform.position = entityPos + radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        
        _time += Time.deltaTime;
        if (_time > lifetime) {
            // fading away
            float progress = Mathf.InverseLerp(0f, 3f, _time - lifetime);
            spriteRenderer.color = new Color(1,1,1,Mathf.Lerp(1f, 0f, progress));
            if (progress >= 1) {
                Destroy(this.gameObject);
                return;
            }
        } else {
            _spawnDelay -= Time.deltaTime;
            if (_spawnDelay < 0) {
                bulletModel.Create(transform.position, 0.6f, targetDir, 5);
                _spawnDelay = 1;
            }
        }
    }
    
}
