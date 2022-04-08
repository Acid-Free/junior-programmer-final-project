using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject levelDescScreen;
    [SerializeField] GameObject inGameScreen;
    [SerializeField] GameObject gameOverScreen;

    [SerializeField] Button startButton;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI levelDescText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI cactiText;
    [SerializeField] Button restartButton;

    void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    void GameManagerOnOnGameStateChanged(GameState state)
    {
        switch(state)
        {
            case GameState.Start:
                SetScreens(true, false, false, false);
                break;
            case GameState.LevelDesc:
                SetupLevelDescScreen();
                SetScreens(false, true, false, false);
                Invoke("StartLevel", 3.0f);
                break;
            case GameState.InGame:
                SetScreens(false, false, true, false);
                break;
            case GameState.GameOver:
                SetScreens(false, false, true, true);
                break;
        }
    }

    void SetScreens(bool screen1, bool screen2, bool screen3, bool screen4)
    {
        startScreen.SetActive(screen1);
        levelDescScreen.SetActive(screen2);
        inGameScreen.SetActive(screen3);
        gameOverScreen.SetActive(screen4);
    }

    void Start()
    {
        startButton.onClick.AddListener(StartGamePressed);
        restartButton.onClick.AddListener(RestartGamePressed);
    }

    void Update()
    {
        if (GameManager.Instance.State == GameState.InGame)
        {
            scoreText.SetText("Score\n" + GameManager.Instance.score);
            timeText.SetText("Time\n" + Mathf.RoundToInt(GameManager.Instance.levelDuration - GameManager.Instance.levelElapsedTime));
        }
    }

    void SetupLevelDescScreen()
    {
        int level =  GameManager.Instance.levelCount;
        int plantCount = GameManager.Instance.levelPlantCount;
        float duration = GameManager.Instance.levelDuration;
        levelText.SetText("Level " + level);
        levelDescText.SetText((level == 1 ? "Water the cactus" : "Water all " + plantCount + " Cacti") + "\nfor " + duration + " seconds");
        cactiText.SetText((plantCount == 1 ? "Cactus\n1" : "Cacti\n" + plantCount));
    }

    void StartLevel()
    {
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }

    void StartGamePressed()
    {
        GameManager.Instance.UpdateGameState(GameState.LevelDesc);
    }

    void RestartGamePressed()
    {
        GameManager.Instance.UpdateGameState(GameState.Restart);
    }
}
