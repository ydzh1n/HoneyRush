using UnityEngine;
using UnityEngine.UI;

public class ControlPicker : MonoBehaviour
{
    public static ControlPicker Instance;

    [SerializeField] private Button keyboardButton;
    [SerializeField] private Button joystickButton;
    [SerializeField] private Button gyroButton;

    private void Awake()
    {
        Instance = this;

        if (!SystemInfo.supportsGyroscope)
            gyroButton.gameObject.SetActive(false);

        keyboardButton.onClick.AddListener(() => Pick(ControlMode.Keyboard));
        joystickButton.onClick.AddListener(() => Pick(ControlMode.Joystick));
        gyroButton.onClick.AddListener(() => Pick(ControlMode.Gyro));

        if (ControlSettings.HasChosen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f; // мир замирает, пока игрок выбирает
    }

    private void Pick(ControlMode mode)
    {
        ControlSettings.Current = mode;
        PlayerPrefs.Save();
        if (mode == ControlMode.Gyro && InputProvider.Instance != null)
            InputProvider.Instance.CalibrateGyro(); // точка нуля = поза в момент выбора
        Close();
    }

    private void Close()
    {
        GameFlow.StartGame();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}