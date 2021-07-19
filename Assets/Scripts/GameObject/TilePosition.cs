using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePosition : MonoBehaviour
{
    [SerializeField] Vector2Int pos;

    public void SetPosition(int x, int y)
    {
        pos = new Vector2Int(x, y);
    }

    public void AdjustPosition(int x, int y)
    {
        pos = new Vector2Int(pos.x + x, pos.y + y);
    }

    public Vector2Int Position()
    {
        return pos;
    }
}
