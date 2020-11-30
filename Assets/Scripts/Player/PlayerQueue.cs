using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQueue : MonoBehaviour
{
    bool playerTurn;
    List<Queue> enemyTimers;
    GlobalTimer gt;

    private void Awake()
    {
        gt = FindObjectOfType<GlobalTimer>();
    }
    private void Update()
    {
        enemyTimers = new List<Queue>(FindObjectsOfType<Queue>());
        if (!playerTurn)
        {
            playerTurn = CheckIfTurn();
        }
    }

    private bool CheckIfTurn()
    {
        foreach (Queue queue in enemyTimers)
        {
            if (!queue.Exhausted())
            {
                return false;
            }
        }
        return true;
    }

    public bool DoAction(int time)
    {
        if (!playerTurn)
        {
            return false;
        }
        gt.ElapseTime(time);
        playerTurn = false;

        return true;

    }
}
