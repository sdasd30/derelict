using System.Collections;
using System; //So the script can use serialization commands.
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour //This is about the tilemap. The surface where most action happens. 
                                        //grid where rooms are made, see CellMap.
{
    public static Tile[,] map; //This is the map with information of all tiles.
    public static Cell[,] cells; //This is the map with information of all tiles.
    public static List<Feature> features;
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

[Serializable]
public class Cell //Each cell will possibly contain a room.
{
    public int ID; //Which room is this?
    public string type; //Is this a room or a hallway?
    public Vector2Int location; //Which cell does this cell occupy?
    public bool exists = false; //Does this cell currently contain a room?
    public List<int> connectedRoomID;
}

[Serializable]
public class Feature
{ // A class for saving the feature (corridor or room) information, for the dungeon generation algorithm
    public List<Vector2Int> positions;
    public string type; //This is to determine if the room is a hallway or a proper room
    public string roomType; //This is to determine what kind of room it will be. Medical? Engineering? Food? Reactor?
    public int width;
    public int height;
    public int ID; //For map generation

    public Vector2 centerPoint; //This is for delaunay triangulation.
}

