using System;
using UnityEngine;

public class DisplayFightingStyle : MonoBehaviour
{
    [SerializeField] private FightingStyle fightingStyle;

    [SerializeField] private GameObject meleeImage;
    [SerializeField] private GameObject rangedImage;
    [SerializeField] private GameObject magicImage;

    private GameObject _previousActive;

    private void Awake() {
        _previousActive = meleeImage;
    }

    private void Update() {
        switch (fightingStyle.currentStyle) {
            case FightingStyle.Style.Melee:
                UpdateActiveImage(meleeImage);
                break;

            case FightingStyle.Style.Range:
                UpdateActiveImage(rangedImage);
                break;

            case FightingStyle.Style.Magic:
                UpdateActiveImage(magicImage);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateActiveImage(GameObject current) {
        if (current == _previousActive) return;

        current.SetActive(true);
        _previousActive.SetActive(false);
        _previousActive = current;
    }
}