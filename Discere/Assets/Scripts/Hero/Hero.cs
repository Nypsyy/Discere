using UnityEngine;
using Rewired;

public class Hero : MonoBehaviour {

    [Header("General")]
    public float speed;
    public HeroAnim anim;

    [Header("Melee")]
    public HeroSword sword;

    [Header("Range")]
    public GameObject daggerPrefab;
    public float firingFrequency;

    [Header("Magic")]
    public GameObject magicBallPrefab;
    public GameObject magicLaserPrefab;
    public float magicFiringFrequency;
    public float magicManaCost = 10f;
    public float magicHeavyManaCost = 50f;

    private Player player;
    private Vector2 input_vec;
    private Vector2 facing_vec;
    private Vector2 aiming_vec_Joystick;
    private Vector2 aiming_vec_Mouse;
    private Rigidbody2D body;
    private float firingCooldown;
    private float magicFiringCooldown;
    private Laser magicLaserInstance = null;
    private Camera mainCamera;

    // Components
    private Health health;
    private Mana mana;
    private FightingStyle fightingStyle;

    private void Awake() {
        player = ReInput.players.GetPlayer(0);
        body = GetComponent<Rigidbody2D>();

        // Controller map in gameplay mode
        player.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "Gameplay").enabled = true;
        player.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "UI").enabled = false;
        player.controllers.maps.mapEnabler.Apply();
    }

    // Start is called before the first frame update
    private void Start() {
        health = GetComponent<Health>();
        mana = GetComponent<Mana>();
        fightingStyle = GetComponent<FightingStyle>();

        mainCamera = Camera.main;

        facing_vec = new Vector2(1.0f, 0);
        firingCooldown = -1;
        magicFiringCooldown = -1;
    }


    // Update is called once per frame
    private void Update() {
        input_vec = player.GetAxis2D("Move Horizontal", "Move Vertical");

        if (!(input_vec.x == 0 && input_vec.y == 0)) {
            if (anim.mode == HeroAnim.Mode.Move || anim.mode == HeroAnim.Mode.Jump) {
                // only updating facing_vec when actually moving
                facing_vec = input_vec;
            }
        }

        anim.UpdateDirection(input_vec);

        if (magicLaserInstance != null && fightingStyle.currentStyle != FightingStyle.Style.Magic)
        {
            magicLaserInstance.Destroy();
            magicLaserInstance = null;
        }

        switch (fightingStyle.currentStyle)
        {
            case FightingStyle.Style.Melee:
                UpdateAttackMelee();
                break;

            case FightingStyle.Style.Range:
                UpdateAttackRange();
                break;

            case FightingStyle.Style.Magic:
                UpdateAttackMagic();
                UpdateAttackMagicHeavy();
                break;

            default:
                break;
        }

        if (player.GetButtonDown("Jump")) {
            anim.SwitchMode(HeroAnim.Mode.Jump);
        }

        if (anim.mode == HeroAnim.Mode.Move) {
            switch (fightingStyle.currentStyle) {
                case FightingStyle.Style.Melee:
                    UpdateAttackMelee();
                    break;

                case FightingStyle.Style.Range:
                    UpdateAttackRange();
                    break;
            }
            
        } else if (anim.mode == HeroAnim.Mode.BigSlash) {
            if (!player.GetButton("Heavy Attack"))
                if (sword.CancelBigSlash())
                    anim.SwitchMode(HeroAnim.Mode.Move);
        }

        float switch_attack_style = player.GetAxis("Attack Style");
        if (switch_attack_style > 0)
            fightingStyle.NextStyle();
        else if (switch_attack_style < 0)
            fightingStyle.PreviousStyle();

    }

    private void UpdateAttackMelee() {
        if (anim.mode == HeroAnim.Mode.Move) {
            bool is_light_attack = player.GetButtonDown("Light Attack");
            bool is_heavy_attack = player.GetButtonDown("Heavy Attack");
                
            if (is_light_attack == is_heavy_attack) // either none or both selected
                return;
            anim.UpdateSlashDirection(facing_vec);
            anim.SwitchMode(is_heavy_attack ? HeroAnim.Mode.BigSlash : HeroAnim.Mode.Slash);
            sword.TriggerSlash(facing_vec, is_heavy_attack);
        }
    }

    private void UpdateAttackRange() {
        if (firingCooldown >= 0) return; // ensures cooldown has expired

        Controller controller = player.controllers.GetLastActiveController();
        if (controller == null || (controller.type != ControllerType.Joystick && !player.GetButton("Light Attack"))) return;

        Vector2 shootingDirection = getAimingDirection();
        if (shootingDirection.x == 0 && shootingDirection.y == 0) return;

        shootingDirection = shootingDirection.normalized;

        // shooting the dagger
        GameObject dagger = Instantiate(daggerPrefab, transform.position - new Vector3(0.0f, 0.0f), Quaternion.identity);
        dagger.GetComponent<Rigidbody2D>().velocity = shootingDirection * dagger.GetComponent<Projectiles>().velocity;
        dagger.transform.Rotate(0.0f, 0.0f, -45.0f);
        dagger.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        firingCooldown = (1 / firingFrequency) * 60;

        // triggering the animation
        anim.UpdateDirection(shootingDirection);
        anim.UpdateSlashDirection(shootingDirection);
        anim.SwitchMode(HeroAnim.Mode.Slash);
        anim.SetModeSpeed(3); // Slash animation 3 times faster
    }

    void UpdateAttackMagic()
    {
        magicFiringCooldown -= Time.deltaTime;
        if (magicFiringCooldown >= 0) return; // ensures cooldown has expired

        Controller controller = player.controllers.GetLastActiveController();
        if (controller == null || (controller.type != ControllerType.Joystick && !player.GetButton("Light Attack"))) return;

        if (player.GetButton("Heavy Attack")) return;

        Vector2 shootingDirection = getAimingDirection();
        if (shootingDirection.x == 0 && shootingDirection.y == 0) return;

        shootingDirection = shootingDirection.normalized;

        if (!mana.UseMana(magicManaCost)) return; // ensure enough mana is available, and use mana

        // shooting the magic ball
        GameObject magicBall = Instantiate(magicBallPrefab, transform.position, Quaternion.identity);
        magicBall.GetComponent<MagicProjectile>().SetDirection(shootingDirection);
        magicFiringCooldown = (1 / magicFiringFrequency);

        // triggering the animation
        anim.UpdateDirection(shootingDirection);
        anim.UpdateSlashDirection(shootingDirection);
        anim.SwitchMode(HeroAnim.Mode.Slash);
        anim.SetModeSpeed(3); // Slash animation 3 times faster
    }

    void UpdateAttackMagicHeavy()
    {
        if (player.GetButtonDown("Heavy Attack") && magicLaserInstance == null)
        {
            if (!mana.HasEnough(magicHeavyManaCost)) return;
            magicLaserInstance = Instantiate(magicLaserPrefab, transform.position, Quaternion.identity, transform).GetComponent<Laser>();
        }
        if (player.GetButtonUp("Heavy Attack") && magicLaserInstance != null)
        {
            if (magicLaserInstance.isReady)
            {
                mana.UseMana(magicHeavyManaCost);
                magicLaserInstance.Shoot();

                anim.SwitchMode(HeroAnim.Mode.Slash);
                anim.SetModeSpeed(3);
            }
            else
            {
                magicLaserInstance.Destroy();
                magicLaserInstance = null;
            }
        }
        if (magicLaserInstance != null)
        {
            Vector2 aimDir = getAimingDirection();
            magicLaserInstance.SetDirection(aimDir);
            anim.UpdateDirection(aimDir);
            anim.UpdateSlashDirection(aimDir);
        }
    }

    Vector2 getAimingDirection() {
        Controller controller = player.controllers.GetLastActiveController();
        if (controller == null) return Vector2.zero;

        if (controller.type == ControllerType.Joystick) {
            return player.GetAxis2D("Aim Horizontal", "Aim Vertical");
        }
        else {
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition - new Vector2(transform.position.x, transform.position.y);
        }
    }

    private void FixedUpdate() {
        if (firingCooldown >= 0)
            firingCooldown--;
        switch (anim.mode) {
            case HeroAnim.Mode.Jump:
            case HeroAnim.Mode.Move:
                body.velocity = input_vec * speed;
                break;
            case HeroAnim.Mode.Slash:
                body.velocity = input_vec * (speed * 0.2f);
                break;
            default:
                body.velocity = Vector2.zero;
                break;
        }

        // Managing move speed while shooting magic laser
        if (fightingStyle.currentStyle == FightingStyle.Style.Magic && magicLaserInstance != null)
        {
            body.velocity = magicLaserInstance.isShooting ? Vector2.zero : input_vec * speed * 0.2f;
        }
    }

    public void OnHealthEmpty() {
        Debug.Log("Should die");
    }
}
