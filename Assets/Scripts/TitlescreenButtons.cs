using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlescreenButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D steveTexture;

    public void SingleplayerButton_Click()
    {
        SceneManager.LoadScene("Scenes/InGame");
    }

    public void OnClick()
    {
        transform.GetChild(0).localScale = new Vector3(0.95f, 0.95f, 1);
    }
    public void OnClickUp()
    {
        transform.GetChild(0).localScale = new Vector3(1.05f, 1.05f, 1);
    }

    public void OnPointerEnter(PointerEventData ped)
    {
        transform.GetChild(0).localScale = new Vector3(1.05f, 1.05f, 1);
    }

    public void OnPointerExit(PointerEventData ped)
    {
        transform.GetChild(0).localScale = new Vector3(1, 1, 1);
    }

    public void QuitButton_Click()
    {
        Application.Quit();
    }

    public void SkinButton_Click()
    {
        var skinPath = StandaloneFileBrowser.OpenFilePanel("Open your Minecraft skin", "", "png", false);
        try
        {
            if(!File.Exists(skinPath[0])) return;
            else
            {
                byte[] skinByte = File.ReadAllBytes(skinPath[0]);
                steveTexture.LoadImage(skinByte);
            }
        } catch{}
    }

    public void FullscreenButton_Click()
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
