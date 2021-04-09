using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rewired;
using TMPro;
using UnityEngine.EventSystems;

public class MenuButtonBehavior : MonoBehaviour
{
    [System.Serializable]
    public struct TutorialImage
    {
        public Image image;
        public string name;
    }

    [Header("General")]
    public GameObject mainMenu;
    public GameObject howToMenu;
    public GameObject backButton;
    public GameObject howToButton;

    [Header("HowTo menu")]
    public TutorialImage[] controllerTutorials;
    public TextMeshProUGUI changeTutorialBtnText;
    
    private Player _input;
    private int _tutorialIndex;

    private void Awake() {
        _input = ReInput.players.GetPlayer("SYSTEM");
    }

    private void Update() {
        if (_input.GetButtonDown("UI Cancel") && howToMenu.activeSelf) {
            Back();
        }
    }

    // MAIN MENU
    public void NewGame() {
        SceneManager.LoadScene("Boss");
    }

    public void HowTo() {
        mainMenu.SetActive(false);
        howToMenu.SetActive(true);
        _tutorialIndex = 0;
        EventSystem.current.SetSelectedGameObject(backButton);
        UpdateTutorialView();
    }

    public void Quit() {
        Application.Quit();
    }

    // HOW TO

    public void Back() {
        mainMenu.SetActive(true);
        howToMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(howToButton);
    }

    public void ChangeControlsView() {
        _tutorialIndex = (_tutorialIndex + 1) % controllerTutorials.Length;

        UpdateTutorialView();
    }

    private void UpdateTutorialView() {
        for (var i = 0; i < controllerTutorials.Length; i++) {
            controllerTutorials[i].image.gameObject.SetActive(i == _tutorialIndex);
        }

        var nextControllerName = controllerTutorials[(_tutorialIndex + 1) % controllerTutorials.Length].name;
        changeTutorialBtnText.text = $"Show {nextControllerName} controls";
    }
}