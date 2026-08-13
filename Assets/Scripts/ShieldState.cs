public static class ShieldState
{
    public static bool Active { get; private set; }
    public static event System.Action<bool> OnChanged;

    public static void Gain()
    {
        Active = true;
        OnChanged?.Invoke(true);
    }

    public static void Consume()
    {
        Active = false;
        OnChanged?.Invoke(false);
    }
    public static void Reset() 
    { 
        Active = false;
        OnChanged?.Invoke(false);
    }
}