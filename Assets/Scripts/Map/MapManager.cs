using System.Collections;
using System; //So the script can use serialization commands.
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    public static Tile[,] map; //This is the map with information of all tiles.
    public static List<Feature> allFeatures;
}

[Serializable] //Makes the class serializatble. This makes it saveable.
public class Tile 
{ //Holds all information for each tile.
    public Vector2Int position; //Position on the overall map
    public bool occupied; //This is to see if objects can step onto it or be placed onto it.
    [NonSerialized]
    public GameObject objectType; //The maptype object attached to the tile. Defines what kind of map object. Is it floor or wall or space?
    public string type; //Type of the object. Floor, wall, etc.
    public string sprite;

    public bool isVisible = false;
    public bool isOpaque = false;
    public bool isExplored = false;
}



public enum Direction {NORTH, SOUTH, EAST, WEST }

[Serializable]
public class Wall
{ //Represents a wall created out of walls. A list of walls making up one wall.
    public List<Vector2Int> position;
    public Direction direction;
    public int length;
    public bool hasFeature = false;
}

[Serializable]
public class Feature
{ // A class for saving the feature (corridor or room) information, for the dungeon generation algorithm
    public List<Vector2Int> positions;
    public Dictionary<Direction, Wall> walls;
    public string type; //This is to determine if the room is a hallway or a proper room
    public string roomType; //This is to determine what kind of room it will be. Medical? Engineering? Food? Reactor?
    public int width;
    public int height;
    public int ID; //For map generation

    public Vector2 centerPoint; //This is for delaunay triangulation.
}

public class Path
{
    public Vector2 sourcePoint;
    public Vector2 endPoint;
    public float length;

    public int sourceID;
    public int endID;
    public int pathID;

    public Path(Vector2 sp, Vector2 ep, float len, int sID, int eID, int pID)
    {
        sourcePoint = sp;
        endPoint = ep;
        length = len;

        sourceID = sID;
        endID = eID;
        pathID = pID;
    }

    public override string ToString()
    {
        return "Path:" + pathID + " SourceID: " + sourceID + " End ID: " + endID + " Length: " + length;
    }



}

