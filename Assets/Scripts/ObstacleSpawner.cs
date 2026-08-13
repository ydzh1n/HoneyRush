using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Transform bee;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private float spawnAhead = 30f;
    [SerializeField] private float interval = 12f;
    [SerializeField] private float lateralRange = 6f;
    [SerializeField] private float minGap = 3f;
    [SerializeField] private int attempts = 5;

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
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 up = Planet.Instance.UpAt(bee.position);
        Vector3 forward = Vector3.ProjectOnPlane(bee.forward, up).normalized;
        Vector3 right = Vector3.Cross(forward, up);

        int count = Random.Range(1, 3);
        for (int i = 0; i < count; i++)
        {
            for (int attempt = 0; attempt < attempts; attempt++)
            {
                float lateral = Random.Range(-lateralRange, lateralRange);
                Vector3 pos = bee.position + forward * spawnAhead;
                pos = Planet.Instance.SurfacePoint(pos);
                pos += right * lateral;
                pos = Planet.Instance.SurfacePoint(pos);

                // Проверяем реестр: чтобы не спавнилось в других грибах (minGap) и не перекрывало капли (1.2f)
                if (!SpawnRegistry.IsFree(pos, minGap, 1.2f)) continue;

                var mushroom = Instantiate(mushroomPrefab, pos, Quaternion.identity);
                mushroom.transform.rotation = Quaternion.FromToRotation(Vector3.up, Planet.Instance.UpAt(pos));
                Destroy(mushroom, 30f); // оставляем как страховку
                break;
            }
        }
    }
}