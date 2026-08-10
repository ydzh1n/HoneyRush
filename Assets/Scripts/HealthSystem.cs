using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;

    [SerializeField] private int maxLives = 3;
    [SerializeField] private float invulnerability = 2f;
    [SerializeField] private LivesHUD hud;

    public int Lives { get; private set; }

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
        if (Time.time < invulnerableUntil) return; // окно неуязвимости

        Lives--;
        hud.SetLives(Lives);
        invulnerableUntil = Time.time + invulnerability;

        Debug.Log($"Удар! Осталось жизней: {Lives}");
        // TODO следующий коммит: вспышка, замедление, реплика паука

        if (Lives <= 0)
        {
            Debug.Log("Конец забега: поймана!");
            GameFlow.EndGame();
        }
    }
}