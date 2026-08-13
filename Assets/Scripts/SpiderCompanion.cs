using UnityEngine;

public class SpiderCompanion : MonoBehaviour
{
    [SerializeField] private Transform bee;
    [SerializeField] private float calmSpeed = 4.5f;
    [SerializeField] private float rageSpeed = 8f;
    [SerializeField] private float catchDistance = 1.2f;
    [SerializeField] private float knockBack = 10f;
    [SerializeField] private float hoverHeight = 0f; // паук бежит по земле, а не летит

    public bool Rage { get; private set; }

    private float calmUntil;

    private void Start() => KnockBack();

    private void LateUpdate()
    {
        if (!GameFlow.Started || GameFlow.GameOver) return;

        Vector3 up = Planet.Instance.UpAt(transform.position);
        Vector3 toBee = bee.position - transform.position;
        Vector3 dir = Vector3.ProjectOnPlane(toBee, up).normalized;

        // Безопасное получение множителя: если менеджера нет, считаем его равным 1
        float baseSpeed = Rage ? rageSpeed : calmSpeed;
        float multiplier = DifficultyScaler.Instance != null ? DifficultyScaler.Instance.Multiplier : 1f;
        float speed = baseSpeed * multiplier;

        // Если паук оглушён после удара, он не двигается, независимо от сложности
        if (Time.time < calmUntil) speed = 0f;

        transform.position += dir * (speed * Time.deltaTime);

        // ноги на траве: без hover и покачиваний, позиция постоянная
        transform.position = Planet.Instance.SurfacePoint(transform.position)
            + Planet.Instance.UpAt(transform.position) * hoverHeight;

        if (toBee.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toBee, up), Time.deltaTime * 6f);

        // ловим по горизонтали: пчела летит выше, вертикаль не входит в радиус
        float horizontal = Vector3.ProjectOnPlane(toBee, up).magnitude;
        if (horizontal < catchDistance && Time.time >= calmUntil)
            Catch();
    }

    private void Catch()
    {
        HealthSystem.Instance.TakeHit();
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

    public void SetRage(bool rage) => Rage = rage;
}