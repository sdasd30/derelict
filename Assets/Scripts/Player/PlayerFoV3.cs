using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoV3 : MonoBehaviour //This is raycasting in a 360 degree arc.
{
    public int viewRadius;
    EntityPosition playerPos;

    private void Start()
    {
        playerPos = GetComponent<EntityPosition>();
    }
    public void CheckFov()
    {
        //ClearSight();
        float x, y;
        for (int i = 0; i < 360; i++)
        {
            x = Mathf.Cos((float)i * .01745f / 1f);
            y = Mathf.Sin((float)i * .01745f / 1f);
            DoFoV(x, y);
        }


    }

    void DoFoV(float x, float y)
    {
        Vector2 sight = playerPos.GetPosition() + new Vector2(.5f,.5f);

        for (int i = 0; i < viewRadius; i++)
        {
            if (!((int)sight.x < 0 ||
                (int)sight.x >= MapManager.map.GetLength(0) ||
                (int)sight.y < 0 ||
                (int)sight.y >= MapManager.map.GetLength(1)))
            {
                //MapManager.map[(int)sight.x, (int)sight.y].isVisible = true;
                MapManager.map[(int)sight.x, (int)sight.y].isExplored = true;
                //if (MapManager.map[(int)sight.x, (int)sight.y].isOpaque)
                    //break;

                sight += new Vector2(x, y);
            }
        }
    }


    public void ClearSight()
    {
        foreach (Tile tile in MapManager.map)
        {
            tile.isVisible = false;
        }
    }
}
