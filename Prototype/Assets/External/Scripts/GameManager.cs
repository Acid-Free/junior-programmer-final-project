using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    public int score;

    public int levelCount;
    public int levelPlantCount;
    public float levelDuration;
    public float levelElapsedTime;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.Start);
    }

    void Update()
    {
        levelElapsedTime += Time.deltaTime;
        if (levelElapsedTime >= levelDuration && State == GameState.InGame)
        {
            score += 10;
            UnitManager.Instance.DestroyAllUnits();
            UpdateGameState(GameState.LevelDesc);
        }
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch(newState)
        {
            case GameState.Restart:
                RestartGame();
                break;
            case GameState.Start:
                break;
            case GameState.LevelDesc:
                SetupLevel();
                break;
            case GameState.InGame:
                levelElapsedTime = 0;
                SpawnUnits();
                break;
            case GameState.GameOver:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    void SetupLevel()
    {
        ++levelCount;
        levelPlantCount += UnityEngine.Random.Range(1, 3);

        // Level 1 will always have one cactus
        if (levelCount == 1)
        {
            levelPlantCount = 1;
        }

        levelDuration += 5 + UnityEngine.Random.Range(-2, 2f) * 3;
        levelDuration = Mathf.Clamp((int)levelDuration, 5, 15);
    }

    void SpawnUnits()
    {
        UnitManager.Instance.SpawnPlayer();
        UnitManager.Instance.SpawnPlants();
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UpdateGameState(GameState.Start);
    }
}

public enum GameState
{
    Restart,
    Start,
    LevelDesc,
    InGame,
    GameOver
}
