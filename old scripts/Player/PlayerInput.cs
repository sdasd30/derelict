using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerQueue pq;
    // Start is called before the first frame update
    void Start()
    {
        pq = GetComponent<PlayerQueue>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("."))
        {
            Debug.Log("You wait a second");
            if (!pq.DoAction(100))
            {
                return;
            }

            //If this were a normal action, this is where the actual command for what happens next goes.
        }
    }
}
