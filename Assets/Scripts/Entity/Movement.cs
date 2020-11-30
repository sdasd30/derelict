using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    EntityPosition pos;
    PlayerFoV fov;
    PlayerFoV3 fov3;
    bool isPlayer;

    private void Awake()
    {
        pos = GetComponent<EntityPosition>();
        isPlayer = (GetComponent<PlayerFoV>() != null);
        if (isPlayer)
        {
            fov = GetComponent<PlayerFoV>();
            fov3 = GetComponent<PlayerFoV3>();
        }
    }
    public void AttemptMovement(Direction dir)
    {

        Vector2Int move = new Vector2Int(0, 0);
        switch (dir)
        {
            case Direction.NORTH:
                move = Vector2Int.up;
                break;
            case Direction.SOUTH:
                move = Vector2Int.down;
                break;
            case Direction.EAST:
                move = Vector2Int.right;
                break;
            case Direction.WEST:
                move = Vector2Int.left;
                break;
        }

        Vector2Int updatePosition = pos.GetPosition() + move;

        if (!MapManager.map[updatePosition.x, updatePosition.y].occupied)
        {
            pos.SetPosition(updatePosition);
        }

        if (isPlayer)
        {
            fov.CheckFov();
            fov3.CheckFov();
        }
    }
}
