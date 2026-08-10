using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Transform bee;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private float spawnAhead = 30f;
    [SerializeField] private float interval = 12f;
    [SerializeField] private float lateralRange = 6f;

    private float distance;
    private Vector3 lastPos;

    private void Start() => lastPos = bee.position;

    private void Update()
    {
        if (!GameFlow.Started) return;

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

        int count = Random.Range(1, 3); // один-два гриба на волну
        for (int i = 0; i < count; i++)
        {
            float lateral = Random.Range(-lateralRange, lateralRange);
            Vector3 pos = bee.position + forward * spawnAhead;
            pos = Planet.Instance.SurfacePoint(pos);
            pos += right * lateral;
            pos = Planet.Instance.SurfacePoint(pos);

            var mushroom = Instantiate(mushroomPrefab, pos, Quaternion.identity);
            // Ђшл€пой от небаї: локальный вверх гриба = нормаль поверхности
            mushroom.transform.rotation = Quaternion.FromToRotation(Vector3.up, Planet.Instance.UpAt(pos));

            Destroy(mushroom, 30f); // не копим мусор за спиной
        }
    }
}