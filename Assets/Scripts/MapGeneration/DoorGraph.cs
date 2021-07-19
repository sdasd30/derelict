using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGraph : MonoBehaviour
{
    //This script is to verify that all rooms have another room as a neighbor. It will reject a generation and attempt another if not all rooms
    //are connected with each other

    Vertex[] Vertices;
    int[] universe;

    public void Run()
    {
        FillVertices();
        universe = new int[Vertices.Length];
    }

    void MakeConnection(int from, int to)
    {
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

}

class Vertex
{
    int id;
    public List<int> adj;

    public Vertex(int i)
    {
        id = i;
        adj = new List<int>();
    }
}