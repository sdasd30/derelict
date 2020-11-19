using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    [SerializeField]
    int actionPoints;

    public bool DoAction(int cost)
    {// Returns true if no more time.
        if (cost == 0)
        {
            Debug.LogError("The " + gameObject.name + " explodes!! (Verify what its doing takes action points!)");
            Destroy(this);
        }

        if (actionPoints <= 0)
        {
            Debug.Log(gameObject + "tried to do an action but it was too tired");
            return true;
        }
        actionPoints -= cost;
        if (actionPoints <= 0)
        {
            return true;
        }
        return false;


    }

    public void GiveActionPoints(int give)
    {
        actionPoints += give;
    }

    public bool Exhausted()
    {
        if (actionPoints <= 0)
        {
            return true;
        }
        return false;
    }

}
