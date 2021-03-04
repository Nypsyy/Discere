using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class Pause : MonoBehaviour
{
    public Canvas ui;

    private Player input;
    private bool isPaused = false;
    private float timeScaleBackup;

    private void Awake()
    {
        input = ReInput.players.GetPlayer(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeScaleBackup = Time.timeScale;
        SetPause(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (input.GetButtonDown("Pause"))
        {
            SetPause(!isPaused);
        }
    }

    private void SetPause(bool pause)
    {
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
        Debug.Log("Options not implemented yet !");
    }

    public void OnExit()
    {
        Debug.Log("Exit not implemented yet !");
    }
}
