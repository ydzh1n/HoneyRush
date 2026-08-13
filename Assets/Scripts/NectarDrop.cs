using UnityEngine;

public class NectarDrop : MonoBehaviour
{
    [SerializeField] private float bobHeight = 0.15f;
    [SerializeField] private float bobSpeed = 3f;
    [SerializeField] private float spinSpeed = 120f;
    [SerializeField] private float lifetime = 25f;
    [SerializeField] private float magnetSpeed = 8f;

    private Vector3 basePos;
    private float phase;
    private Transform bee;

    private void Start()
    {
        basePos = transform.position;
        phase = Random.Range(0f, Mathf.PI * 2f);
        bee = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        Vector3 up = Planet.Instance.UpAt(basePos);

        if (MagnetState.Active && Vector3.Distance(basePos, bee.position) < MagnetState.Radius)
        {
            basePos = Vector3.MoveTowards(basePos, bee.position, magnetSpeed * Time.deltaTime);
            up = Planet.Instance.UpAt(basePos);
        }

        transform.position = basePos + up * (Mathf.Sin(Time.time * bobSpeed + phase) * bobHeight);
        transform.Rotate(up, spinSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (ComboTracker.Instance != null) ComboTracker.Instance.RegisterPickup();
        ScoreManager.Instance.AddDrop();
        Destroy(gameObject);
    }
}