using UnityEngine;
using System.Collections;
using static Utils;

public class Minotaur : MonoBehaviour
{
    // String hashes

    public Health health; // Minotaur's health
    public GameObject hero;

    public Familier familierModel;
    public Bullet bulletModel;
    public GameObject shockwave;
    public Rock rockModel;

    // Minotaur's rages
    public Rage meleeRage;    // Melee rage
    public Rage distanceRage; // Ranged rage
    public Rage magicRage;    // Magic rage

    public GameObject winScreen;

    private LightMeleeAttackAction _lightMeleeAttackAction;
    private Animator _spriteAnimator;       // Sprite animator
    private MinotaurSprite _minotaurSprite; // Sprite manager
    private Rigidbody2D _rigidbody;
    private AudioManager _audio;
    private bool _isDead;
    private bool _isDashing;

    private void Awake() {
        // Get the components
        _spriteAnimator = GetComponentInChildren<Animator>();
        _minotaurSprite = GetComponentInChildren<MinotaurSprite>();
        _lightMeleeAttackAction = GetComponentInChildren<LightMeleeAttackAction>();
        _audio = FindObjectOfType<AudioManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _isDashing = false;
    }

    private void Start() {
        // Boss' rages are increasing constantly
        InvokeRepeating(nameof(UpdateRage), 0, 1);
        //StartCoroutine(_SpawnRocks(30, 0.2f));
        // For testing BulletWall only: StartCoroutine(_test_BulletWall());
    }

    private void Update() {
        // If the boss is dead then do nothing
        if (_isDead) {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }

        // If the boss' health reaches 0
        if (health.value <= 0f) {
            _spriteAnimator.SetTrigger(AnimStrings.IsDead); // Trigger death animation
            _isDead = true;
        }

        _isDashing = _rigidbody.velocity.sqrMagnitude > 10;
    }

    // Increases the boss' rages
    public void UpdateRage() {
        meleeRage.IncreaseRage(0.5f);
        distanceRage.IncreaseRage(0.5f);
        magicRage.IncreaseRage(0.5f);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        HandleCollidingObject(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        HandleCollidingObject(other.gameObject);
    }

    private void HandleCollidingObject(GameObject obj) {
        var rock = obj.GetComponent<Rock>();

        if (rock) {
            if (_isDashing)
                Destroy(obj);
            return;
        }

        if (obj.layer != LayerMask.NameToLayer("HeroProjectile")) return;

        var proj = obj.GetComponent<Projectiles>()?.projectile
            ? obj.GetComponent<Projectiles>()?.projectile
            : obj.GetComponent<MagicProjectile>()?.projectile;

        if (proj is null) return;

        _lightMeleeAttackAction.Cost += 0.05f;
        TakeDamage(proj.damage, proj.style);
    }

    public void TakeDamage(float damage, FightingStyle.Style style) {
        GetComponentInChildren<ShockwaveAttackCost>().Touched();

        if (_minotaurSprite.isBlinking)
            return; // do not apply damage when blinking = invulnerability time

        health.TakeDamage(damage);
        _audio.Play(Sounds.MinotaurHurt);

        switch (style) {
            case FightingStyle.Style.Melee:
                meleeRage.IncreaseRage(damage);
                break;

            case FightingStyle.Style.Range:
                distanceRage.IncreaseRage(damage);
                break;

            case FightingStyle.Style.Magic:
                magicRage.IncreaseRage(damage);
                break;
        }

        var shockwaveSpeed = 6 + 12 * health.progress;
        shockwave.GetComponent<ShockwaveBehavior>().data.speed = shockwaveSpeed;

        StartCoroutine(_minotaurSprite.Blink());
    }

    public void OnHealthEmpty() {
        if (_isDead) return;

        hero.GetComponent<Hero>().Won = true;

        _spriteAnimator.SetBool(AnimStrings.IsDead, true); // Trigger death animation
        _isDead = true;
        Time.timeScale = 0.5f;

        CinemachineEffects.Instance.Zoom(2f, transform);

        Invoke(nameof(_DisplayWinScreen), 1f);
    }

    private void _DisplayWinScreen() {
        if (winScreen != null) {
            winScreen.SetActive(true);
        }

        Time.timeScale = 1f;
        CinemachineEffects.Instance.UnZoom();
    }


    public IEnumerator SpawnFamiliers(int n, float delay) {
        for (var i = 0; i < n; i++) {
            Instantiate(familierModel, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }

        yield return null;
    }

    // angle_width & angle_offset are in degrees
    public void SpawnBulletWall(int nbBullets, float speed, float angleWidth, float angleOffset = 0) {
        angleWidth *= Mathf.Deg2Rad;
        angleOffset *= Mathf.Deg2Rad;
        bulletModel.GetComponent<Bullet>().data.speed = speed;

        // bullets are launched in [-angle_width/2, +angle_width/2] in direction of the player
        var dir = hero.transform.position - transform.position;
        var baseAngle = Mathf.Atan2(dir.y, dir.x) + angleOffset - angleWidth / 2;

        for (var i = 0; i < nbBullets; ++i) {
            var angle = baseAngle + i * angleWidth / (nbBullets - 1);
            Instantiate(bulletModel,
                        transform.position,
                        Quaternion.FromToRotation(
                            Vector3.right,
                            dir.normalized + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized));
        }
    }

    private IEnumerator _SpawnRocks(int n, float delayBetween) {
        for (var i = 0; i < n; ++i) {
            rockModel.Create((Vector2) hero.transform.position + 15 * Random.insideUnitCircle);
            yield return new WaitForSeconds(delayBetween);
        }

        yield return null;
    }
}