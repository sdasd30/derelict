using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoV : MonoBehaviour //Also known as FoV 2. This is raycasting and leaves ugly artefacts, but it works for now.
{
    public int maxDistance; //Should be even
    EntityPosition playerPos;
    List<Vector2Int> bounds;

    private void Start()
    {
        playerPos = GetComponent<EntityPosition>();
    }

    public void CheckFov()
    {
        ClearSight();
        bounds = CreateBoundingBox();

        List<Vector2Int> seeTile = new List<Vector2Int>();
        foreach (Vector2Int bound in bounds)
        {
            foreach (Vector2Int cell in GetCellsAlongLine(playerPos.GetPosition(), bound))
            {
                seeTile.Add(cell);
                if (MapManager.map[cell.x, cell.y].isOpaque)
                    break;
            }

        }

        List<Vector2Int> newSight = new List<Vector2Int>();

        foreach (Vector2Int cell in seeTile)
        {
            newSight.Add(cell);
            newSight.AddRange(SeeWalls(cell));
        }

        foreach (Vector2Int set in newSight)
        {
            CreateSight(set);
        }


    }
    public static List<Vector2Int> GetCellsAlongLine(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        Vector2Int delta = new Vector2Int(
            Mathf.Abs(start.x - end.x),
            Mathf.Abs(start.y - end.y));

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

            if (MapManager.map[start.x, start.y] == null)
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
    } //Implementaiton 1

    public static List<Vector2Int> Bresenham(Vector2Int start, Vector2Int end) //Implementation 2
    {
        List<Vector2Int> cells = new List<Vector2Int>();
        int x0 = start.x;
        int x1 = end.x;
        int y0 = start.y;
        int y1 = end.y;

        bool steep = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0);
        if (steep)
        {
            int t;
            t = x0; // swap x0 and y0
            x0 = y0;
            y0 = t;
            t = x1; // swap x1 and y1
            x1 = y1;
            y1 = t;
        }

        int dx = 0;
        if (x0 > x1)
            dx = x0 - x1;
        else
            dx = x1 - x0;


        int dy = Mathf.Abs(y1 - y0);

        int error = dx / 2;

        int ystep = (y0 < y1) ? 1 : -1;
        int y = y0;

        for (int x = x0; x < x1; x++)
        {
            if (steep)
                cells.Add(new Vector2Int(y, x));
            else
                cells.Add(new Vector2Int(x, y));

            error -= dy;
            if (error < 0)
            {
                y += ystep;
                error += dx;
            }
        }

        return cells;
    }

    public void ClearSight()
    {
        foreach (Tile tile in MapManager.map)
        {
            tile.isVisible = false;
        }
    }

    public void DEBUGEXPLORE()
    {
        foreach (Tile tile in MapManager.map)
        {
            tile.isExplored = true;
        }
    }

    List<Vector2Int> CreateBoundingBox()
    {
        List<Vector2Int> bounds = new List<Vector2Int>();
        Vector2Int start = playerPos.GetPosition();

        start -= new Vector2Int(maxDistance / 2,maxDistance/2);

        bounds.Add(start);

        for (int row = 0; row <= maxDistance; row++)
        {
            bounds.Add(new Vector2Int(start.x + row, start.y));
            bounds.Add(new Vector2Int(start.x + row, start.y + maxDistance));
        }

        for (int col = 0; col <= maxDistance; col++)
        {
            bounds.Add(new Vector2Int(start.x, start.y + col));
            bounds.Add(new Vector2Int(start.x + maxDistance, start.y + col));
        }

        HashSet<Vector2Int> nov = new HashSet<Vector2Int>(bounds);
        bounds.Clear();

        foreach (Vector2Int bound in nov)
        {
            bounds.Add(bound);
        }

        for (int i = 0; i < bounds.Count; i++)
        {
            Vector2Int position = bounds[i];
            if (bounds[i].x >= MapManager.map.GetLength(0)) position.x = MapManager.map.GetLength(0) - 1;
            if (bounds[i].x < 0) position.x = 0;

            if (bounds[i].y >= MapManager.map.GetLength(1)) position.y = MapManager.map.GetLength(1) - 1;
            if (bounds[i].y < 0) position.y = 0;

            bounds[i] = position;
        }


        return bounds;
    }

    void CreateSight(Vector2Int fovPos)
    {
        if (MapManager.map[fovPos.x, fovPos.y] != null)
        {
            MapManager.map[fovPos.x, fovPos.y].isExplored = true;
            MapManager.map[fovPos.x, fovPos.y].isVisible = true;
        }
    }

    List<Vector2Int> SeeWalls(Vector2Int cell)
    {
        List<Vector2Int> newSight = new List<Vector2Int>();
        //if (!MapManager.map[cell.x, cell.y].isOpaque)
        //{
        //    for (int row = -1; row <= 1; row++)
        //    {
        //        for (int col = -1; col <= 1; col++)
        //        {
        //            if (MapManager.map[cell.x + row, cell.y + col].isOpaque)
        //            {
        //                newSight.Add(new Vector2Int(cell.x + row, cell.y + col));
        //            }
        //        }
        //    }
        //}

        //if (!MapManager.map[cell.x, cell.y].isOpaque){
        //    if (MapManager.map[cell.x + 1, cell.y].isOpaque)
        //    {
        //        newSight.Add(new Vector2Int(cell.x + 1, cell.y));
        //    }
        //    if (MapManager.map[cell.x - 1, cell.y].isOpaque)
        //    {
        //        newSight.Add(new Vector2Int(cell.x - 1, cell.y));
        //    }
        //    if (MapManager.map[cell.x, cell.y + 1].isOpaque)
        //    {
        //        newSight.Add(new Vector2Int(cell.x, cell.y + 1));
        //    }
        //    if (MapManager.map[cell.x, cell.y - 1].isOpaque)
        //    {
        //        newSight.Add(new Vector2Int(cell.x, cell.y - 1));
        //    }
        //}
        return newSight;
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    foreach (Vector2Int bound in bounds)
    //    {
    //        Vector3 p0 = new Vector3(playerPos.GetPosition().x, playerPos.GetPosition().y, 0);
    //        Vector3 p1 = new Vector3(bound.x, bound.y, 0);
    //        Gizmos.DrawLine(p0, p1);
    //    }
    //}

}
