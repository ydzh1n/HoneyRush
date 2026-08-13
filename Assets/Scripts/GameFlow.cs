using UnityEngine;

public static class GameFlow
{
    public static bool Started { get; private set; }
    public static bool GameOver { get; private set; }
    public static bool FromRestart;
    public static event System.Action OnGameEnded;

    public static void StartGame() => Started = true;

    public static void EndGame()
    {
        GameOver = true;
        Time.timeScale = 0f;
        OnGameEnded?.Invoke();
    }

    public static void ResetGame()
    {
        Started = false;
        GameOver = false;
    }
}