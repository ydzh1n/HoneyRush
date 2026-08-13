using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameRestart
{
    public static void Run()
    {
        GameFlow.FromRestart = true;
        Time.timeScale = 1f;
        GameFlow.ResetGame();
        MagnetState.Reset();
        ShieldState.Reset();
        DashState.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}