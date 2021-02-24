using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin camShakeNoise;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        cam = GetComponent<CinemachineVirtualCamera>();
        if (cam == null) return;
        camShakeNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
}
