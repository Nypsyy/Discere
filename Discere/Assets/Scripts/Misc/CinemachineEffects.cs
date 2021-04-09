using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineEffects : MonoBehaviour
{
    public static CinemachineEffects Instance { get; private set; }

    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin camShakeNoise;

    private bool isZooming = false;
    private float originalOrthographicSize;
    private Transform originalTarget;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        cam = GetComponent<CinemachineVirtualCamera>();
        if (cam == null) return;
        camShakeNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        originalOrthographicSize = cam.m_Lens.OrthographicSize;
        originalTarget = cam.Follow;
    }

    public void Shake(float intensity, float time)
    {
        if (cam == null)
        {
            Debug.LogWarning("No Cinemachine camera : Shake not available");
            return;
        }

        if (camShakeNoise.m_AmplitudeGain != 0f)
        {
            CancelInvoke();
        }

        camShakeNoise.m_AmplitudeGain = intensity;
        Invoke(nameof(StopShake), time);
    }

    private void StopShake()
    {
        camShakeNoise.m_AmplitudeGain = 0f;
    }

    public void Zoom(float multiplier, Transform target, float transitionDuration = 0.2f, bool useRealTime = false)
    {
        if (isZooming) return;
        StartCoroutine(DoZoom(multiplier, target, transitionDuration, useRealTime));
    }

    public void Zoom(float multiplier, float transitionDuration = 0.2f, bool useRealTime = false)
    {
        Zoom(multiplier, originalTarget, transitionDuration, useRealTime);
    }

    public void UnZoom(float transitionDuration = 0.2f, bool useRealTime = false)
    {
        if (isZooming)
        {
            StopAllCoroutines();
            isZooming = false;
        }
        Zoom(1f, originalTarget, transitionDuration, useRealTime);
    }

    IEnumerator DoZoom(float multiplier, Transform target, float transitionDuration = 0.2f, bool useRealTime = false)
    {
        isZooming = true;

        cam.Follow = target;
        float delta = 0f;
        float beginOrtho = cam.m_Lens.OrthographicSize;

        while (delta < transitionDuration)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(beginOrtho, originalOrthographicSize / multiplier, delta / transitionDuration);
            yield return null;
            delta += useRealTime ? Time.unscaledDeltaTime : Time.deltaTime;
        }

        isZooming = false;
    }
}
