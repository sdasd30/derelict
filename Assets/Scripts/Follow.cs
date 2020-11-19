using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    Transform mbody;

    private void Start()
    {
        mbody = GetComponent<Transform>();
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        mbody.position = target.transform.position + offset;
    }
}
