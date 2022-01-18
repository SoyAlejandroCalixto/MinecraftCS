using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject hudCanvas;
    public GameObject PauseCanvas;

    public void BackToGame_Click()
    {
        PauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        hudCanvas.SetActive(true);
        Time.timeScale = 1;
    }

    public void QuitToTitle_Click()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Scenes/Titlescreen");
    }
}
