using UnityEngine;
using static Utils;

public class BowScript : MonoBehaviour
{
    public GameObject[] arrows;
    public Sprite[] arrowSprites;
    public SpriteRenderer arrowSprite;
    public float posRadius;

    private Hero _hero;
    private Animator _animator;
    private int _arrowIndex;

    private float CurrentAngle =>
        Vector2.SignedAngle(Vector2.right, _hero.ShootingDirection != Vector2.zero ? _hero.ShootingDirection : Vector2.right) *
        Mathf.Deg2Rad;

    private AudioManager _audio;

    private void Awake() {
        _hero = GetComponentInParent<Hero>();
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        arrowSprite.sprite = arrowSprites[0];
        _audio = FindObjectOfType<AudioManager>();
        gameObject.SetActive(false);
    }

    private void Update() {
        transform.position = _hero.gameObject.transform.position +
                             new Vector3(Mathf.Cos(CurrentAngle), Mathf.Sin(CurrentAngle)) * posRadius;
        transform.right = _hero.ShootingDirection;
    }

    public void ChargeShot() {
        _arrowIndex = -1;
        _animator.SetTrigger(AnimStrings.ChargingShot);
        _audio.Play(Sounds.BowCharge);
    }

    public void Shoot() {
        _animator.SetTrigger(AnimStrings.FiringShot);
        if (_arrowIndex >= 0 && _arrowIndex < arrows.Length)
            Instantiate(arrows[_arrowIndex], transform.position, Quaternion.identity);
        _audio.Play(Sounds.BowShoot + (_arrowIndex + 1));
    }

    // Used by the animation
    public void UpgradeArrow() {
        if (_arrowIndex < arrows.Length) {
            _arrowIndex++;
            _audio.Play(Sounds.BowChange, pitchMultiplier: 0.5f * _arrowIndex);
        }

        arrowSprite.sprite = arrowSprites[_arrowIndex];
    }
}