using UnityEngine;

public class BeeController : MonoBehaviour
{
    [Header("Бег")]
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float accel = 3f;
    [SerializeField] private float turnSpeed = 140f;
    [SerializeField] private float steerSmooth = 10f;

    private float currentSpeed;
    private float currentSteer;

    private void FixedUpdate()
    {
        Vector3 up = Planet.Instance.UpAt(transform.position);

        // ввод: пока клавиатура, джойстик и гироскоп добавим следующим коммитом
        float steer = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) steer -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) steer += 1f;
        currentSteer = Mathf.Lerp(currentSteer, steer, Time.fixedDeltaTime * steerSmooth);

        // плавный разгон до скорости бега
        currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, Time.fixedDeltaTime * accel);

        // поворот вокруг нормали поверхности (рулежка)
        transform.Rotate(up, currentSteer * turnSpeed * Time.fixedDeltaTime, Space.World);

        // бег по касательной к сфере
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;
        transform.position += forward * (currentSpeed * Time.fixedDeltaTime);

        // держим на поверхности и выравниваем «верх» по нормали
        transform.position = Planet.Instance.SurfacePoint(transform.position);
        transform.rotation = Quaternion.FromToRotation(transform.up, up) * transform.rotation;
    }
}