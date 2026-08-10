using UnityEngine;

public class BeeBlink : MonoBehaviour
{
    [SerializeField] private float blinkInterval = 0.12f;

    private MeshRenderer[] renderers;
    private bool[] baseEnabled;
    private float timer;
    private bool blinkOn = true;

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>(true);
        baseEnabled = new bool[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            baseEnabled[i] = renderers[i].enabled;
    }

    private void Update()
    {
        bool inv = HealthSystem.Instance != null && HealthSystem.Instance.IsInvulnerable;

        if (inv)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= blinkInterval)
            {
                timer = 0f;
                blinkOn = !blinkOn;
                Apply();
            }
        }
        else if (!blinkOn)
        {
            blinkOn = true;
            Apply();
        }
    }

    private void Apply()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].enabled = baseEnabled[i] && blinkOn; // капсула остаётся невидимой
    }
}