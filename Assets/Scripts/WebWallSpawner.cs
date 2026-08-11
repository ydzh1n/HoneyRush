using UnityEngine;

public class WebWallSpawner : MonoBehaviour
{
    [SerializeField] private Transform bee;
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private float spawnAhead = 30f;
    [SerializeField] private float interval = 26f;
    [SerializeField] private float lateralRange = 6f;
    [SerializeField] private float segmentWidth = 2f;
    [SerializeField] private float gapWidth = 3.2f;

    private float distance;
    private Vector3 lastPos;

    private void Start() => lastPos = bee.position;

    private void Update()
    {
        if (!GameFlow.Started || GameFlow.GameOver) return;

        distance += Vector3.Distance(bee.position, lastPos);
        lastPos = bee.position;

        if (distance >= interval)
        {
            distance = 0f;
            SpawnWall();
        }
    }

    private void SpawnWall()
    {
        Vector3 up = Planet.Instance.UpAt(bee.position);
        Vector3 forward = Vector3.ProjectOnPlane(bee.forward, up).normalized;
        Vector3 right = Vector3.Cross(forward, up);

        // проход всегда целиком внутри трассы
        float gapCenter = Random.Range(-lateralRange + gapWidth, lateralRange - gapWidth);

        for (float lateral = -lateralRange; lateral <= lateralRange; lateral += segmentWidth)
        {
            if (Mathf.Abs(lateral - gapCenter) < gapWidth / 2f) continue; // гарантированный проход

            Vector3 pos = bee.position + forward * spawnAhead;
            pos = Planet.Instance.SurfacePoint(pos);
            pos += right * lateral;
            pos = Planet.Instance.SurfacePoint(pos);

            var segment = Instantiate(segmentPrefab, pos, Quaternion.identity);
            segment.transform.rotation = Quaternion.LookRotation(forward, Planet.Instance.UpAt(pos));
            segment.transform.position += Planet.Instance.UpAt(pos) * 0.8f; // стена «стоит» на траве
            Destroy(segment, 30f);
        }
    }
}