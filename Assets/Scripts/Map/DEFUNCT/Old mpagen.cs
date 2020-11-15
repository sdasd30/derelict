//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MapGenerator// : MonoBehaviour
//{
//    public int mapWidth;
//    public int mapHeight;

//    public int widthMinRoom;
//    public int widthMaxRoom;
//    public int heightMinRoom;
//    public int heightMaxRoom;

//    public int minCorridorLength;
//    public int maxCorridorLength;
//    public int maxFeatures;

//    public float roomChance;
    
//    [HideInInspector]
//    public int countFeatures;

//    public List<Feature> allFeatures;

//    public void InitializeMap()
//    {
//        MapManager.map = new Tile[mapWidth, mapHeight];
//    }

//    public void BuildMap()
//    {
//        allFeatures = new List<Feature>();

//        GenerateFeature("room", new Wall(), true);
//        Debug.Log("Generated a room");
//        Feature originFeature;

//        for (int i = 0; i < 10000; i++ )
//        {
//            if (countFeatures >= maxFeatures) break;
//            Debug.Log(allFeatures.Count);

//            if (allFeatures.Count == 1)
//            {
//                originFeature = allFeatures[0];
//            }
//            else
//            {
//                originFeature = allFeatures[Random.Range(0, allFeatures.Count - 1)];
//            }

//            Wall wall = ChooseWall(originFeature);
//            if (wall == null) continue;


//            if (Random.Range(0, 100) < roomChance)
//            {
//                GenerateFeature("room", wall, false);
//            }
//            else
//            {
//                GenerateFeature("corridor", wall, false);
//            }
//        }
//    }

//    public void GenerateFeature(string type, Wall wall, bool isFirst = false)
//    {
//        Feature room = new Feature();
//        room.positions = new List<Vector2Int>();
//        int roomHeight = 10;
//        int roomWidth = 10;

//        if (type == "room")
//        {
//            roomHeight = Random.Range(heightMinRoom, heightMaxRoom);
//            roomWidth = Random.Range(widthMinRoom, widthMaxRoom);
//        }

//        else if (type == "corridor")
//        {
//            switch (wall.direction)
//            {
//                case "south":
//                    roomWidth = 5;
//                    roomHeight = Random.Range(minCorridorLength, maxCorridorLength);
//                    break;
//                case "north":
//                    roomWidth = 5;
//                    roomHeight = Random.Range(minCorridorLength, maxCorridorLength);
//                    break;
//                case "east":
//                    roomHeight = 5;
//                    roomWidth = Random.Range(minCorridorLength, maxCorridorLength);
//                    break;
//                case "west":
//                    roomHeight = 5;
//                    roomWidth = Random.Range(minCorridorLength, maxCorridorLength);
//                    break;
//                default:
//                    Debug.LogError("Attempted to generate a corridor without direction!");
//                    roomHeight = 5;
//                    roomWidth = 5;
//                    break;
//            }
//        }

//        int xStartingPoint;
//        int yStartingPoint;

//        if (isFirst)
//        {
//            xStartingPoint = mapWidth / 2;
//            yStartingPoint = mapHeight / 2;
//        }

//        else //Generate Off of currently existing room
//        {
//            int id = Random.Range(2, wall.position.Count - 2);

//            xStartingPoint = wall.position[id].x;
//            yStartingPoint = wall.position[id].y;

//        }

//        Vector2Int lastWallPosition = new Vector2Int(xStartingPoint, yStartingPoint);

//        if (isFirst)
//        {
//            xStartingPoint -= Random.Range(1, roomWidth);
//            yStartingPoint -= Random.Range(1, roomHeight);
//        }

//        else
//        {
//            switch (wall.direction)
//            {
//                case "south":
//                    if (type == "room") xStartingPoint -= Random.Range(0, roomWidth - 2);
//                    else xStartingPoint--;
//                    yStartingPoint -= Random.Range(1, roomHeight - 2);
//                    break;
//                case "north":
//                    if (type == "room") xStartingPoint -= Random.Range(0, roomWidth - 2);
//                    else xStartingPoint--;
//                    yStartingPoint++;
//                    break;
//                case "west":
//                    xStartingPoint -= roomWidth;
//                    if (type == "room") yStartingPoint -= Random.Range(0, roomHeight - 2);
//                    else yStartingPoint++;
//                    break;
//                case "east":
//                    xStartingPoint++;
//                    if (type == "room") yStartingPoint -= Random.Range(0, roomHeight - 2);
//                    else yStartingPoint--;
//                    break;
//                default:
//                    Debug.LogError("Attempted to generate a corridor without direction!");
//                    break;
//            }
//        }

//        if (!CheckIfHasSpace(new Vector2Int(xStartingPoint, yStartingPoint), 
//            new Vector2Int (xStartingPoint + roomWidth - 1, yStartingPoint + roomHeight - 1)))
//        {
//            return;
//        }

//        room.walls = new Wall[4];

