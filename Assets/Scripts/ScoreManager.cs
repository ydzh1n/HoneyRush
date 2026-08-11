using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public static event System.Action<int> OnDropCollected;

    [SerializeField] private TextMeshProUGUI counterText;

    private int count;

    private void Awake() => Instance = this;

    public void AddDrop()
    {
        count++;
        counterText.text = count.ToString();
        OnDropCollected?.Invoke(count);
    }
}