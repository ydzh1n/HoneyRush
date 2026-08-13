using System.Collections.Generic;
using UnityEngine;

public class BeeBlink : MonoBehaviour
{
    [SerializeField] private float blinkInterval = 0.12f;

    private MeshRenderer[] renderers;
    private float timer;
    private bool blinkOn = true;

    private void Awake()
    {
        var list = new List<MeshRenderer>();
        foreach (var r in GetComponentsInChildren<MeshRenderer>(true))
            if (r.enabled && r.gameObject.activeInHierarchy) list.Add(r);
        renderers = list.ToArray();
    }

    private void Update()
    {
        if (GameFlow.GameOver)
        {
            if (blinkOn) { blinkOn = false; Apply(); } // пчела спрятана в коконе
            return;
        }

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
            renderers[i].enabled = blinkOn;
    }
}