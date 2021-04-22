using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using Rewired.Integration.UnityUI;
using UnityEngine.SceneManagement;
using SGoap.Services;

public class Pause : MonoBehaviour
{
    public Canvas ui;

    private Player input;
    private bool isPaused = false;
    private float timeScaleBackup;

    private Button.ButtonClickedEvent onCancelPressed;
    public Button optionsBackButton;
    public Button pauseBackButton;

    private bool canTogglePause = true;

    private void Awake()
    {
        input = ReInput.players.GetPlayer(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeScaleBackup = Time.timeScale;
        SetPause(false);
        OnReturnToPauseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.GetButtonDown("Pause"))
        {
            SetPause(!isPaused);
        }
        if (input.GetButtonDown("UI Cancel") && isPaused)
        {
            onCancelPressed.Invoke();
        }
    }

    private void SetPause(bool pause)
    {
        if (!canTogglePause) return;

        // Set time scale
        if (pause && !isPaused) timeScaleBackup = Time.timeScale;
        Time.timeScale = pause ? 0f : timeScaleBackup;

        // Hide/Show menu
        ui.gameObject.SetActive(pause);

        // Switch input map
        input.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "Gameplay").enabled = !pause;
        input.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "UI").enabled = pause;
        input.controllers.maps.mapEnabler.Apply();

        // Set variable 
        isPaused = pause;
    }


    // UI Events
    public void OnResume()
    {
        SetPause(false);
    }

    public void OnOptions()
    {
        onCancelPressed = optionsBackButton.onClick;
        canTogglePause = false;
    }

    public void OnExit()
    {
        Time.timeScale = timeScaleBackup;
        TargetManager.Clear();
        SceneManager.LoadScene("MainMenu");
    }

    public void OnReturnToPauseMenu()
    {
        onCancelPressed = pauseBackButton.onClick;
        canTogglePause = true;
    }
}
