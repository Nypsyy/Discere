using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    private void Start()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public void ToggleFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }
}
