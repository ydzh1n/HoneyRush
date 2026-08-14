using UnityEngine;

public class InputProvider : MonoBehaviour
{
    public static InputProvider Instance;

    public float Steer { get; private set; }
    public float Throttle { get; private set; }

    [SerializeField] private SimpleJoystick joystick;
    [SerializeField] private GameObject joystickCanvas;

    [Header("Гироскоп")]
    // Чувствительность для вектора гравитации (диапазон -1..1). 1.2 = чуть резче, 0.8 = плавнее
    [SerializeField] private float gyroSensitivity = 1.2f;
    [SerializeField] private bool gyroInvert = false;
    // Мёртвая зона для вектора гравитации (0.05 - 0.15). Не в градусах!
    [SerializeField] private float gyroDeadzone = 0.1f;

    private bool gyroAvailable;

    private void Awake()
    {
        Instance = this;
        gyroAvailable = SystemInfo.supportsGyroscope;
        if (gyroAvailable)
        {
            Input.gyro.enabled = true;
        }
    }

    private void Update()
    {
        float steer = 0f;
        float throttle = 0f;

        switch (ControlSettings.Current)
        {
            case ControlMode.Keyboard:
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) steer -= 1f;
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) steer += 1f;
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) throttle += 1f;
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) throttle -= 1f;
                break;

            case ControlMode.Joystick:
                if (joystick != null)
                {
                    steer = joystick.AxisX * Mathf.Abs(joystick.AxisX);
                    throttle = joystick.AxisY * Mathf.Abs(joystick.AxisY);
                }
                break;

            case ControlMode.Gyro:
                if (gyroAvailable)
                {
                    // В Landscape Left наклон ВЛЕВО-ВПРАВО меняет ось X гравитации, а не Y!
                    // Когда телефон лежит плоско: gravity.x = 0
                    // Наклон влево: gravity.x > 0
                    // Наклон вправо: gravity.x < 0
                    float rawTilt = -Input.gyro.gravity.x; // Инвертируем для интуитивности

                    if (Mathf.Abs(rawTilt) <= gyroDeadzone)
                    {
                        // Телефон держится ровно: руль в нуле
                        steer = 0f;
                    }
                    else
                    {
                        // Осознанный наклон: компенсируем мёртвую зону
                        float adjustedTilt = rawTilt - Mathf.Sign(rawTilt) * gyroDeadzone;
                        float maxRange = 1f - gyroDeadzone;

                        // Нормализуем и применяем чувствительность
                        float s = (adjustedTilt / maxRange) * gyroSensitivity;

                        if (gyroInvert) s = -s;
                        steer = Mathf.Clamp(s, -1f, 1f);
                    }
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && PauseUI.Instance != null)
        {
            if (GameFlow.GameOver) return;
            if (PauseUI.Instance.IsOpen) PauseUI.Instance.Resume();
            else PauseUI.Instance.Pause();
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
            PlayerPrefs.DeleteAll();
#endif

        UpdateJoystickVisibility();

        Steer = steer;
        Throttle = throttle;
    }

    private void UpdateJoystickVisibility()
    {
        bool pickerOpen = ControlPicker.Instance != null && ControlPicker.Instance.gameObject.activeSelf;
        bool bonusOpen = BonusPicker.Instance != null && BonusPicker.Instance.gameObject.activeSelf;
        bool pauseOpen = PauseUI.Instance != null && PauseUI.Instance.IsOpen;
        bool need = GameFlow.Started
                    && !GameFlow.GameOver
                    && ControlSettings.Current == ControlMode.Joystick
                    && !pickerOpen
                    && !bonusOpen
                    && !pauseOpen;
        if (joystickCanvas.activeSelf != need)
            joystickCanvas.SetActive(need);
    }
}