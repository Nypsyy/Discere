using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rewired;
using TMPro;

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
    public GameObject howTo;
    private Player input;
    private Button.ButtonClickedEvent onCancelPressed = null;

    [Header("HowTo menu")]
    public TutorialImage[] controllerTutorials;
    private int tutorialIndex = 0;
    public TextMeshProUGUI changeTutorialBtnText;

    private void Awake()
    {
        input = ReInput.players.GetPlayer(0);

        // Switch input map
        input.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "Gameplay").enabled = false;
        input.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "UI").enabled = true;
        input.controllers.maps.mapEnabler.Apply();
    }

    private void Update()
    {
        if (input.GetButtonDown("UI Cancel") && onCancelPressed != null)
        {
            onCancelPressed.Invoke();
        }
    }

    // MAIN MENU
    public void NewGame()
    {
        SceneManager.LoadScene("Boss");
    }

    public void HowTo()
    {
        mainMenu.SetActive(false);
        howTo.SetActive(true);
        tutorialIndex = 0;
        onCancelPressed = howTo?.transform.Find("Back")?.GetComponent<Button>()?.onClick;

        UpdateTutorialView();
    }

    public void Quit()
    {
        Application.Quit();
    }

    // HOW TO

    public void Back()
    {
        mainMenu.SetActive(true);
        howTo.SetActive(false);
        onCancelPressed = null;
    }

    public void ChangeControlsView()
    {
        tutorialIndex = (tutorialIndex + 1) % controllerTutorials.Length;

        UpdateTutorialView();
    }

    private void UpdateTutorialView()
    {
        for (int i = 0; i < controllerTutorials.Length; i++)
        {
            controllerTutorials[i].image.gameObject.SetActive(i == tutorialIndex);
        }

        string nextControllerName = controllerTutorials[(tutorialIndex + 1) % controllerTutorials.Length].name;
        changeTutorialBtnText.text = $"Show {nextControllerName} controls";
    }
}
