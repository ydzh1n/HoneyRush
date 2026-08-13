using UnityEngine;

public class InputProvider : MonoBehaviour
{
    public static InputProvider Instance;

    public float Steer { get; private set; }
    public float Throttle { get; private set; }

    [SerializeField] private SimpleJoystick joystick;
    [SerializeField] private GameObject joystickCanvas;

    [Header("Гироскоп")]
    // Чувствительность теперь маленькая: 0.05 означает, что наклон на 20 градусов = полный поворот (20 * 0.05 = 1)
    [SerializeField] private float gyroSensitivity = 0.05f;
    [SerializeField] private bool gyroInvert = false;
    [SerializeField] private float gyroDeadzone = 3f;      // Мёртвая зона в градусах (не реагируем на дрожь рук)
    [SerializeField] private float gyroRecenter = 2f;      // Скорость компенсации дрейфа (градусов в секунду)

    private bool gyroAvailable;
    private float currentGyroZero; // Динамический "ноль" для компенсации дрейфа

    private void Awake()
    {
        Instance = this;
        gyroAvailable = SystemInfo.supportsGyroscope;
        if (gyroAvailable)
        {
            Input.gyro.enabled = true;
            CalibrateGyro();
        }
    }

    public void CalibrateGyro()
    {
        if (gyroAvailable)
        {
            float z = Input.gyro.attitude.eulerAngles.z;
            if (z > 180f) z -= 360f; // Нормализуем в диапазон -180..180
            currentGyroZero = z;
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
                    // Квадратичная кривая: смягчает центр, сохраняя полный ход по краям
                    steer = joystick.AxisX * Mathf.Abs(joystick.AxisX);
                    throttle = joystick.AxisY * Mathf.Abs(joystick.AxisY);
                }
                break;

            case ControlMode.Gyro:
                if (gyroAvailable)
                {
                    float z = Input.gyro.attitude.eulerAngles.z;
                    if (z > 180f) z -= 360f; // Нормализуем в -180..180

                    float delta = z - currentGyroZero;

                    if (Mathf.Abs(delta) <= gyroDeadzone)
                    {
                        // Держим ровно: руль в нуле, а "ноль" медленно подтягивается к реальному углу, убирая дрейф
                        currentGyroZero = Mathf.MoveTowards(currentGyroZero, z, gyroRecenter * Time.unscaledDeltaTime);
                        steer = 0f;
                    }
                    else
                    {
                        // Осознанный наклон: вычитаем мёртвую зону, чтобы не было рывка на границе
                        float s = (delta - Mathf.Sign(delta) * gyroDeadzone) * gyroSensitivity;
                        if (gyroInvert) s = -s; // Если телефон инвертирует оси, эта галочка всё исправит
                        steer = Mathf.Clamp(s, -1f, 1f);
                    }
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && PauseUI.Instance != null)
        {
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