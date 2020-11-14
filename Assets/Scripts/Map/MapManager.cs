using System.Collections;
using System; //So the script can use serialization commands.
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    public static Tile[,] map; //This is the map with information of all tiles.
}

[Serializable] //Makes the class serializatble. This makes it saveable.
public class Tile 
{ //Holds all information for each tile.
    public int xPosition; //Position on the x axis of the tilemap.
    public int yPosition; //Position on the y axis of the tilemap.
    public bool occupied; //This is to see if objects can step onto it.
    [NonSerialized]
    public GameObject objectType; //The maptype object attached to the tile. Defines what kind of map object. Is it floor or wall or space?
    public string type; //Type of the object. Floor, wall, etc.
    public string sprite;
}

[Serializable]
public class Position
{ //The position of stuff.
    public int x;
    public int y;
}

[Serializable]
public class Wall
{ //Represents a wall
    public List<Position> position;
    public string direction;
    public int length;
    public bool hasFeature = false;
}

[Serializable]
public class Feature
{ // A class for saving the feature (corridor or room) information, for the dungeon generation algorithm
    public List<Position> positions;
    public Wall[] walls;
    public string type;
    public int width;
    public int height;
}
