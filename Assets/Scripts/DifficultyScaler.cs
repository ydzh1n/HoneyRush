using UnityEngine;

public class DifficultyScaler : MonoBehaviour
{
    public static DifficultyScaler Instance;

    [Header("–ост сложности")]
    [Tooltip(" акие секунды игры считаютс€ одним 'уровнем' сложности")]
    [SerializeField] private float timePerLevel = 15f;

    [Tooltip("Ќа сколько увеличиваетс€ сложность за каждый уровень (1.0 = +0%)")]
    [SerializeField] private float multiplierStep = 0.15f;

    [Tooltip("ћаксимальный предел сложности (чтобы игра не стала невозможной)")]
    [SerializeField] private float maxMultiplier = 2.0f;

    // “екущий множитель (1.0 в начале, максимум 2.0)
    public float Multiplier { get; private set; } = 1f;

    private float timer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // —читаем врем€ только во врем€ активного забега
        if (!GameFlow.Started || GameFlow.GameOver) return;

        timer += Time.deltaTime;

        if (timer >= timePerLevel)
        {
            timer = 0f;
            // ”величиваем множитель, но не выше максимума
            Multiplier = Mathf.Min(Multiplier + multiplierStep, maxMultiplier);
            Debug.Log($"—ложность возросла! ћножитель: {Multiplier:F1}x");
        }
    }

    // —брос при новом забеге
    public void ResetDifficulty()
    {
        Multiplier = 1f;
        timer = 0f;
    }
}