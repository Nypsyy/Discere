using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Defines a fighting style for the hero
 */
public class FightingStyle : MonoBehaviour
{
    public enum Style
    {
        Melee = 0, Range, Magic
    }

    // Maps each Style to its UI element
    [System.Serializable]
    public class StyleImageMapping
    {
        public Style style;
        public Image image;
    }
    public List<StyleImageMapping> stylesUI;

    public Style currentStyle { get; private set; } = Style.Melee;

    private void Start()
    {
        ChangeDisplayedImage(currentStyle);
    }

    /**
     * Change current style, using a circular selection between possible styles.
     * @param invertDirection : allows to chose direction of the selection
     */
    public void SwitchStyle(bool invertDirection = false)
    {
        int stylesCnt = System.Enum.GetNames(typeof(Style)).Length;
        Style newStyle = (invertDirection ? --currentStyle : ++currentStyle);

        if ((int)newStyle >= stylesCnt) newStyle -= stylesCnt;
        if ((int)newStyle < 0) newStyle += stylesCnt;

        currentStyle = newStyle;
        ChangeDisplayedImage(currentStyle);
    }

    // Switches displayed ui element to the correct image
    private void ChangeDisplayedImage(Style style)
    {
        foreach (StyleImageMapping el in stylesUI)
        {
            if (el.image == null)
            {
                Debug.LogError("No image for style " + System.Enum.GetName(typeof(Style), style));
                return;
            }
            el.image.gameObject.SetActive(el.style == style);
        }
    }
}
