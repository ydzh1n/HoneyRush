using UnityEngine;
using UnityEngine.UI;

public class LivesHUD : MonoBehaviour
{
    [SerializeField] private Image[] petals; // три, слева направо

    private Color[] baseColors;

    private void Awake()
    {
        baseColors = new Color[petals.Length];
        for (int i = 0; i < petals.Length; i++)
            baseColors[i] = petals[i].color;
    }

    public void SetLives(int lives)
    {
        for (int i = 0; i < petals.Length; i++)
        {
            Color c = baseColors[i];
            c.a = i < lives ? 1f : 0.15f; // потерянный лепесток гаснет, но не ломает раскладку
            petals[i].color = c;
        }
    }
}