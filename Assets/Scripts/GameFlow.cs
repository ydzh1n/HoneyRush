using UnityEngine;

public static class GameFlow
{
    public static bool Started { get; private set; }
    public static bool GameOver { get; private set; }

    public static void StartGame() => Started = true;

    public static void EndGame()
    {
        GameOver = true;
        Time.timeScale = 0f; // мир замирает, как перед коконом
    }

    public static void ResetGame() // пригодится для кнопки «ещё раз»
    {
        Started = false;
        GameOver = false;
    }
}