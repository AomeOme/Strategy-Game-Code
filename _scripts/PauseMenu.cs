using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;

    bool menuOpen = false;
    public UnityEvent<GameObject> Resume;



    // Update is called once per frame
    void Update()
    {

        if (!menuOpen)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                pauseMenuPanel.SetActive(true);
                menuOpen = true;
                Time.timeScale = 0;
            } 
        }
        else if (menuOpen)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                pauseMenuPanel.SetActive(false);
                menuOpen = false;
                Time.timeScale = 1;
            }
        }

    }

    public void ResumeFunction()
    {
        pauseMenuPanel.SetActive(false);
        menuOpen = false;
        Time.timeScale = 1;
    }
}
