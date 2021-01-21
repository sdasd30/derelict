using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    public int time;
    List<Queue> enemyTimers;
    public void ElapseTime(int increment)
    {
        time += increment;
        foreach (Queue item in enemyTimers)
        {
            item.GiveActionPoints(increment);
        }
    }

    private void Update()
    {
        enemyTimers = new List<Queue>(FindObjectsOfType<Queue>());
    }
}
