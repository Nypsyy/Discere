using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletData data;
    private Vector2 _direction = Vector2.zero;
    private float _speed = 0;
    private float _lifetime = 30;
    
    public void Create(Vector2 position, float radius, Vector2 direction, float speed, float lifetime = 30) {
        GameObject obj = Instantiate(this.gameObject);
        obj.transform.localScale = new Vector3(radius,radius,1);
        obj.transform.position = position;
        obj.SetActive(true);
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet._direction = direction.normalized;
        bullet._speed = speed;
        obj.GetComponent<SpriteRenderer>().color = data.color;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        _lifetime -= Time.deltaTime;
        if (_lifetime < 0)
            Destroy(this.gameObject);
    }
    
    void FixedUpdate()
    {
        transform.position += (Vector3)_direction * _speed * Time.fixedDeltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            Destroy(this.gameObject);
            return;
        }
        Hero hero = collision.gameObject.GetComponent<Hero>();
        if (hero != null && hero.anim.CurrentMode != HeroAnim.Mode.Jump) {
            hero.TakeDamage(data.damage);
            Destroy(this.gameObject);
            return;
        }
    }
}
