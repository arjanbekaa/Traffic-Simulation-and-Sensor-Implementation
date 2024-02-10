using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MenuManager : MonoSingleton<MenuManager>
{
    [Header("Game Panel Config")]
    public GameObject menuPanel;
    public GameObject gamePlayPanel;
    public GameObject gameOverPanel;
    public GameObject gameFinishedPanel;
    public GameObject settingsPanelHolder;
    
    [Header("Car Camera Config")]
    public GameObject cameraDisplayImageHolder;
    public Image cameraDisplayImage;
    public TextMeshProUGUI cameraDisplayText;

    private Camera currentCarCameraInstance;
    public override void Init()
    {
        persistOnSceneLoad = false;

        base.Init();
    }

    public void TogglePanel(PanelType panelType, bool show)
    {
        switch (panelType)
        {
            case PanelType.Menu:
                if(show)
                {
                    gamePlayPanel.SetActive(false);
                    gameOverPanel.SetActive(false);
                    gameFinishedPanel.SetActive(false);
                    ToggleCameraDisplayImageHolder(false);
                }

                menuPanel.SetActive(show);
                break;
            case PanelType.GamePlay:
                if (show)
                {
                    menuPanel.SetActive(false);
                    gameOverPanel.SetActive(false);
                    gameFinishedPanel.SetActive(false);
                    ToggleCameraDisplayImageHolder(true);
                }

                gamePlayPanel.SetActive(show);
                break;
            case PanelType.GameOver:
                if (show)
                {
                    gamePlayPanel.SetActive(false);
                    menuPanel.SetActive(false);
                    gameFinishedPanel.SetActive(false);
                }

                gameOverPanel.SetActive(show);
                break;
            case PanelType.GameFinished:
                if (show)
                {
                    gamePlayPanel.SetActive(false);
                    menuPanel.SetActive(false);
                    gameOverPanel.SetActive(false);
                }

                gameFinishedPanel.SetActive(show);
                break;
        }
    }
    
    public void ToggleCameraDisplayImageHolder(bool isOn)
    {
        cameraDisplayImageHolder.SetActive(isOn);
    }

    public void ChangeCameraText(string cameraName)
    {
        if(cameraDisplayText != null)
            cameraDisplayText.text = cameraName;
    }

    public void PlayGame()
    {
        GameManager.instance.StartGame();
    }

    public void NextLevel()
    {
        GameManager.instance.PlayNextLevel();
    }

    public void ReplayLevel()
    {
        GameManager.instance.PlayNextLevel();
    }
   
    #region Settings UI
    public void ToggleSettingsMenu(bool show)
    {
        settingsPanelHolder.SetActive(show);
    }
    #endregion

}

public enum PanelType
{
    Menu,
    GamePlay,
    GameOver,
    GameFinished
}
