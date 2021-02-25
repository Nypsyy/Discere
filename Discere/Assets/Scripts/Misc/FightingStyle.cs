using UnityEngine;

/**
 * Defines a fighting style for the hero
 */
public class FightingStyle : MonoBehaviour
{
    public enum Style
    {
        Melee = 0,
        Range,
        Magic
    }

    public Style currentStyle { get; private set; } = Style.Melee;

    private readonly int _styleCnt = System.Enum.GetNames(typeof(Style)).Length;

    // Select the next style up in the enum, loops to the first if > enum length
    public void NextStyle() {
        if ((int) ++currentStyle >= _styleCnt) {
            currentStyle = 0;
        }
    }

    // Select the previous style down in the enum, loops to the last if < 0
    public void PreviousStyle() {
        if ((int) --currentStyle < 0) {
            currentStyle += _styleCnt;
        }
    }
}