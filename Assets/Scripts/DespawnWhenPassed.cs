using UnityEngine;

public class DespawnWhenPassed : MonoBehaviour
{
    [SerializeField] private float behindMargin = 5f;

    private Transform bee;
    private bool wasAhead;

    private void Awake() => bee = GameObject.FindGameObjectWithTag("Player").transform;

    private void Update()
    {
        Vector3 up = Planet.Instance.UpAt(bee.position);
        Vector3 forward = Vector3.ProjectOnPlane(bee.forward, up).normalized;
        float ahead = Vector3.Dot(Vector3.ProjectOnPlane(transform.position - bee.position, up), forward);

        if (ahead > 0f) wasAhead = true;
        else if (wasAhead && ahead < -behindMargin) Destroy(gameObject);
    }
}