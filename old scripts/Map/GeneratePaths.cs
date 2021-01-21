using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriangleNet.Geometry;

public class GeneratePaths : MonoBehaviour
{
    public float PathMultiplier = 1f; //How many paths should be removed after spanning path is found?
    public bool superLink;
    MapGenerator2 MapGen;
    List<Feature> allFeatures;
    List<Path> paths;
    List<Path> shortPath;

    TriangleNet.Mesh mesh;

    public void GenerateMesh()
    {
        MapGen = GetComponent<MapGenerator2>();
        allFeatures = MapManager.allFeatures;

        paths = new List<Path>();
        shortPath = new List<Path>();

        Polygon polygon = new Polygon();

        for (int i = 0; i < allFeatures.Count; i++)
        {
            polygon.Add(new Vertex(allFeatures[i].centerPoint.x, allFeatures[i].centerPoint.y));
        }

        mesh = (TriangleNet.Mesh)polygon.Triangulate();

        paths = ConvertToPath(mesh);

        //foreach (Path item in paths)
        //{
        //    Debug.Log(item);
        //}

        shortPath = GenerateShortPath(paths);
    }

    List<Path> GenerateShortPath(List<Path> paths)
    {
        if (!superLink)
        {
            List<Path> oldpath = paths;
            paths.Sort((p1, p2) => p1.length.CompareTo(p2.length));
            List<Path> tree = new List<Path>();
            List<int> verticesRepresented = new List<int>();

            int i = 0;
            while (verticesRepresented.Count != allFeatures.Count)
            {
                if (paths.Count == 0)
                {
                    Debug.Log("Too many points!");
                    break;
                }
                i = Random.Range(0, paths.Count - 1);
                tree.Add(paths[i]);
                if (!verticesRepresented.Contains(paths[i].sourceID))
                {
                    verticesRepresented.Add(paths[i].sourceID);
                }
                if (!verticesRepresented.Contains(paths[i].endID))
                {
                    verticesRepresented.Add(paths[i].endID);
                }
                verticesRepresented.Sort();

                paths.RemoveAt(i);
            }

            while (tree.Count > allFeatures.Count * PathMultiplier)
            {
                tree.RemoveAt(Random.Range(0, tree.Count - 1));
            }
            return tree;
        }
        else
        {
            return paths;
        }

    }

    int FindID(Vector2 point)
    {
        for (int i = 0; i < allFeatures.Count; i++)
        {
            if (point == allFeatures[i].centerPoint)
                return allFeatures[i].ID;
        }
        Debug.LogWarning("Point at " + point + " has no ID associated with it");
        return -1;
    }
    List<Path> ConvertToPath(TriangleNet.Mesh mesh)
    {
        List<Path> path = new List<Path>();
        int pathID = 0;
        foreach (Edge edge in mesh.Edges)
        {
            Vertex v0 = mesh.vertices[edge.P0];
            Vertex v1 = mesh.vertices[edge.P1];
            Vector2 start = new Vector2((float)v0.x, (float)v0.y);
            Vector2 end = new Vector2((float)v1.x, (float)v1.y);

            float length = Vector2.Distance(start, end);

            int startID = FindID(start);
            int endID = FindID(end);

            if (startID > endID)
            {
                int tmp = startID;
                startID = endID;
                endID = tmp;
            }

            path.Add(new Path(start, end, length, startID, endID, pathID));
            pathID++;
        }
        return path;
    }

    public List<Path> GetShortPath()
    {
        return shortPath;
    }

    public void OnDrawGizmos()
    {
        if (mesh == null)
        {
            return;
        }

        //Gizmos.color = Color.red;
        //foreach (Edge edge in mesh.Edges)
        //{
        //    Vertex v0 = mesh.vertices[edge.P0];
        //    Vertex v1 = mesh.vertices[edge.P1];
        //    Vector3 p0 = new Vector3((float)v0.x, (float)v0.y, 0.0f);
        //    Vector3 p1 = new Vector3((float)v1.x, (float)v1.y, 0.0f );
        //    Gizmos.DrawLine(p0, p1);
        //}

        Gizmos.color = Color.blue;
        foreach (Path path in shortPath)
        {
            Vector3 p0 = path.sourcePoint;
            Vector3 p1 = path.endPoint;
            Gizmos.DrawLine(p0, p1);
        }
    }
}
