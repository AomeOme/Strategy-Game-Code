using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Its game time");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Fuck off go to sleep");
    }
}
