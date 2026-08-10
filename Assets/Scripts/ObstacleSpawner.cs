using System.Collections.Generic;
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

    private readonly List<Transform> alive = new List<Transform>();
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
        alive.RemoveAll(t => t == null); // убираем уничтожЄнные (Destroy даЄт null)

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

                if (TooClose(pos)) continue; // зан€то Ч пробуем другой бок

                var mushroom = Instantiate(mushroomPrefab, pos, Quaternion.identity);
                mushroom.transform.rotation = Quaternion.FromToRotation(Vector3.up, Planet.Instance.UpAt(pos));
                Destroy(mushroom, 30f);
                alive.Add(mushroom.transform);
                break;
            }
        }
    }

    private bool TooClose(Vector3 pos)
    {
        for (int i = 0; i < alive.Count; i++)
        {
            if (Vector3.Distance(alive[i].position, pos) < minGap)
                return true;
        }
        return false;
    }
}