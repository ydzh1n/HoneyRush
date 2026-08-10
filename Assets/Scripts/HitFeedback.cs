using UnityEngine;
using UnityEngine.UI;

public class HitFeedback : MonoBehaviour
{
    public static HitFeedback Instance;

    [SerializeField] private Image flashImage;
    [SerializeField] private float flashPower = 0.5f;
    [SerializeField] private float flashFade = 2f;
    [SerializeField] private float slowMoTime = 0.4f;
    [SerializeField] private float slowMoScale = 0.35f;

    private float slowMoLeft;

    private void Awake() => Instance = this;

    public void OnHit()
    {
        Color c = flashImage.color;
        c.a = flashPower;
        flashImage.color = c;

        slowMoLeft = slowMoTime;
        Time.timeScale = slowMoScale;
    }

    private void Update()
    {
        // вспышка гаснет в реальном времени, даже пока мир замедлен
        Color c = flashImage.color;
        if (c.a > 0f)
        {
            c.a = Mathf.Max(0f, c.a - flashFade * Time.unscaledDeltaTime);
            flashImage.color = c;
        }

        if (slowMoLeft > 0f)
        {
            slowMoLeft -= Time.unscaledDeltaTime;
            if (slowMoLeft <= 0f && !GameFlow.GameOver) // не будим замороженный конец забега
                Time.timeScale = 1f;
        }
    }
}