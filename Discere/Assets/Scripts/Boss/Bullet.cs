using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletData data;

    private void Start() {
        GetComponent<SpriteRenderer>().color = data.color;
        StartCoroutine(Exists(data.lifeTime));
    }

    private void FixedUpdate() {
        transform.position += transform.right.normalized * (data.speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            Destroy(gameObject);
            return;
        }

        var hero = collision.gameObject.GetComponent<Hero>();
        if (hero != null && hero.anim.CurrentMode != HeroAnim.Mode.Jump) {
            hero.TakeDamage(data.damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator Exists(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}