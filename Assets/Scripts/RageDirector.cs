using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RageDirector : MonoBehaviour
{
    [SerializeField] private SpiderCompanion spider;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private Image rageVignette;
    [SerializeField] private int rageEvery = 15;
    [SerializeField] private float warningTime = 1.5f;
    [SerializeField] private float rageTime = 6f;

    private bool busy; // не накладываем циклы друг на друга

    private void OnEnable() => ScoreManager.OnDropCollected += OnDrop;
    private void OnDisable() => ScoreManager.OnDropCollected -= OnDrop;

    private void OnDrop(int count)
    {
        if (busy || GameFlow.GameOver) return;
        if (count % rageEvery == 0)
            StartCoroutine(RageCycle());
    }

    private IEnumerator RageCycle()
    {
        busy = true;

        // телеграф: предупреждение до ускорения
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(warningTime);
        warningText.gameObject.SetActive(false);

        // ярость
        spider.SetRage(true);
        SetVignette(0.12f);
        yield return new WaitForSeconds(rageTime);

        spider.SetRage(false);
        SetVignette(0f);

        busy = false;
    }

    private void SetVignette(float alpha)
    {
        Color c = rageVignette.color;
        c.a = alpha;
        rageVignette.color = c;
    }
}