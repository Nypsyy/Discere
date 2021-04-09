using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    public GameObject shadowParent;
    public ParticleSystem particles;

    private float _speed = -1;
    private Vector2 _targetPos;
    private new AudioManager audio;

    public void Create(Vector2 targetPos) {
        GameObject obj = Instantiate(this.gameObject);
        Rock rock = obj.GetComponent<Rock>();
        rock._targetPos = targetPos;
        rock._speed = -10;
        
        obj.transform.position = new Vector3(targetPos.x, targetPos.y + 100, 0);
        obj.SetActive(true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DestroyMe() {
        StartCoroutine(DestroyCoroutine());
    }
    
    private IEnumerator DestroyCoroutine() {
        particles.Play();
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        shadowParent.SetActive(false);
        AstarPath.active.Scan();
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
        yield return null;
    }
    
    void FixedUpdate()
    {
        if (_speed == 0) return; // already landed
        // acceleration from gravity
        _speed -= 9.81f * Time.fixedDeltaTime;
        
        float newY = transform.position.y + _speed * Time.fixedDeltaTime;
        if (newY < _targetPos.y) {
            newY = _targetPos.y; _speed = 0;
            
            Collider2D collider = GetComponent<CapsuleCollider2D>();
            collider.enabled = true;
            Collider2D[] colliders = new Collider2D[10];
            int nb_colliders = collider.OverlapCollider(new ContactFilter2D().NoFilter(), colliders);
            for (int i = 0; i < nb_colliders; ++i) {
                Hero hero = colliders[i].gameObject.GetComponent<Hero>();
                if (hero) {
                    hero.TakeDamage(10);
                    break;
                }
            }
            
            particles.Play();
            AstarPath.active.Scan();
            CinemachineEffects.Instance.Shake(2f, 0.2f);
            audio.Play("RockFall");
        }
        transform.position = new Vector2(transform.position.x, newY);
        
        float progress = 1 - (newY -_targetPos.y) / 100;
        shadowParent.transform.localScale = new Vector2(progress, progress);
        shadowParent.transform.position = _targetPos;
        
    }
    
}
