using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameRestart
{
    public static void Run() // сразу в забег
    {
        GameFlow.FromRestart = true;
        ToMenu();
    }

    public static void ToMenu() // чистая загрузка сцены — меню покажется само
    {
        Time.timeScale = 1f;
        GameFlow.ResetGame();
        MagnetState.Reset();
        ShieldState.Reset();
        DashState.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}