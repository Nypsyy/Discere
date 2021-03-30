using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFrame : MonoBehaviour
{
    public static FreezeFrame Instance { get; private set; } = null;

    private float oldTimeScale = 1f;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Freeze(float duration = 0.1f)
    {
        StartCoroutine(DoFreeze(duration));
    }

    private IEnumerator DoFreeze(float duration)
    {
        oldTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = oldTimeScale;
    }
}
