using System;
using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LaserData laserData;

    public LineRenderer ray;
    public GameObject chargingCircle;
    public LayerMask obstacleLayers;
    public LayerMask hurtingLayers;
    public SpriteRenderer beamSprite;
    public SpriteRenderer beamStartSprite;

    private Hero _hero;
    private float _initialRayWidth;
    private float _chargingTimer;
    private Vector3 _initialChargingCircleScale;
    private float _beamWidth;
    private Vector2 _laserVector;
    private bool _isCharging;

    public bool IsReady { get; private set; }
    public bool IsShooting { get; private set; }

    public void Cancel() {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator Shooting() {
        IsShooting = true;
        yield return new WaitForSeconds(laserData.beamDuration);
        Destroy(gameObject);

        yield return null;
    }

    private void UpdateChargingRay() {
        // Update timer
        _chargingTimer += Time.deltaTime;

        chargingCircle.transform.localScale = _initialChargingCircleScale * (laserData.chargeTime - _chargingTimer) / laserData.chargeTime;

        // Update ray width
        var newWidth = (_initialRayWidth - laserData.readyRayWidth) * (laserData.chargeTime - _chargingTimer) / laserData.chargeTime +
                       laserData.readyRayWidth;
        ray.startWidth = newWidth;
        ray.endWidth = newWidth;
    }

    private IEnumerator Charging() {
        _isCharging = true;
        yield return new WaitForSeconds(laserData.chargeTime);
        _isCharging = false;
        chargingCircle.SetActive(false);
        StartCoroutine(ReadyRay());
        IsReady = true;

        yield return null;
    }

    private void Awake() {
        _hero = GetComponentInParent<Hero>();
    }

    private void Start() {
        _initialChargingCircleScale = chargingCircle.transform.localScale;
        _initialRayWidth = ray.startWidth;
        StartCoroutine(Charging());
    }

    private void Update() {
        // Manage shooting
        if (IsShooting) {
            var angle = Vector2.SignedAngle(Vector2.up, _hero.ShootingDirection);
            var hits = Physics2D.BoxCastAll(transform.position,
                                            Vector2.one * _beamWidth,
                                            angle,
                                            _hero.ShootingDirection,
                                            _laserVector.magnitude,
                                            hurtingLayers);
            if (hits.Length <= 0) return;

            foreach (var hit2D in hits) {
                var obj = hit2D.collider.gameObject;
                obj.GetComponent<Minotaur>()?.TakeDamage(laserData.damage, FightingStyle.Style.Magic);

                obj.GetComponent<Rock>()?.DestroyMe();
            }
        }

        // If charging
        if (_isCharging)
            UpdateChargingRay();

        // Raycast in laser's direction
        var hit = Physics2D.Raycast(transform.position, _hero.ShootingDirection, float.PositiveInfinity, obstacleLayers);
        var hitCoords = transform.InverseTransformPoint(hit.point);
        var points = new Vector3[2];

        // Update ray's points
        points[0] = Vector3.zero;
        points[1] = new Vector3(hitCoords.x, hitCoords.y, 0f);
        ray.SetPositions(points);
        _laserVector.x = hitCoords.x;
        _laserVector.y = hitCoords.y;
    }

    public void Shoot() {
        if (!IsReady) return;

        FreezeFrame.Instance.Freeze(0.15f);

        // Display beam
        var rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(_laserVector.x, _laserVector.y, 0f));
        var startBeamLength = beamStartSprite.transform.localScale.y;

        beamSprite.transform.localScale = new Vector3(beamSprite.transform.localScale.x,
                                                      beamSprite.transform.localScale.y * (_laserVector.magnitude - startBeamLength) / 2f,
                                                      0f);
        beamSprite.transform.rotation = rotation;
        beamSprite.transform.Translate(Vector2.up * (_laserVector.magnitude / 2f + startBeamLength));
        beamSprite.gameObject.SetActive(true);

        beamStartSprite.transform.rotation = rotation;
        beamStartSprite.transform.Translate(Vector2.up * startBeamLength);
        beamStartSprite.gameObject.SetActive(true);

        _beamWidth = beamSprite.transform.localScale.x;

        // Remove ray
        ray.gameObject.SetActive(false);

        // Screen shake
        if (CinemachineShake.Instance)
            CinemachineShake.Instance.Shake(6f, 0.5f);

        StartCoroutine(Shooting());
    }

    private IEnumerator ReadyRay() {
        ray.startColor = Color.white;
        ray.endColor = Color.white;
        yield return new WaitForSeconds(0.05f);
        ray.startColor = laserData.readyRayColor;
        ray.endColor = laserData.readyRayColor;

        yield return null;
    }
}