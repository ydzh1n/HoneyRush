public static class GameFlow
{
    public static bool Started { get; private set; }

    public static void StartGame() => Started = true;
}