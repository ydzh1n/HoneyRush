using UnityEngine;

public enum BonusType { Magnet, Shield, Dash }

public static class BonusActivator
{
    public static event System.Action<BonusType> OnBonusActivated;

    public static void Activate(BonusType type)
    {
        Debug.Log($"Bonus activated: {type}");
        OnBonusActivated?.Invoke(type);
    }
}