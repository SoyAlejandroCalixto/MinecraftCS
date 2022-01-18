using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlescreenPanorama : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Started");
    }

    private void Awake()
    {
        Debug.Log("Awaked");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F11))
        {
            if(Screen.fullScreenMode == FullScreenMode.Windowed)
            {
                Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow);
            }

            if(Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            {
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
            }
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0.025f, 0);
    }
}
