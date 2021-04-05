using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.Integration.UnityUI;
using UnityEngine.SceneManagement;
using SGoap.Services;

public class GameOverScreen : MonoBehaviour
{
    private Player input;
    private RewiredEventSystem eventSystem;

    public GameObject firstSelected;

    void Awake()
    {
        input = ReInput.players.GetPlayer(0);
    }

    private void OnEnable()
    {
        input.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "Gameplay").enabled = false;
        input.controllers.maps.mapEnabler.ruleSets.Find(rs => rs.tag == "UI").enabled = true;
        input.controllers.maps.mapEnabler.Apply();

        eventSystem = FindObjectOfType<RewiredEventSystem>();
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    public void Retry()
    {
        TargetManager.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        TargetManager.Clear();
        SceneManager.LoadScene("MainMenu");
    }
}
