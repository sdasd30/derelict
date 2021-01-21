using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator2 : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;

    public int widthMinRoom;
    public int widthMaxRoom;
    public int heightMinRoom;
    public int heightMaxRoom;
    public int maxFeatures;

    public int maxAttempts = 10000000;

    [HideInInspector]
    public int featureCount;

    public void InitializeMap()
    {
        MapManager.map = new Tile[mapWidth,mapHeight];
        MapManager.allFeatures = new List<Feature>();
    }

    public void ClearMap()
    {
        MapManager.allFeatures = new List<Feature>();
        featureCount = 0;
    }

    public void FillNullSpace()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (MapManager.map[x,y] == null)
                {
                    MapManager.map[x, y] = new Tile();
                    MapManager.map[x, y].position = new Vector2Int(x,y);
                    MapManager.map[x, y].type = "hull";
                    MapManager.map[x, y].sprite = "hull";
                    MapManager.map[x, y].occupied = true;
                    MapManager.map[x, y].isOpaque
                        = true;
                }
            }
        }
    }

    public void BuildMap()
    {
        InitializeMap();
        ClearMap();
        //Debug.Log("Started map generation...");
        for (int i = 0; i < maxAttempts; i++)
        {
            GenerateRoom("room", "Room");
            //Debug.Log(featureCount);
            if (featureCount >= maxFeatures) 
                break;
        }

        GetComponent<GeneratePaths>().GenerateMesh();
        GetComponent<GenerateCorridors>().BuildCorridors();


        FillNullSpace();
    }

    public void GenerateRoom(string type, string roomType)
    {
        Feature room = new Feature();
        if (type == "room" && roomType != null)
            room.roomType = roomType;
        room.type = type;

        room.width = Random.Range(widthMinRoom, widthMaxRoom);
        room.height = Random.Range(heightMinRoom, heightMaxRoom);
        room.positions = new List<Vector2Int>();

        Vector2Int startPoint = new Vector2Int(Random.Range(0, mapWidth), Random.Range(0, mapHeight));
        Vector2Int endPoint = new Vector2Int(startPoint.x + room.width,startPoint.y + room.height);

        if (!CheckIfSpace(startPoint, endPoint))
        {
            return;
        }

        room.walls = new Dictionary<Direction, Wall>();
        room.walls[Direction.SOUTH] = new Wall();
        room.walls[Direction.SOUTH].direction = Direction.SOUTH;
        room.walls[Direction.SOUTH].position = new List<Vector2Int>();

        room.walls[Direction.NORTH] = new Wall();
        room.walls[Direction.NORTH].direction = Direction.NORTH;
        room.walls[Direction.NORTH].position = new List<Vector2Int>();

        room.walls[Direction.EAST] = new Wall();
        room.walls[Direction.EAST].direction = Direction.EAST;
        room.walls[Direction.EAST].position = new List<Vector2Int>();

        room.walls[Direction.WEST] = new Wall();
        room.walls[Direction.WEST].direction = Direction.WEST;
        room.walls[Direction.WEST].position = new List<Vector2Int>();


        for (int y = 0; y < room.height; y++) //This loop generates the room actually, assuming there is enough space.
        {
            for (int x = 0; x < room.width; x++)
            {
                Vector2Int position = new Vector2Int(startPoint.x + x, startPoint.y + y);

                room.positions.Add(position);

                MapManager.map[position.x, position.y] = new Tile();
                MapManager.map[position.x, position.y].position = position;

                if (y == 0)
                { //This will be a south wall
                    room.walls[Direction.SOUTH].position.Add(position);
                    room.walls[Direction.SOUTH].length++;
                    MapManager.map[position.x, position.y].type = "wall";
                    MapManager.map[position.x, position.y].sprite = "wall";
                    MapManager.map[position.x, position.y].occupied = true;
                    MapManager.map[position.x, position.y].isOpaque = true;
                }

                if (y == room.height - 1)
                { //This will be a north wall
                    room.walls[Direction.NORTH].position.Add(position);
                    room.walls[Direction.NORTH].length++;
                    MapManager.map[position.x, position.y].type = "wall";
                    MapManager.map[position.x, position.y].sprite = "wall";
                    MapManager.map[position.x, position.y].occupied = true;
                    MapManager.map[position.x, position.y].isOpaque = true;
                }

                if (x == 0)
                { //This will be a west wall
                    room.walls[Direction.WEST].position.Add(position);
                    room.walls[Direction.WEST].length++;
                    MapManager.map[position.x, position.y].type = "wall";
                    MapManager.map[position.x, position.y].sprite = "wall";
                    MapManager.map[position.x, position.y].occupied = true;
                    MapManager.map[position.x, position.y].isOpaque = true;
                }

                if (x == room.width - 1)
                { //This will be an east wall
                    room.walls[Direction.EAST].position.Add(position);
                    room.walls[Direction.EAST].length++;
                    MapManager.map[position.x, position.y].type = "wall";
                    MapManager.map[position.x, position.y].sprite = "wall";
                    MapManager.map[position.x, position.y].occupied = true;
                    MapManager.map[position.x, position.y].isOpaque = true;
                }

                if (MapManager.map[position.x, position.y].type != "wall")
                {
                    MapManager.map[position.x, position.y].type = "floor";
                    MapManager.map[position.x, position.y].sprite = "floor";
                    MapManager.map[position.x, position.y].occupied = false;
                }

            }
        }

        room.ID = featureCount;
        room.centerPoint = new Vector2(
            (startPoint.x + (startPoint.x + room.width - 1)) / 2.0f,
            (startPoint.y + (startPoint.y + room.height - 1)) / 2.0f);
        featureCount++;
        MapManager.allFeatures.Add(room);
    }

    public bool CheckIfSpace(Vector2Int startPoint, Vector2Int endPoint)
    {
        for (int y = startPoint.y; y <= endPoint.y; y++)
        {
            for (int x = startPoint.x; x <= endPoint.x; x++)
            {
                if (x - 1 < 0 || y - 1 < 0 || x + 1 >= mapWidth || y + 1 >= mapHeight)
                {
                    return false;
                }
                if (MapManager.map[x, y] != null)
                {
                    return false;
                }

                //This will check if the rooms are directly touching.

                if (MapManager.map[x + 1, y] != null)
                {
                    return false;
                }

                if (MapManager.map[x - 1, y] != null)
                {
                    return false;
                }

                if (MapManager.map[x, y +1] != null)
                {
                    return false;
                }

                if (MapManager.map[x, y - 1] != null)
                {
                    return false;
                }

            }
        }
        return true;
    }
}
