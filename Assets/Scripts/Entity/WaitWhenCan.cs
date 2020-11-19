using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitWhenCan : MonoBehaviour
{
    // Start is called before the first frame update
    Queue q;
    void Awake()
    {
        q = GetComponent<Queue>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!q.Exhausted())
        {
            q.DoAction(100);
            Debug.Log("The " + gameObject.name + " waits a second.");
        }
    }
}
