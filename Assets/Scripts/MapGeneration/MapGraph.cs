using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGraph : MonoBehaviour
{
    //This script is to verify that all rooms have another room as a neighbor. It will reject a generation and attempt another if not all rooms
    //are connected with each other

    //Its an implementation of unionfind.

    int[] universe;

    // Start is called before the first frame update
    public bool CellsCheck()
    {
        universe = new int[CountCellTypes()];
        for (int i = 0; i < universe.Length; i++)
        {
            universe[i] = i;
        }

        GenerateGraph();

        return IsConnected();
    }

    int Find(int cell)
    {
//        Debug.Log("Call " + cell);
//        Debug.Log("Check" + universe[cell]);
        if (universe[cell] == cell)
        {
            return cell;
        }
        else
        {
            universe[cell] = Find(universe[cell]);
        }
        return universe[cell];
    }

    void Union(int c1, int c2)
    {
        if (c1 == -1 || c2 == -1)
        {
            return;
        }
        universe[Find(c1)] = Find(c2);
    }

    int CountCellTypes()
    {
        List<int> cellIDs = new List<int>();
        cellIDs.Add(-1);
        int count = 0;
        foreach (Cell item in MapManager.cells)
        {
            if (!cellIDs.Contains(item.ID))
            {
                count++;
                cellIDs.Add(item.ID);
            }
               
        }

        return count;
    }

    void GenerateGraph()
    {
        int cellsHeight = GetComponent<MapGenerator3>().cellsHeight;
        int cellsWidth = GetComponent<MapGenerator3>().cellsWidth;
        Cell[,] celmap = MapManager.cells;

        foreach (Cell checkCell in MapManager.cells)
        {
                int indexX = checkCell.location.x;
                int indexY = checkCell.location.y;
                if (!(indexY + 1 >= cellsHeight) &&
                    checkCell.ID != celmap[indexX, indexY + 1].ID) //Check South
                {
                    Union(checkCell.ID, celmap[indexX, indexY + 1].ID);
                }
                if (!(indexX + 1 >= cellsWidth) &&
                    checkCell.ID != celmap[indexX + 1, indexY].ID) //Check East
                {
                    Union(checkCell.ID, celmap[indexX + 1, indexY].ID);
                }
                if (!(indexY - 1 < 0) &&
                    checkCell.ID != celmap[indexX, indexY - 1].ID) //Check North
                {
                    Union(checkCell.ID, celmap[indexX, indexY - 1].ID);
                }
                if (!(indexX - 1 < 0) &&
                    checkCell.ID != celmap[indexX - 1, indexY].ID) //Check West
                {
                    Union(checkCell.ID, celmap[indexX - 1, indexY].ID);
                }
        }
    }

    public bool IsConnected()
    {
        int master = Find(0);

        for (int i = 0; i < universe.Length; i++)
        {
            if (master != Find(i))
            {
                return false;
            }
        }

        return true;
    }
}
