using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public GameState gameState { get; private set; }

    [Header("Internal")]
    [Range(0, 1)]
    [SerializeField] int mainSceneId = 0;
    [SerializeField] bool deleteData;

    public override void Init()
    {
        persistOnSceneLoad = false;

        base.Init();
    }

    private void Awake()
    {
        if (deleteData)
            PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        //MenuManager.instance.TogglePanel(PanelType.Menu, true);
        StartGame();
    }

    public void StartGame()
    {
        if (gameState == GameState.Started)
            return;
        gameState = GameState.Started;

        MenuManager.instance.TogglePanel(PanelType.GamePlay, true);
    }

    public void GameOver()
    {
        if (gameState == GameState.GameOver || gameState != GameState.Started)
            return;
        gameState = GameState.GameOver;

        MenuManager.instance.TogglePanel(PanelType.GameOver, true);
    }

    public void GameFinished()
    {
        if (gameState == GameState.GameFinished || gameState != GameState.Started)
            return;
        gameState = GameState.GameFinished;

        MenuManager.instance.TogglePanel(PanelType.GameFinished, true);
    }

    public void PlayNextLevel()
    {
        CleanUp();

        //SceneSelector.LoadScene(mainSceneId);
    }

    private void OnApplicationQuit()
    {
        CleanUp();
    }

    void CleanUp()
    {
        Timing.KillCoroutines();
    }
}

public enum GameState
{
    NotStarted,
    Started,
    GameOver,
    GameFinished
}
