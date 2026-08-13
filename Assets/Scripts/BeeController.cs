using UnityEngine;

public class BeeController : MonoBehaviour
{
    [Header("Бег")]
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float accel = 3f;
    [SerializeField] private float turnSpeed = 140f;
    [SerializeField] private float steerSmooth = 10f;
    [SerializeField] private float hoverHeight = 1.1f;

    private float currentSpeed;
    private float currentSteer;

    private void FixedUpdate()
    {
        if (!GameFlow.Started || GameFlow.GameOver) return;
        Vector3 up = Planet.Instance.UpAt(transform.position);

        currentSteer = Mathf.Lerp(currentSteer, InputProvider.Instance.Steer, Time.fixedDeltaTime * steerSmooth);

        float targetSpeed = runSpeed * (1f + 0.4f * InputProvider.Instance.Throttle);
        if (DashState.Active) targetSpeed *= DashState.Multiplier; // рывок поверх газа
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * accel);

        // поворот вокруг нормали поверхности (рулежка)
        transform.Rotate(up, currentSteer * turnSpeed * Time.fixedDeltaTime, Space.World);

        // бег по касательной к сфере
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;
        transform.position += forward * (currentSpeed * Time.fixedDeltaTime);

        // держим на поверхности и выравниваем «верх» по нормали
        transform.position = Planet.Instance.SurfacePoint(transform.position) + up * hoverHeight;
        transform.rotation = Quaternion.FromToRotation(transform.up, up) * transform.rotation;
    }
}