using System.Collections.Generic;
using UnityEngine;

public class Familier : MonoBehaviour
{
    public List<FamilierData> familierDatas;
    public Bullet bulletModel;
    public SpriteRenderer spriteRenderer;

    private Vector3 TargetPos => _hero.gameObject.transform.position;
    private Vector3 FireDirection => TargetPos - transform.position;
    private float RadPerSecond => _orbitTime == 0 ? 0 : _radFactor * 2 * Mathf.PI / _orbitTime;

    private Hero _hero; // Target

    // Minotaur's rages
    private Rage _distanceRage;
    private Rage _magicRage;

    private int _index;                   // Index for the familier data
    private float _time;                  // Time elapsed since creation
    private float _radFactor;             // Radian factor for familier movement
    private float _fireDelay;             // Bullet instantiation delay
    private float _lifeTime;              // Life time of a familier
    private float _orbitRadius;           // Orbit radius for familier movement
    private float _orbitTime;             // Time a familier takes to make a full circle
    private float _radiusEvolutionFactor; // Radius changes during familier movement
    private float _moveFactor;            // Movement towards the hero


    private void Fire() {
        Instantiate(bulletModel, transform.position, Quaternion.FromToRotation(Vector3.right, FireDirection.normalized));
    }

    private void Awake() {
        _hero = FindObjectOfType<Hero>();
        _distanceRage = FindObjectOfType<Minotaur>().distanceRage;
        _magicRage = FindObjectOfType<Minotaur>().magicRage;

        // Choose familier level based on distance / magic rage
        _index = Mathf.Max(_distanceRage.FillRatio, _magicRage.FillRatio) / 34;
        Debug.Log(_index);

        _fireDelay = 1 / familierDatas[_index].fireFrequency;
        _lifeTime = familierDatas[_index].lifeTime;
        _orbitRadius = familierDatas[_index].orbitRadius;
        _orbitTime = familierDatas[_index].orbitTime;
        _radiusEvolutionFactor = familierDatas[_index].radiusEvolutionFactor;
    }

    private void Fade() {
        var progress = 0f;

        while (progress < 1) {
            _time += Time.deltaTime;
            progress = Mathf.InverseLerp(0f, 3f, _time - _lifeTime);
            spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, progress));
        }

        Destroy(gameObject);
    }

    private void Start() {
        Invoke(nameof(Fade), _lifeTime);
        InvokeRepeating(nameof(Fire), _fireDelay, _fireDelay);
        FindObjectOfType<AudioManager>().Play("FamilierSpawn");
    }

    private void Update() {
        spriteRenderer.flipX = FireDirection.x < 0;

        _radFactor = Mathf.Clamp01(1f - 0.1f * (FireDirection.magnitude - _orbitRadius));
        _moveFactor = Mathf.Abs(FireDirection.magnitude - _orbitRadius);

        var radius = Mathf.MoveTowards(FireDirection.magnitude, _orbitRadius, _radiusEvolutionFactor * _moveFactor * Time.deltaTime);
        var angle = Mathf.Atan2(-FireDirection.y, -FireDirection.x);

        angle -= RadPerSecond * Time.deltaTime;
        transform.position = TargetPos + radius * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
    }
}