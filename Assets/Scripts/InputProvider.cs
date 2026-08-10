using UnityEngine;

public class InputProvider : MonoBehaviour
{
    public static InputProvider Instance;

    public float Steer { get; private set; }     // -1..1 руль
    public float Throttle { get; private set; }  // -1..1 газ/тормоз

    private void Awake() => Instance = this;

    private void Update()
    {
        float steer = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) steer -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) steer += 1f;

        float throttle = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) throttle += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) throttle -= 1f;

        Steer = steer;
        Throttle = throttle;
    }
}