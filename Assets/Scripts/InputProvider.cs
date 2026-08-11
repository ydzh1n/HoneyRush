using UnityEngine;

public class InputProvider : MonoBehaviour
{
    public static InputProvider Instance;

    public float Steer { get; private set; }
    public float Throttle { get; private set; }

    [SerializeField] private SimpleJoystick joystick;
    [SerializeField] private GameObject joystickCanvas;

    [Header("Гироскоп")]
    [SerializeField] private float gyroSensitivity = 2.5f;
    [SerializeField] private bool gyroInvert = false;

    private bool gyroAvailable;
    private Quaternion gyroCalibration;

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
            gyroCalibration = Input.gyro.attitude;
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
                    steer = joystick.AxisX;
                    throttle = joystick.AxisY;
                }
                break;

            case ControlMode.Gyro:
                if (gyroAvailable)
                {
                    Quaternion delta = Quaternion.Inverse(gyroCalibration) * Input.gyro.attitude;
                    Vector3 tilt = delta * Vector3.forward;
                    float s = tilt.x * gyroSensitivity;
                    if (gyroInvert) s = -s;
                    steer = Mathf.Clamp(s, -1f, 1f);
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && ControlPicker.Instance != null)
            ControlPicker.Instance.Open();

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
        bool need = GameFlow.Started
                    && ControlSettings.Current == ControlMode.Joystick
                    && !pickerOpen
                    && !bonusOpen;
        if (joystickCanvas.activeSelf != need)
            joystickCanvas.SetActive(need);
    }
}