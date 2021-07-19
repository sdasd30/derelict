using System.Collections;
using System; //So the script can use serialization commands.
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour //This is about the tilemap. The surface where most action happens. 
                                        //grid where rooms are made, see CellMap.
{
    public static Tile[,] map; //This is the map with information of all tiles.
    public static Cell[,] cells; //This is the map with information of all tiles.
    public static List<Room> rooms;
}

public enum Direction { NORTH, SOUTH, EAST, WEST, NE, SE, SW, NW}

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

    public int roomID;
}

[Serializable]
public class Cell //Each cell will possibly contain a room.
{
    public int ID = -1; //Which room is this?
    public string type; //Is this a room or a hallway?
    public Vector2Int location; //Which cell does this cell occupy?
    public bool exists = false; //Does this cell currently contain a room?

    public List<Direction> doorwayConnections; //This is for doorway connections
    public List<Direction> sameRoomNeighbors; //This is for neighboring doors.
}

[Serializable]
public class Room
{ // A class for saving the feature (corridor or room) information, for the dungeon generation algorithm
    public List<Room> neighbors;
    public int size;
    public int ID;
}

