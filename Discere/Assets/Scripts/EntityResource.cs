using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/**
 *   Defines a resource shown by a UI bar
 *   e.g. Health, Mana, ...
 */
public class EntityResource : MonoBehaviour
{
    public Slider ui;                               // UI Element
    public float minValue = 0f, maxValue = 100f;    // Bounds
    public float initialValue = 100f;               // Initial Value
    public UnityEvent emptyEvt, fullEvt;            // Events

    [Header("Transition")]
    private Slider transitionSlider = null;
    public float transitionWaitingTime = 1f;
    public float transitionTime = 0.2f;
    private short transitionCnt = 0;

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
        ui.value = initialValue;

        // Check if ui has a slider for transition
        Slider[] sliders = ui.GetComponentsInChildren<Slider>();
        if (sliders.Length > 1)
        {
            transitionSlider = sliders[1];
            transitionSlider.maxValue = maxValue;
            transitionSlider.minValue = minValue;
            transitionSlider.value = maxValue;
        }
    }

    protected void ChangeValue(float amount, bool transitionEffect = true)
    {
        value = Mathf.Clamp(value + amount, minValue, maxValue);

        // Manage visual
        if (ui != null)
        { 
            ui.value = value; 
            if (transitionSlider != null)
            {
                if (transitionEffect) StartCoroutine(ChangeTransitionValue());
                else if (transitionCnt <= 0) transitionSlider.value = ui.value;
            }
        }

        if (value <= minValue) emptyEvt.Invoke();
        if (value >= maxValue) fullEvt.Invoke();
    }

    // Coroutine changing value of transitionSlider
    private IEnumerator ChangeTransitionValue()
    {
        // Initial wait
        transitionCnt++;
        yield return new WaitForSeconds(transitionWaitingTime);
        transitionCnt--;
        if (transitionCnt > 0) yield break;

        // Shrinking slider
        float transitionTimer = 0f;
        float startValue = transitionSlider.value;
        while (transitionTimer <= transitionTime)
        {
            transitionSlider.value = Mathf.Lerp(startValue, ui.value, transitionTimer / transitionTime);
            yield return null;
            if (transitionCnt > 0) yield break;
            transitionTimer += Time.deltaTime;
        }

        // Making sure both slider are the same value in the end
        transitionSlider.value = ui.value;
    }
}
