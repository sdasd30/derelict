using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPosition : MonoBehaviour
{
    Transform mBody;
    Vector2Int position;

    private void Awake()
    {
        mBody = GetComponent<Transform>();
        position = new Vector2Int(0,0);
    }

    public void UpdatePosition(Vector2Int Modification)
    {
        position += Modification;
        Vector3 pos = new Vector3Int(position.x, position.y, 0);
        mBody.position = pos;
    }

    public void SetPosition(Vector2Int NewPos)
    {
        position = NewPos;
        Vector3 pos = new Vector3Int(NewPos.x, NewPos.y, 0);
        mBody.position = pos;
    }
    public Vector2Int GetPosition()
    {
        return position;
    }
}
