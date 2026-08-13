using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;

    [SerializeField] private int maxLives = 3;
    [SerializeField] private float invulnerability = 2f;
    [SerializeField] private LivesHUD hud;

    public int Lives { get; private set; }
    public bool IsInvulnerable => Time.time < invulnerableUntil;

    private float invulnerableUntil;

    private void Awake()
    {
        Instance = this;
        Lives = maxLives;
    }

    private void Start() => hud.SetLives(Lives);

    public void TakeHit()
    {
        if (GameFlow.GameOver) return;
        if (IsInvulnerable) return;
        if (AudioManager.Instance != null) AudioManager.Instance.Hit();

        if (ShieldState.Active)
        {
            ShieldState.Consume(); // щит сгорел, жизнь цела
            invulnerableUntil = Time.time + invulnerability;
            if (HitFeedback.Instance != null) HitFeedback.Instance.OnHit();
            return;
        }

        Lives--;
        hud.SetLives(Lives);
        invulnerableUntil = Time.time + invulnerability;

        if (HitFeedback.Instance != null) HitFeedback.Instance.OnHit();

        if (Lives <= 0)
            GameFlow.EndGame();
    }
}