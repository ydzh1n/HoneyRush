using UnityEngine;
using System.Collections;

public class ChainSpawner : MonoBehaviour
{
    [SerializeField] private Transform bee;
    [SerializeField] private NectarDrop dropPrefab;
    [SerializeField] private int dropsPerChain = 5;
    [SerializeField] private float step = 1.8f;
    [SerializeField] private float lateralRange = 4f;
    [SerializeField] private float spawnAhead = 20f;
    [SerializeField] private float chainInterval = 14f;
    [SerializeField] private float dropSpawnDelay = 0.15f;

    private float distanceSinceLast;
    private Vector3 lastPos;
    private bool spawnedFirst;
    private bool isSpawning;

    private void Start()
    {
        lastPos = bee.position;
    }

    private void Update()
    {
        if (!GameFlow.Started) return;

        if (!spawnedFirst)
        {
            spawnedFirst = true;
            SpawnChain();
        }

        distanceSinceLast += Vector3.Distance(bee.position, lastPos);
        lastPos = bee.position;

        if (distanceSinceLast >= chainInterval && !isSpawning)
        {
            distanceSinceLast = 0f;
            SpawnChain();
        }
    }

    private void SpawnChain()
    {
        if (isSpawning) return;
        StartCoroutine(SpawnChainCoroutine());
    }

    private IEnumerator SpawnChainCoroutine()
    {
        isSpawning = true;

        Vector3 center = Planet.Instance.transform.position;
        Vector3 up = Planet.Instance.UpAt(bee.position);
        Vector3 forward = Vector3.ProjectOnPlane(bee.forward, up).normalized;

        // Ось вращения для движения ВПЕРЁД по сфере (правило правой руки)
        Vector3 right = Vector3.Cross(forward, up);
        float lateral = Random.Range(-lateralRange, lateralRange);

        // Базовый вектор направления от центра планеты к пчеле
        Vector3 beeDir = (bee.position - center).normalized;

        for (int i = 0; i < dropsPerChain; i++)
        {
            // 1. Считаем расстояние и угол для i-й капли (от ближней i=0 к дальней)
            float distance = spawnAhead + (i * step);
            float angleRad = distance / Planet.Instance.radius;
            float angleDeg = angleRad * Mathf.Rad2Deg;

            // 2. Поворачиваем вектор направления ВПЕРЁД по сфере на нужный угол
            Vector3 targetDir = Quaternion.AngleAxis(angleDeg, right) * beeDir;

            // 3. Получаем точку на поверхности сферы
            Vector3 pos = center + targetDir * Planet.Instance.radius;

            // 4. Применяем боковое смещение и снова проецируем на сферу (чтобы не уйти под землю)
            pos += right * lateral;
            pos = Planet.Instance.SurfacePoint(pos);

            // 5. Приподнимаем над поверхностью
            pos += Planet.Instance.UpAt(pos) * 0.6f;

            // 6. Спавним, если место свободно
            if (SpawnRegistry.IsFree(pos, 1f, 0f))
            {
                Instantiate(dropPrefab, pos, Quaternion.identity);
            }

            // 7. Ждём перед спавном следующей (более дальней) капли
            if (i < dropsPerChain - 1)
            {
                yield return new WaitForSeconds(dropSpawnDelay);
            }
        }

        isSpawning = false;
    }
}