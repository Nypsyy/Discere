using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Pause : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        if (input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                timeScaleBackup = Time.timeScale;
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = timeScaleBackup;
            }
        }
    }
}
