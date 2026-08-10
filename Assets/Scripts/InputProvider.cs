using UnityEngine;

public class InputProvider : MonoBehaviour
{
    public static InputProvider Instance;


    public float Steer { get; private set; }
    public float Throttle { get; private set; }

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
                // следующий коммит
                break;

            case ControlMode.Gyro:
                // следующий коммит
                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && ControlPicker.Instance != null)
            ControlPicker.Instance.Open();

        Steer = steer;
        Throttle = throttle;

        #if UNITY_EDITOR
                // дев-клавиша: сброс сохранения, чтобы гонять сценарий первого запуска
                if (Input.GetKeyDown(KeyCode.F1))
                    PlayerPrefs.DeleteAll();
#endif
    }
}