using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;

    public int widthMinRoom;
    public int widthMaxRoom;
    public int heightMinRoom;
    public int heightMaxRoom;

    public int maxCorridorLength;
    public int maxFeatures;

    public void InitializeMap()
    {
        MapManager.map = new Tile[mapWidth, mapHeight];
    }

    public void FirstRoom()
    {
        Feature room = new Feature();
        room.positions = new List<Position>();

        int roomHeight = Random.Range(heightMinRoom, heightMaxRoom);
        int roomWidth = Random.Range(widthMinRoom, widthMaxRoom);

        int xStartingPoint = mapWidth / 2;
        int yStartingPoint = mapHeight / 2;

        xStartingPoint -= Random.Range(0, roomWidth);
        yStartingPoint -= Random.Range(0, roomHeight);

        room.walls = new Wall[4];

        for (int i = 0; i < room.walls.Length; i++)
        {
            room.walls[i] = new Wall();
            room.walls[i].position = new List<Position>();
            room.walls[i].length = 0;

            switch (i)
            {
                case 0:
                    room.walls[i].direction = "South";
                    break;
                case 1:
                    room.walls[i].direction = "North";
                    break;
                case 2:
                    room.walls[i].direction = "West";
                    break;
                case 3:
                    room.walls[i].direction = "East";
                    break;
            }
        }

        for (int y = 0; y < roomHeight; y++)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                Position position = new Position();
                position.x = xStartingPoint + x;
                position.y = yStartingPoint + y;

                room.positions.Add(position);

                MapManager.map[position.x, position.y] = new Tile();
                MapManager.map[position.x, position.y].xPosition = x;
                MapManager.map[position.x, position.y].yPosition = y;

                if (y == 0) // South Wall
                {
                    room.walls[0].position.Add(position);
                    room.walls[0].length++;
                    MapManager.map[position.x, position.y].type = "wall";
                    MapManager.map[position.x, position.y].sprite = "wall";
                    MapManager.map[position.x, position.y].occupied = true;
                }
                else if (y == (roomHeight-1)) // North Wall
                {
                    room.walls[1].position.Add(position);
                    room.walls[1].length++;
                    MapManager.map[position.x, position.y].type = "wall";
                    MapManager.map[position.x, position.y].sprite = "wall";
                    MapManager.map[position.x, position.y].occupied = true;
                }

                else if (x == 0) //West Wall
                {
                    room.walls[2].position.Add(position);
                    room.walls[2].length++;
                    MapManager.map[position.x, position.y].type = "wall";
                    MapManager.map[position.x, position.y].sprite = "wall";
                    MapManager.map[position.x, position.y].occupied = true;
                }
                else if (x == (roomWidth-1)) //East Wall
                {
                    room.walls[3].position.Add(position);
                    room.walls[3].length++;
                    MapManager.map[position.x, position.y].type = "wall";
                    MapManager.map[position.x, position.y].sprite = "wall";
                    MapManager.map[position.x, position.y].occupied = true;
                }

                else
                {
                    MapManager.map[position.x, position.y].type = "floor";
                    MapManager.map[position.x, position.y].sprite = "floor";
                    MapManager.map[position.x, position.y].occupied = false;
                }

            }
        }

        room.width = roomWidth;
        room.height = roomHeight;
        room.type = "Room";
    }
}