//        for (int i = 0; i < room.walls.Length; i++)
//        {
//            room.walls[i] = new Wall();
//            room.walls[i].position = new List<Vector2Int>();
//            room.walls[i].length = 0;

//            switch (i)
//            {
//                case 0:
//                    room.walls[i].direction = "south";
//                    break;
//                case 1:
//                    room.walls[i].direction = "north";
//                    break;
//                case 2:
//                    room.walls[i].direction = "west";
//                    break;
//                case 3:
//                    room.walls[i].direction = "east";
//                    break;
//            }
//        }

//        for (int y = 0; y < roomHeight; y++)
//        {
//            for (int x = 0; x < roomWidth; x++)
//            {
//                Vector2Int position = new Vector2Int();
//                position.x = xStartingPoint + x;
//                position.y = yStartingPoint + y;

//                room.positions.Add(position);

//                MapManager.map[position.x, position.y] = new Tile();
//                MapManager.map[position.x, position.y].xPosition = x;
//                MapManager.map[position.x, position.y].yPosition = y;

//                if (y == 0) // South Wall
//                {
//                    room.walls[0].position.Add(position);
//                    room.walls[0].length++;
//                    MapManager.map[position.x, position.y].type = "wall";
//                    MapManager.map[position.x, position.y].sprite = "wall";
//                    MapManager.map[position.x, position.y].occupied = true;
//                }
//                if (y == (roomHeight-1)) // North Wall
//                {
//                    room.walls[1].position.Add(position);
//                    room.walls[1].length++;
//                    MapManager.map[position.x, position.y].type = "wall";
//                    MapManager.map[position.x, position.y].sprite = "wall";
//                    MapManager.map[position.x, position.y].occupied = true;
//                }

//                if (x == 0) //West Wall
//                {
//                    room.walls[2].position.Add(position);
//                    room.walls[2].length++;
//                    MapManager.map[position.x, position.y].type = "wall";
//                    MapManager.map[position.x, position.y].sprite = "wall";
//                    MapManager.map[position.x, position.y].occupied = true;
//                }
//                if (x == (roomWidth-1)) //East Wall
//                {
//                    room.walls[3].position.Add(position);
//                    room.walls[3].length++;
//                    MapManager.map[position.x, position.y].type = "wall";
//                    MapManager.map[position.x, position.y].sprite = "wall";
//                    MapManager.map[position.x, position.y].occupied = true;
//                }

//                if (MapManager.map[position.x,position.y].type != "wall")
//                {
//                    MapManager.map[position.x, position.y].type = "floor";
//                    MapManager.map[position.x, position.y].sprite = "floor";
//                    MapManager.map[position.x, position.y].occupied = false;
//                }

//            }
//        } // This function generates the walls and floors for the rooms.

//        if (!isFirst)
//        {
//            switch (wall.direction)
//            {
//                case "south":
//                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y].type = "floor";
//                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y].sprite = "door";

//                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y - 1].type = "floor";
//                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y - 1].sprite = "door";
//                    break;
//                case "north":
//                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y].type = "floor";
//                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y].sprite = "door";

//                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y + 1].type = "floor";
//                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y + 1].sprite = "door";
//                    break;
//                case "west":
//                    MapManager.map[lastWallPosition.x, lastWallPosition.y + 1].type = "floor";
//                    MapManager.map[lastWallPosition.x, lastWallPosition.y + 1].sprite = "door";

//                    MapManager.map[lastWallPosition.x - 1, lastWallPosition.y + 1].type = "floor";
//                    MapManager.map[lastWallPosition.x - 1, lastWallPosition.y + 1].sprite = "door";
//                    break;
//                case "east":
//                    MapManager.map[lastWallPosition.x, lastWallPosition.y + 1].type = "floor";
//                    MapManager.map[lastWallPosition.x, lastWallPosition.y + 1].sprite = "door";

//                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y + 1].type = "floor";
//                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y + 1].sprite = "door";
//                    break;
//                default:
//                    Debug.LogError("Please give directions!");
//                    break;
//            }
//        }

//        room.width = roomWidth;
//        room.height = roomHeight;
//        room.type = type;
//        allFeatures.Add(room);
//        Debug.Log("Added to feature list");
//        countFeatures++;
//    }

//    bool CheckIfHasSpace(Vector2Int start, Vector2Int end)
//    {
//        for (int y = start.y; y <= end.y; y++)
//        {
//            for (int x = start.x; x <= end.x; x++)
//            {
//                if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight)
//                {
//                    Debug.Log("Out of bounds!");
//                    return false;
//                }
//                if (MapManager.map[x, y] != null)
//                {
//                    Debug.Log("Bonked!");
//                    return false;
//                }


//            }
//        }
//        return true;
//    }

//    public Wall ChooseWall(Feature feature)
//    {
//        for (int i = 0; i <10; i++) //10 attempts to find a wall with no features attached.
//        {
//            int id = Random.Range(0, 100) / 25;
//            if (!feature.walls[id].hasFeature)
//            {
//                return feature.walls[id];
//            }
//        }
//        return null;
//    }
//}
