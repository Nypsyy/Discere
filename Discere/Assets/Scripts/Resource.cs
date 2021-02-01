using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/**
 * Defines a resource shown by a UI bar
 * e.g. Health, Mana, ...
 */
public class Resource : MonoBehaviour
{
    public Slider ui;                               // UI Element
    public float minValue = 0f, maxValue = 100f;    // Bounds
    public UnityEvent emptyEvt, fullEvt;            // Events

    public float value { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        value = maxValue;

        if (ui == null)
        {
            Debug.LogWarning("No UI element for resource");
            return;
        }
        ui.maxValue = maxValue;
        ui.minValue = minValue;
        ui.value = maxValue;
    }

    public void ChangeValue(float amount)
    {
        value = Mathf.Clamp(value + amount, minValue, maxValue);
        if (ui != null) ui.value = value;

        if (value <= minValue) emptyEvt.Invoke();
        if (value >= maxValue) fullEvt.Invoke();
    }
}
