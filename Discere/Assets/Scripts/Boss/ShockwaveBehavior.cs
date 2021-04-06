using System.Collections;
using UnityEngine;

public class ShockwaveBehavior : MonoBehaviour
{
    public ShockwaveData data;
    private bool _touched;

    private void Start() {
        GetComponent<SpriteRenderer>().color = data.color;
        StartCoroutine(Exists(data.lifeTime));
    }

    private void Update() {
        transform.localScale += Vector3.one * (data.speed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player") || _touched ||
            other.gameObject.GetComponent<Hero>().anim.CurrentMode == HeroAnim.Mode.Jump)
            return;

        FindObjectOfType<Hero>().TakeDamage(data.damage);
        _touched = true;
    }

    private IEnumerator Exists(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}