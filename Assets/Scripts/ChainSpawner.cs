using UnityEngine;

public class ChainSpawner : MonoBehaviour
{
    [SerializeField] private Transform bee;
    [SerializeField] private NectarDrop dropPrefab;
    [SerializeField] private int dropsPerChain = 6;
    [SerializeField] private float step = 2f;
    [SerializeField] private float lateralRange = 4f;
    [SerializeField] private float spawnAhead = 14f;
    [SerializeField] private float chainInterval = 14f;

    private float distanceSinceLast;
    private Vector3 lastPos;

    private void Start()
    {
        lastPos = bee.position;
        SpawnChain();
    }

    private void Update()
    {
        distanceSinceLast += Vector3.Distance(bee.position, lastPos);
        lastPos = bee.position;
        if (distanceSinceLast >= chainInterval)
        {
            distanceSinceLast = 0f;
            SpawnChain();
        }
    }

    private void SpawnChain()
    {
        Vector3 up = Planet.Instance.UpAt(bee.position);
        Vector3 forward = Vector3.ProjectOnPlane(bee.forward, up).normalized;
        Vector3 right = Vector3.Cross(forward, up);
        float lateral = Random.Range(-lateralRange, lateralRange);

        for (int i = 0; i < dropsPerChain; i++)
        {
            Vector3 pos = bee.position + forward * (spawnAhead + i * step);
            pos = Planet.Instance.SurfacePoint(pos) + Planet.Instance.UpAt(pos) * 0.6f;
            pos += right * lateral;
            pos = Planet.Instance.SurfacePoint(pos) + Planet.Instance.UpAt(pos) * 0.6f;
            Instantiate(dropPrefab, pos, Quaternion.identity);
        }
    }
}