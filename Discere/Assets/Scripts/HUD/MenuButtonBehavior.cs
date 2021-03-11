using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonBehavior : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Boss");
    }

    public void Options()
    {
        Debug.Log("OPTIONS");
    }

    public void Quit()
    {
        Debug.Log("QUIT");
    }
}
