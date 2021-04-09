using Rewired;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using static Utils;

public class Hero : MonoBehaviour
{
    #region EDITOR
    
    [Header("General")]
    public float speed;

    public float slowFactor;
    public HeroAnim anim;
    public Material heroMaterial;

    [Header("Melee")]
    public HeroSword sword;

    [Header("Range")]
    public GameObject daggerPrefab;

    public float attackAnimSpeedFactor;

    [Header("Magic")]
    public GameObject magicBallPrefab;

    public GameObject magicLaserPrefab;
    public float magicManaCost = 10f;
    public float magicHeavyManaCost = 50f;

    [Header("Dash")]
    public float dashDuration = 0.2f;

    public float dashCooldown = 1.0f;
    public float dashImpulseFactor = 20f;

    [Header("Hit")]
    public float iframeTime = 0.5f;
    public float iframeBlinkPeriod = 0.2f;
    public UnityEvent OnHitEvent;
    public GameObject gameOverUI;

    public bool Won { get; set; } = false;

    #endregion

    #region PRIVATE VARIABLES
    
    private Vector2 _facingVec;
    private bool _wantsToDash;
    private float _dashTiming;
    private Laser _magicLaserInstance;
    private Camera _mainCamera;
    private new AudioManager audio;
    private float _iframeTiming = 0f;
    private bool _isDead;

    // Post Procesing
    private PostProcessVolume _postProcess;
    private Vignette _ppVignette;
    
    #endregion

    #region COMPONENTS

    private Player _player;
    private Rigidbody2D _body;
    private Health _health;
    private Mana _mana;
    private FightingStyle _fightingStyle;
    private BowScript _bowScript;
    private SpriteRenderer _spriteRenderer;

    #endregion

    #region INPUT VARIABLES
    
    private Vector2 _movement;
    private Vector2 _joystickAim;
    private float _attackStyleChange;
    private bool _jump;
    private bool _dash;
    private bool _lightAttack;
    private bool _heavyAttack;
    private bool _heavyAttackRelease;
    private bool _heavyAttackCurrentlyPressed;
    
    #endregion

    #region PROPERTIES

    // Shooting direction (both mouse / joystick support)
    public Vector2 ShootingDirection => CurrentController != null
        ? CurrentController.type == ControllerType.Joystick ? _joystickAim : GetAimingMouseDirection()
        : Vector2.zero;

    // Current active controller
    private Controller CurrentController => _player.controllers.GetLastActiveController();
    
    #endregion
    
    public void TakeDamage(float damage) {
        if (_isDead) return;
        if (_iframeTiming > 0f) return;
        if (Won) return;

        FreezeFrame.Instance.Freeze();

        _health.TakeDamage(damage);
        audio.Play("HeroHurt");
        _iframeTiming = iframeTime;

        StartCoroutine(BlinkSprite(iframeBlinkPeriod));
        if (_ppVignette)
        {
            _ppVignette.intensity.Interp(0f, 0.6f, 1f - _health.value / _health.maxValue);
            StartCoroutine(BlinkScreen(_ppVignette, _ppVignette.intensity.value, 0.7f, 0.4f));
        }
    }
    
