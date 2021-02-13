namespace SGoap.Combatant
{
    using System;
    using SGoap;
    using SGoap.Services;
    using UnityEngine;

    public class PlayerController : MonoBehaviour, ITarget, IAttacker
    {
        public CoolDown Cooldown = new CoolDown();

        public AnimationController AnimatorController;
        public EffectController EffectController;
        public CharacterStatusController StatusController;
        public Rigidbody Rigidbody;

        public float MoveSpeed = 3;
        public float JumpVelocity = 5;

        public ColliderResult FeetCollider;

        private Vector3 _dir;
        private float? _lastMovementKeyPressed;

        public ETeam Team => ETeam.Blue;
        public bool Guarding => Input.GetKey(KeyCode.LeftShift);

        public GameObject Parry;

        private void Awake()
        {
            StatusController.OnDamageTaken += OnTakeDamage;
        }

        private void OnTakeDamage(int hp, int maxHp, IAttacker attacker)
        {
            // Check if you got hit from behind.
            Cooldown.Run(!AnimatorController.Guarding ? 1.5f : 0.3f);
            AnimatorController.PlayImpact();
        }

        private float _attackChargeTime;

        private void Update()
        {
            if (Cooldown.Active)
                return;

            // Combat
            if (!Guarding && !AnimatorController.Attacking)
            {
                if (Input.GetMouseButton(0))
                {
                    AnimatorController.Charge(_attackChargeTime);
                    _attackChargeTime += Time.deltaTime;
                    AnimatorController.SetAttackDirection(-1);
                }

                if (Input.GetMouseButton(1))
                {
                    AnimatorController.Charge(_attackChargeTime);
                    _attackChargeTime += Time.deltaTime;
                    AnimatorController.SetAttackDirection(1);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (TryParry())
                        return;

                    AnimatorController.Attack(EAttackDirection.Left, _attackChargeTime);
                    _attackChargeTime = 0;
                    AnimatorController.Charge(0);
                    return;
                }

                if (Input.GetMouseButtonUp(1))
                {
                    if (TryParry())
                        return;

                    AnimatorController.Attack(EAttackDirection.Right, _attackChargeTime);
                    _attackChargeTime = 0;
                    AnimatorController.Charge(0);
                    return;
                }
            }

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                return;

            // Movement
            if (!AnimatorController.Attacking && FeetCollider.Collided)
            {
                UpdateMovement();
                UpdateJump();
            }
        }

        public bool TryParry()
        {
            var nearestTarget = TargetManager.FindClosest(transform.position);
            var agentAnimationMessages = nearestTarget.transform.GetComponentInChildren<AnimationMessages>();
            var agentAnimator = nearestTarget.transform.GetComponentInChildren<Animator>();
            var agent = nearestTarget.transform.GetComponent<BasicAgent>();

            if (agentAnimationMessages.ParryProcess.State)
            {
                Parry.SetActive(true);
                var dir = transform.position - nearestTarget.transform.position;
                dir.Normalize();
                Parry.transform.position = nearestTarget.transform.position + dir * 1;
                Parry.transform.position += new Vector3(0, 1, 0);

                agentAnimator.SetTrigger("Impact");
                agent.Data.Cooldown.Run(0.5f);
                return true;
            }

            return false;
        }

        private void UpdateJump()
        {
            if (Input.GetButtonDown("Jump"))
            {
                var factor = 2;
                var x = _dir.x * factor;
                var z = _dir.z * factor;
                Rigidbody.velocity = new Vector3(x, JumpVelocity, z);
                AnimatorController.Jump();
                Cooldown.Run(0.1f);
            }
        }

        private Vector3 _dashDir;
        public Vector3 MoveDir => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        private void UpdateMovement()
        {
            _dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            var rawDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            if (Guarding)
            {
                var nearestTarget = TargetManager.FindClosest(transform.position);

                if (nearestTarget != null)
                {
                    var dir = nearestTarget.transform.position - transform.position;
                    dir.Normalize();

                    transform.forward = Vector3.Lerp(transform.forward, dir, 3 * Time.deltaTime);
                }

                AnimatorController.Guard(true);
            }
            else
                AnimatorController.Guard(false);

            // Rotation
            var rotation = _dir.sqrMagnitude != 0 ? Quaternion.LookRotation(_dir) : transform.rotation;
            if (_dir.sqrMagnitude > 0 && !Guarding)
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 8f * Time.deltaTime);

            // Movement
            var lookDotProduct = Quaternion.Dot(transform.rotation, rotation);

            if (Mathf.Abs(lookDotProduct) > 0.9f || Guarding)
                transform.position += _dir * Time.deltaTime * MoveSpeed;

            var anyMovementKey = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S);

            if (anyMovementKey && Guarding)
            {
                if (_lastMovementKeyPressed != null)
                {
                    var _lastMovementTimeElapsed = Time.time - _lastMovementKeyPressed;
                    if (_lastMovementTimeElapsed <= 0.2f && _dashDir == rawDir)
                    {
                        var dashToPosition = transform.position + rawDir * 2;

                        transform.GoTo(dashToPosition, 0.1F);
                        Cooldown.Run(duration: 0.2F);

                        // Instead we should put an event
                        EffectController.PlayDash(0.5F);
                    }

                    if (!rawDir.Equals(_dashDir))
                        _dashDir = rawDir;
                }

                _lastMovementKeyPressed = Time.time;
            }

            var x = Mathf.Abs(_dir.x);
            var z = Mathf.Abs(_dir.z);

            AnimatorController.SetMovement(x > z ? x : z);
        }
    }
}