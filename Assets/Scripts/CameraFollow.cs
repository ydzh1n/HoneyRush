using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 6f;
    [SerializeField] private float height = 2.5f;
    [SerializeField] private float smooth = 5f;

    private void LateUpdate()
    {
        Vector3 up = Planet.Instance.UpAt(target.position);
        Vector3 back = -Vector3.ProjectOnPlane(target.forward, up).normalized;
        Vector3 desired = target.position + back * distance + up * height;

        transform.position = Vector3.Lerp(transform.position, desired, Time.unscaledDeltaTime * smooth);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(target.position - transform.position, up),
            Time.unscaledDeltaTime * smooth);
    }
}