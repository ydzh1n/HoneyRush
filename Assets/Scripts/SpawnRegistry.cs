using System.Collections.Generic;
using UnityEngine;

public static class SpawnRegistry
{
    private static readonly List<Vector3> hazards = new List<Vector3>();
    private static readonly List<Vector3> pickups = new List<Vector3>();

    public static void RegisterHazard(Vector3 p) => hazards.Add(p);
    public static void UnregisterHazard(Vector3 p) => hazards.Remove(p);
    public static void RegisterPickup(Vector3 p) => pickups.Add(p);
    public static void UnregisterPickup(Vector3 p) => pickups.Remove(p);

    public static bool IsFree(Vector3 pos, float hazardRadius, float pickupRadius)
    {
        for (int i = 0; i < hazards.Count; i++)
            if (Vector3.Distance(hazards[i], pos) < hazardRadius) return false;
        for (int i = 0; i < pickups.Count; i++)
            if (Vector3.Distance(pickups[i], pos) < pickupRadius) return false;
        return true;
    }
}