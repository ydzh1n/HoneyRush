using UnityEngine;

public class Planet : MonoBehaviour
{
    public static Planet Instance;

    [SerializeField] private float radius = 10f;
    public float Radius => radius;

    private void Awake() => Instance = this;

    public Vector3 UpAt(Vector3 worldPos) =>
        (worldPos - transform.position).normalized;

    public Vector3 SurfacePoint(Vector3 worldPos) =>
        transform.position + UpAt(worldPos) * radius;
}