using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    Transform targetpos;
    Transform mpos;
    // Start is called before the first frame update
    void Start()
    {
        targetpos = target.GetComponent<Transform>();
        mpos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        mpos.position = targetpos.position + offset;
    }
}
