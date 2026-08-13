public static class DashState
{
    public static bool Active { get; private set; }
    public static float Multiplier = 1.8f;

    public static void Start() => Active = true;
    public static void Stop() => Active = false;
    public static void Reset() => Active = false;
}