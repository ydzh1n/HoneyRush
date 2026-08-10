using UnityEngine;

public enum ControlMode { Keyboard, Joystick, Gyro }

public static class ControlSettings
{
    private const string Key = "ControlMode";

    public static bool HasChosen => PlayerPrefs.HasKey(Key);

    public static ControlMode Current
    {
        get => (ControlMode)PlayerPrefs.GetInt(Key, (int)ControlMode.Keyboard);
        set => PlayerPrefs.SetInt(Key, (int)value);
    }
}