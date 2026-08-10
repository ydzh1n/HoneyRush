using UnityEngine;

public class InputProvider : MonoBehaviour
{
    public static InputProvider Instance;

    public float Steer { get; private set; }
    public float Throttle { get; private set; }

    [SerializeField] private SimpleJoystick joystick;
    [SerializeField] private GameObject joystickCanvas;

    private void Awake() => Instance = this;

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
                // следующий коммит
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
        bool need = GameFlow.Started
                    && ControlSettings.Current == ControlMode.Joystick
                    && !pickerOpen;
        if (joystickCanvas.activeSelf != need)
            joystickCanvas.SetActive(need);
    }
}