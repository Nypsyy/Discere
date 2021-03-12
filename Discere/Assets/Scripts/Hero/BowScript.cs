using System.Collections;
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
    private float CurrentAngle => Vector2.SignedAngle(Vector2.right, _hero.ShootingDirection) * Mathf.Deg2Rad;

    private void Awake() {
        _hero = GetComponentInParent<Hero>();
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        arrowSprite.sprite = arrowSprites[0];
        gameObject.SetActive(false);
    }

    private void Update() {
        UpdatePosition();
    }

    public void ChargeShot() {
        _arrowIndex = -1;
        _animator.SetTrigger(AnimationVariables.ChargingShot);
    }

    public void Shoot() {
        _animator.SetTrigger(AnimationVariables.FiringShot);
        Instantiate(arrows[_arrowIndex], transform.position, Quaternion.identity);
    }

    private void UpdatePosition() {
        transform.position = transform.parent.position + new Vector3(Mathf.Cos(CurrentAngle), Mathf.Sin(CurrentAngle)) * posRadius;
        transform.right = _hero.ShootingDirection;
    }

    // Used by the animation
    public void UpgradeArrow() {
        if (_arrowIndex < arrows.Length)
            _arrowIndex++;

        arrowSprite.sprite = arrowSprites[_arrowIndex];
    }
}