    private IEnumerator BlinkSprite(float period)
    {
        while (_iframeTiming > 0f)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(period / 2f);
        }
        _spriteRenderer.enabled = true;
    }

    private IEnumerator BlinkScreen(Vignette vignette, float initialIntensity, float maxIntensity, float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            if (Time.timeScale > 0f)
            {
                if (timeElapsed < duration / 2f)
                {
                    vignette.intensity.Interp(initialIntensity, maxIntensity, 2 * timeElapsed / duration);
                }
                else
                {
                    vignette.intensity.Interp(maxIntensity, initialIntensity, (duration / 2f + (timeElapsed - duration / 2f)) / duration);
                }

                timeElapsed += Time.deltaTime;
            }
            yield return null;
        }
    }

    private void Awake() {
        _player = ReInput.players.GetPlayer(0);
        _body = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
        _mana = GetComponent<Mana>();
        _fightingStyle = GetComponent<FightingStyle>();
        _bowScript = GetComponentInChildren<BowScript>();
        _spriteRenderer = anim.GetComponent<SpriteRenderer>();

        _postProcess = GameObject.FindGameObjectWithTag("Post Processing")?.GetComponent<PostProcessVolume>();
        _ppVignette = _postProcess?.profile.GetSetting<Vignette>();
        if (_ppVignette) _ppVignette.intensity.value = 0f;

        // Controller map in gameplay mode
        _player.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "Gameplay").enabled = true;
        _player.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "UI").enabled = false;
        _player.controllers.maps.mapEnabler.Apply();
    }

    private void Start() {
        _mainCamera = Camera.main;

        _facingVec = new Vector2(1.0f, 0);
        audio = FindObjectOfType<AudioManager>();
        _isDead = false;
        Won = false;
    }


    private void Update() {
        GetInputs();

        if (_movement.magnitude > 0) {
            if (anim.CurrentMode == HeroAnim.Mode.Move || anim.CurrentMode == HeroAnim.Mode.Jump) {
                // only updating facing_vec when actually moving
                _facingVec = _movement;
            }
        }

        if (_iframeTiming > 0f)
        {
            _iframeTiming = Mathf.Clamp(_iframeTiming - Time.deltaTime, 0f, iframeTime);
        }

        if (_dashTiming <= dashCooldown - dashDuration) // not update during dash
            anim.UpdateDirection(_movement);

        if (_magicLaserInstance && _fightingStyle.currentStyle != FightingStyle.Style.Magic) {
            _magicLaserInstance.Destroy();
            _magicLaserInstance = null;
        }

        if (_bowScript.gameObject.activeSelf && _fightingStyle.currentStyle != FightingStyle.Style.Range) {
            _bowScript.gameObject.SetActive(false);
        }

        Dash();

        if (_jump) {
            anim.SwitchMode(HeroAnim.Mode.Jump);
        }

        if (anim.CurrentMode != HeroAnim.Mode.Jump) {
            switch (_fightingStyle.currentStyle) {
                case FightingStyle.Style.Melee:
                    UpdateAttackMelee();
                    break;

                case FightingStyle.Style.Range:
                    UpdateAttackRange();
                    UpdateAttackRangeHeavy();
                    break;

                case FightingStyle.Style.Magic:
                    UpdateAttackMagic();
                    UpdateAttackMagicHeavy();
                    break;
            }
        }

        if (anim.CurrentMode == HeroAnim.Mode.BigSlash) {
            if (_heavyAttackRelease)
                if (sword.CancelBigSlash())
                    anim.SwitchMode(HeroAnim.Mode.Move);
            anim.SetModeSpeed(sword.GetSpeedForHeroAnimator());
        }

        if (_attackStyleChange > 0)
        {
            _fightingStyle.NextStyle();
        }
        else if (_attackStyleChange < 0)
        {
            _fightingStyle.PreviousStyle();
        }

        // On fighting style change
        if (_attackStyleChange != 0)
        {
            _mana.SetEnabled(_fightingStyle.currentStyle == FightingStyle.Style.Magic);
        }
    }


    private void UpdateAttackMelee() {
        if (_lightAttack == _heavyAttack || anim.IsAttacking() || anim.CurrentMode != HeroAnim.Mode.Move)
            return;

        anim.UpdateSlashDirection(_facingVec);
        anim.SwitchMode(_heavyAttack ? HeroAnim.Mode.BigSlash : HeroAnim.Mode.Slash);
        sword.TriggerSlash(_facingVec, _heavyAttack);
    }

    private void UpdateAttackRange() {

        // If still attacking or aiming
        if (anim.IsAttacking() || anim.CurrentMode == HeroAnim.Mode.Aim)
            return;
        // If not trying to shoot
        if (CurrentController.type != ControllerType.Joystick && ShootingDirection.magnitude == 0 || !_lightAttack)
            return;

        if (_heavyAttackCurrentlyPressed) return;

        // Shooting the dagger
        Instantiate(daggerPrefab, transform.position - new Vector3(0.0f, 0.0f), Quaternion.identity);

        // Triggering the animation
        anim.UpdateDirection(ShootingDirection);
        anim.UpdateSlashDirection(ShootingDirection);
        anim.SwitchMode(HeroAnim.Mode.Slash);
        // Increase attack animation's speed
        anim.SetModeSpeed(attackAnimSpeedFactor);

        audio.Play("Throw");
    }

    private void UpdateAttackRangeHeavy() {
        if (_heavyAttack) {
            _bowScript.gameObject.SetActive(true);
            _bowScript.ChargeShot();
            anim.SwitchMode(HeroAnim.Mode.Aim);
        }
        else if (_heavyAttackRelease && _bowScript.gameObject.activeSelf) {
            _bowScript.Shoot();
            _bowScript.gameObject.SetActive(false);
        }

        anim.UpdateDirection(ShootingDirection);
        anim.UpdateSlashDirection(ShootingDirection);
    }

    private void UpdateAttackMagic() {
        if (anim.IsAttacking()) return; // ensures cooldown has expired

        if (CurrentController.type != ControllerType.Joystick && ShootingDirection.magnitude == 0 || !_lightAttack) return;

        if (_heavyAttackCurrentlyPressed) return;

        if (!_mana.UseMana(magicManaCost)) return; // ensure enough mana is available, and use mana

        // shooting the magic ball
        Instantiate(magicBallPrefab, transform.position, Quaternion.identity);

        // triggering the animation
        anim.UpdateDirection(ShootingDirection);
        anim.UpdateSlashDirection(ShootingDirection);
        anim.SwitchMode(HeroAnim.Mode.Slash);
        anim.SetModeSpeed(3); // Slash animation 3 times faster

        // playing sound
        audio.Play("CastSpell");
    }

    private void UpdateAttackMagicHeavy() {
        if (_heavyAttack && _magicLaserInstance == null) {
            if (!_mana.HasEnough(magicHeavyManaCost)) return;
            _magicLaserInstance = Instantiate(magicLaserPrefab, transform.position, Quaternion.identity, transform).GetComponent<Laser>();
            audio.Play("LaserBuildup");
        }

        if (_heavyAttackRelease && _magicLaserInstance != null) {
            if (_magicLaserInstance.isReady) {
                _mana.UseMana(magicHeavyManaCost);
                _magicLaserInstance.Shoot();

                anim.SwitchMode(HeroAnim.Mode.Slash);
                anim.SetModeSpeed(3);

                audio.Play("LaserShoot");
            }
            else {
                _magicLaserInstance.Destroy();
                _magicLaserInstance = null;
            }
            audio.Stop("LaserBuildup");
        }

        if (_magicLaserInstance != null && !_magicLaserInstance.isShooting) {
            _magicLaserInstance.SetDirection(ShootingDirection);
            anim.UpdateDirection(ShootingDirection);
            anim.UpdateSlashDirection(ShootingDirection);
        }
    }


    private Vector2 GetAimingMouseDirection() {
        var screenPosition = new Vector3(_player.controllers.Mouse.screenPosition.x, _player.controllers.Mouse.screenPosition.y);
        var worldPosition = _mainCamera.ScreenToWorldPoint(screenPosition);

        return (worldPosition - transform.position).normalized;
    }

    private void FixedUpdate() {
        if (_dashTiming <= dashCooldown - dashDuration) {
            // If doing dash, do not modify velocity by hand
            switch (anim.CurrentMode) {
                case HeroAnim.Mode.Move:
                case HeroAnim.Mode.Jump:
                    _body.velocity = _movement * speed;
                    break;
                case HeroAnim.Mode.Slash:
                    _body.velocity = _movement * (speed * slowFactor);
                    break;
                case HeroAnim.Mode.BigSlash:
                    _body.velocity = Vector2.zero;
                    break;
            }
            
            // Managing move speed while charging arrows / shooting laser
            switch (_fightingStyle.currentStyle) {
                case FightingStyle.Style.Range when _bowScript.gameObject.activeSelf:
                    _body.velocity = Vector2.zero;
                    break;
                case FightingStyle.Style.Magic when _magicLaserInstance != null:
                    _body.velocity = _magicLaserInstance.isShooting ? Vector2.zero : _movement * (speed * slowFactor);
                    break;
            }

        }
        

        // Dashing
        if (!_wantsToDash) return;
        _body.AddForce(_facingVec * (_body.mass * dashImpulseFactor), ForceMode2D.Impulse);
        audio.Play("Dash");
        _wantsToDash = false;
    }

    private void Dash() {
        if (_dashTiming > 0) {
            _dashTiming -= Time.deltaTime;
            var progress = 1 - _dashTiming / dashCooldown;
            heroMaterial.SetFloat(Progress, 1 - progress * progress);
        }
        else if (_dash) {
            _dashTiming = dashCooldown;
            _wantsToDash = true;
            anim.UpdateDirection(_facingVec);
        }
    }

    private void GetInputs() {
        _movement = _player.GetAxis2D("Move Horizontal", "Move Vertical").normalized;
        _joystickAim = _player.GetAxis2D("Aim Horizontal", "Aim Vertical").normalized;

        _attackStyleChange = _player.GetAxis("Attack Style");
        if (_attackStyleChange == _player.GetAxisPrev("Attack Style")) _attackStyleChange = 0f; // Prevents repeats with gamepad

        _jump = _player.GetButtonDown("Jump");
        _dash = _player.GetButtonDown("Dash");

        _lightAttack = (CurrentController?.type == ControllerType.Joystick && _fightingStyle.currentStyle != FightingStyle.Style.Melee) ?
            _joystickAim != Vector2.zero : _player.GetButtonDown("Light Attack");

        _heavyAttack = _player.GetButtonDown("Heavy Attack");
        _heavyAttackRelease = _player.GetButtonUp("Heavy Attack");
        _heavyAttackCurrentlyPressed = _player.GetButton("Heavy Attack");
    }

    public void OnHealthEmpty() {
        _isDead = true;
        gameOverUI.SetActive(true);
    }
}
