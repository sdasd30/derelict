using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGraph : MonoBehaviour
{
    //This script is to verify that all rooms have another room as a neighbor. It will
    //then make a graph of all rooms, where rooms are vertices and edges are shared walls.
    //It will then generate an MST using kruskall's algorithm, and scatter in some extra loops.
    //Finally, DoorGenerator will take the resultant graph, and generate some doors.

    Vertex[] Vertices;
    int[] universe;

    public void Run()
    {
        FillVertices();
        universe = new int[Vertices.Length];
        GenerateInitialGraph();
        Debug.Log(IsConnected());
    }

    void MakeConnection(int from, int to)
    {
        from = CellIDtoCount(from);
        to = CellIDtoCount(to);
        Vertices[from].adj.Add(to);
        Vertices[to].adj.Add(from);

        Union(from, to);
    }

    void FillVertices()
    {
        List<int> cellIDs = new List<int>();
        cellIDs.Add(-1);
        int count = 0;
        foreach (Cell item in MapManager.cells)
        {
            if (!cellIDs.Contains(item.ID))
            {
                cellIDs.Add(item.ID);
                //Vertices.Add(new Vertex(item.ID));
                count++;
            }

        }

        cellIDs.Remove(-1);

        Vertices = new Vertex[count];
        count = 0;
        foreach (int cellid in cellIDs)
        {
            Vertices[count] = new Vertex(cellid);
            count++;
        }
    }

    void GenerateInitialGraph()
    {
        int thisCell = -1;
        HashSet<int> neighbors = new HashSet<int>();
        foreach (Cell cell in MapManager.cells)
        {
            thisCell = cell.ID;

            if (thisCell != -1)
            {
                neighbors = new HashSet<int>();
                neighbors = NeighborID(cell);
                foreach (int item in neighbors)
                {
                    MakeConnection(thisCell, item);
                }
            }
        }
    }

    HashSet<int> NeighborID(Cell checkCell)
    //What rooms next to this room are the same room?
    {
        HashSet<int> cells = new HashSet<int>();
        Cell[,] celmap = MapManager.cells;
        int indexX = checkCell.location.x;
        int indexY = checkCell.location.y;

        if (!(indexY + 1 >= celmap.GetLength(0)) &&
            checkCell.ID != celmap[indexX, indexY + 1].ID) //Check South
        {
            cells.Add(celmap[indexX, indexY + 1].ID);
        }
        if (!(indexX + 1 >= celmap.GetLength(1)) &&
            checkCell.ID != celmap[indexX + 1, indexY].ID) //Check East
        {
            cells.Add(celmap[indexX + 1, indexY].ID);
        }
        if (!(indexY - 1 < 0) &&
            checkCell.ID != celmap[indexX, indexY - 1].ID) //Check North
        {
            cells.Add(celmap[indexX, indexY - 1].ID);
        }
        if (!(indexX - 1 < 0) &&
            checkCell.ID != celmap[indexX - 1, indexY].ID) //Check West
        {
            cells.Add(celmap[indexX - 1, indexY].ID);
        }

        cells.Remove(-1);

        return cells;
    }



    int CellIDtoCount(int cellID)
    {
        for (int i = 0; i < Vertices.Length; i++)
        {
            if (Vertices[i].id == cellID)
            {
                return i;
            }
        }

        Debug.LogError("Tried to find a count from an ID that does not exist");
        return -1;
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

class Vertex
{
    public int id;
    public List<int> adj;

    public Vertex(int i)
    {
        id = i;
        adj = new List<int>();
    }
}