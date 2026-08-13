public static class MagnetState
{
    public static bool Active { get; private set; }
    public static float Radius = 6f;

    public static void Start() => Active = true;
    public static void Stop() => Active = false;
    public static void Reset() => Active = false;
}