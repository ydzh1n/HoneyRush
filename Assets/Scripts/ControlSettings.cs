using UnityEngine;

public enum ControlMode { Keyboard, Joystick, Gyro }

public static class ControlSettings
{
    // Читаем и сохраняем текущий режим управления
    public static ControlMode Current
    {
        get => (ControlMode)PlayerPrefs.GetInt("ControlMode", (int)ControlMode.Keyboard);
        set => PlayerPrefs.SetInt("ControlMode", (int)value);
    }

    // Добавлен сеттер (set), чтобы мы могли менять значение из ControlPicker
    public static bool HasChosen
    {
        get => PlayerPrefs.GetInt("HasChosen", 0) == 1;
        set => PlayerPrefs.SetInt("HasChosen", value ? 1 : 0);
    }
}