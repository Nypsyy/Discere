using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : EntityResource
{
    public float recovering = 4f;
    public float errorBlinkingSpeed = 1.5f;
    private Image errorFill;
    private Color baseErrorFillColor;
    private bool isErrorFillBlinking = false;

    private new void Start()
    {
        base.Start();

        Transform errorFillObject = ui.transform.Find("ErrorFill");
        if (errorFillObject)
        {
            errorFill = errorFillObject.GetComponent<Image>();
            baseErrorFillColor = errorFill.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (value < maxValue)
        {
            ChangeValue(recovering * Time.deltaTime, false);
        }
    }

    // If has enough mana, use it
    public bool UseMana(float amount)
    {
        bool enough = HasEnough(amount);
        if (enough) ChangeValue(-amount);
        return enough;
    }

    // Returns if it has enough mana for the given amount
    public bool HasEnough(float amount)
    {
        if (amount < value) return true;

        StartCoroutine(BlinkErrorFill());
        return false;
    }

    IEnumerator BlinkErrorFill()
    {     
        if (errorFill == null) yield break;
        if (isErrorFillBlinking) yield break;

        isErrorFillBlinking = true;
        errorFill.gameObject.SetActive(true);

        while (errorFill.color.a > 0f)
        {
            yield return null;
            errorFill.color = new Color(errorFill.color.r, 
                                        errorFill.color.g, 
                                        errorFill.color.b, 
                                        errorFill.color.a - errorBlinkingSpeed * Time.deltaTime);
        }

        errorFill.color = baseErrorFillColor;
        errorFill.gameObject.SetActive(false);
        isErrorFillBlinking = false;
    }
}
