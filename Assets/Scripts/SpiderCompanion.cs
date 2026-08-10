using UnityEngine;

public class SpiderCompanion : MonoBehaviour
{
    [SerializeField] private Transform bee;
    [SerializeField] private float calmSpeed = 4.5f;  // медленнее пчелы (6) Ч отстаЄт
    [SerializeField] private float rageSpeed = 8f;    // быстрее пчелы Ч догон€ет
    [SerializeField] private float catchDistance = 1.3f;
    [SerializeField] private float knockBack = 10f;
    [SerializeField] private float hoverHeight = 1f;
    [SerializeField] private float bobSpeed = 3f;
    [SerializeField] private float bobHeight = 0.15f;

    public bool Rage { get; private set; }

    private float phase;
    private float calmUntil;

    private void Start()
    {
        phase = Random.Range(0f, Mathf.PI * 2f);
        KnockBack(); // стартуем позади пчелы
    }

    private void LateUpdate()
    {
        if (!GameFlow.Started || GameFlow.GameOver) return;

        Vector3 up = Planet.Instance.UpAt(transform.position);
        Vector3 toBee = bee.position - transform.position;
        Vector3 dir = Vector3.ProjectOnPlane(toBee, up).normalized;

        float speed = Rage ? rageSpeed : calmSpeed;
        if (Time.time < calmUntil) speed = 0f; // пауза после поимки

        transform.position += dir * (speed * Time.deltaTime);

        // на поверхность + покачивание
        transform.position = Planet.Instance.SurfacePoint(transform.position)
            + Planet.Instance.UpAt(transform.position) * (hoverHeight + Mathf.Sin(Time.time * bobSpeed + phase) * bobHeight);

        if (toBee.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toBee, up), Time.deltaTime * 6f);

        if (toBee.magnitude < catchDistance && Time.time >= calmUntil)
            Catch();
    }

    private void Catch()
    {
        HealthSystem.Instance.TakeHit(); // неу€звимость учтена внутри
        KnockBack();
        calmUntil = Time.time + 1f;
    }

    public void KnockBack()
    {
        Vector3 up = Planet.Instance.UpAt(bee.position);
        Vector3 forward = Vector3.ProjectOnPlane(bee.forward, up).normalized;
        Vector3 pos = Planet.Instance.SurfacePoint(bee.position - forward * knockBack);
        transform.position = pos + Planet.Instance.UpAt(pos) * hoverHeight;
    }

    public void SetRage(bool rage) => Rage = rage; // пригодитс€ в коммите €рости
}