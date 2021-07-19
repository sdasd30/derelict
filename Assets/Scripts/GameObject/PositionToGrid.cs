using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TilePosition))]
[RequireComponent(typeof(Transform))]

public class PositionToGrid : MonoBehaviour
{
    TilePosition mpos;
    Transform mtrans;
    public float TileSize = 32;
    void Start()
    {
        mpos = GetComponent<TilePosition>();
        mtrans = GetComponent<Transform>();
        TileSize /= 100f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        mtrans.position = new Vector3(mpos.Position().x * TileSize, mpos.Position().y * TileSize, 0);
    }
}
