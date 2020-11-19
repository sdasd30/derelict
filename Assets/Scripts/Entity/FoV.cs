using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FoV
{
    // Start is called before the first frame update

    static List<Vector2Int> borderTiles;
    static int fovRange = 10;

    static public void Initialize()
    {
        borderTiles = new List<Vector2Int>();
        Vector2Int vectorToAdd;

        for (int i = -fovRange; i <= fovRange; i++)
        {
            vectorToAdd = new Vector2Int(i, fovRange);
            if (!borderTiles.Contains(vectorToAdd))
                borderTiles.Add(vectorToAdd);

            vectorToAdd = new Vector2Int(i, -fovRange);
            if (!borderTiles.Contains(vectorToAdd))
                borderTiles.Add(vectorToAdd);

            vectorToAdd = new Vector2Int(fovRange, i);
            if (!borderTiles.Contains(vectorToAdd))
                borderTiles.Add(vectorToAdd);

            vectorToAdd = new Vector2Int(-fovRange, i);
            if (!borderTiles.Contains(vectorToAdd))
                borderTiles.Add(vectorToAdd);

        }

    }

    static public void GetPlayerFoV(Vector2Int position)
    {
        for (int y = 0; y < GameObject.Find("Game Manager").GetComponent<MapGenerator2>().mapHeight; y++)
        {
            for (int x = 0; x < GameObject.Find("Game Manager").GetComponent<MapGenerator2>().mapWidth; x++)
            {
                if (MapManager.map[x,y] != null)
                {
                    MapManager.map[x, y].isVisible = false;
                }
            }
        }

        foreach (Vector2Int borderTile in borderTiles)
        {
            foreach (Vector2Int cell in GetCellsAlongLine(position, position + borderTile))
            {
                MapManager.map[cell.x, cell.y].isVisible = true;
                MapManager.map[cell.x, cell.y].isExplored = true;

                if (MapManager.map[cell.x, cell.y].isOpaque)
                    break;
            }
        }
    }

    // Update is called once per frame
    public static List<Vector2Int> GetCellsAlongLine(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        int mapHeight = GameObject.Find("Game Manager").GetComponent<MapGenerator2>().mapHeight;
        int mapWidth = GameObject.Find("Game Manager").GetComponent<MapGenerator2>().mapWidth;

        //if (mapWidth == null || mapHeight == null)
        //{
        //    Debug.LogError("Game Manager not named correctly or not present! It should be called 'Game Manager'");
        //    return cells;
        //}

        Vector2Int maxPosition = new Vector2Int(mapWidth - 1, mapHeight - 1);

        start.Clamp(Vector2Int.zero, maxPosition);
        end.Clamp(Vector2Int.zero, maxPosition);

        Vector2Int delta = new Vector2Int(
            Math.Abs(start.x - end.x),
            Math.Abs(start.y - end.y));

        Vector2Int step = new Vector2Int(
            start.x < end.x ? 1 : -1,
            start.y < end.y ? 1 : -1);

        int err = delta.x - delta.y;

        for (int i = 0; i < 1000; i++)
        {
            if (i > 900)
            {
                Debug.LogError("I caught myself in an infinite loop!");
                break;
            }

            cells.Add(start);

            if (MapManager.map[start.x,start.y] == null)
            {
                break;
            }
            if (start == end)
            {
                break;
            }


            int e2 = err * 2;

            if (e2 > -delta.y)
            {
                err -= delta.y;
                start.x += step.x;
            }
            if (e2 < delta.x)
            {
                err += delta.x;
                start.y += step.y;
            }
        }



        return cells;
    }
}
