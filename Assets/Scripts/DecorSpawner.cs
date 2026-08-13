using UnityEngine;

public class DecorSpawner : MonoBehaviour
{
    [SerializeField] private Transform decorRoot;
    [SerializeField] private GameObject[] decorPrefabs;
    [SerializeField] private int count = 200;
    [SerializeField] private float scaleMin = 0.7f;
    [SerializeField] private float scaleMax = 1.3f;

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var prefab = decorPrefabs[Random.Range(0, decorPrefabs.Length)];
            Vector3 pos = Planet.Instance.SurfacePoint(Random.onUnitSphere * 50f);
            Vector3 up = Planet.Instance.UpAt(pos);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, up)
                             * Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            var item = Instantiate(prefab, pos, rot, decorRoot);
            item.transform.localScale *= Random.Range(scaleMin, scaleMax);
            item.isStatic = true; // подсказка юнити батчить статику
        }
    }
}