using UnityEngine;

public class WebSpawner : MonoBehaviour
{
    [SerializeField] private Transform bee;
    [SerializeField] private SpiderCompanion spider;
    [SerializeField] private GameObject webPrefab;
    [SerializeField] private float interval = 15f;
    [SerializeField] private float rageInterval = 5f;
    [SerializeField] private float lifetime = 25f;

    private float timer;

    private void Update()
    {
        if (!GameFlow.Started || GameFlow.GameOver) return;

        timer += Time.deltaTime;

        // Базовый интервал, разделённый на множитель сложности
        float baseInterval = (spider != null && spider.Rage) ? rageInterval : interval;
        float currentInterval = baseInterval / DifficultyScaler.Instance.Multiplier;

        if (timer >= currentInterval)
        {
            timer = 0f;
            DropWeb();
        }
    }

    private void DropWeb()
    {
        Vector3 src = spider != null ? spider.transform.position : bee.position;
        Vector3 up = Planet.Instance.UpAt(src);
        Vector3 pos = Planet.Instance.SurfacePoint(src) + up * 0.05f;

        // заплатка лежит по нормали поверхности — на любой точке планеты
        if (!SpawnRegistry.IsFree(pos, 2f, 1.2f)) return;
        var web = Instantiate(webPrefab, pos, Quaternion.FromToRotation(Vector3.up, up));
        Destroy(web, lifetime);
    }
}