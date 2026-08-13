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

        keyboardButton.onClick.AddListener(() => Pick(ControlMode.Keyboard));
        joystickButton.onClick.AddListener(() => Pick(ControlMode.Joystick));
        gyroButton.onClick.AddListener(() => Pick(ControlMode.Gyro));

        // Показываем только то, что реально работает на этой платформе
        bool isMobile = Application.isMobilePlatform;
        bool isEditor = Application.isEditor;

        // Клавиатура: только на ПК (и не в редакторе с мобильной платформой)
        keyboardButton.gameObject.SetActive(!isMobile || isEditor);

        // Гироскоп: только на реальном мобильном устройстве (не в редакторе)
        gyroButton.gameObject.SetActive(!isEditor && SystemInfo.supportsGyroscope);

        gameObject.SetActive(false);
    }

    public void Open() => gameObject.SetActive(true);

    public void Close()
    {
        gameObject.SetActive(false);
        GameFlow.StartGame();
        Time.timeScale = 1f;
    }

    private void Pick(ControlMode mode)
    {
        ControlSettings.Current = mode;
        ControlSettings.HasChosen = true;
        Close();
    }
}