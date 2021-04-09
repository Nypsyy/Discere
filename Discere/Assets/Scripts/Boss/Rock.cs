using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Rock : MonoBehaviour
{
    public GameObject shadowParent;
    public ParticleSystem particles;
    public float fallSpeed = 10;

    public UnityEvent onLandEvent;

    private Vector2 _targetPos;
    private bool _landed;
    private new AudioManager audio;

    public void Landed() {
        _landed = true;
        fallSpeed = 0;

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
    }

    private void Awake() {
        onLandEvent ??= new UnityEvent();

        _targetPos = transform.position;
        transform.position += Vector3.up * 100;
        audio = FindObjectOfType<AudioManager>();
    }

    private void Start() {
        GetComponent<CapsuleCollider2D>().enabled = false;
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
        Destroy(gameObject);
        yield return null;
    }

    private void FixedUpdate() {
        if (_landed) return; // already landed
        // acceleration from gravity
        fallSpeed += -Physics2D.gravity.y * Time.fixedDeltaTime;

        var newY = transform.position.y - fallSpeed * Time.fixedDeltaTime;

        if (newY < _targetPos.y) {
            newY = _targetPos.y;
            onLandEvent.Invoke();
            audio.Play("RockFall");
        }

        transform.position = new Vector3(transform.position.x, newY);

        var progress = 1 - (newY - _targetPos.y) / 100;
        shadowParent.transform.localScale = new Vector2(progress, progress);
        shadowParent.transform.position = _targetPos;
    }
}