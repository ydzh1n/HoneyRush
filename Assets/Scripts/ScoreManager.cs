using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI counterText;

    private int count;

    private void Awake() => Instance = this;

    public void AddDrop()
    {
        count++;
        counterText.text = count.ToString();
    }
}