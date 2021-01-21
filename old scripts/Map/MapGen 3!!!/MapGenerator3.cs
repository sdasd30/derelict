using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator3 : MonoBehaviour
{
    public int cellsWidth;
    public int cellsHeight;
    //How large is the cells? (This determines how many rooms can potentially exist.

    public int roomWidth;
    public int roomHeight;
    //How large are the rooms themselves? This includes walls, so it must be larger than 3x3.

    public int maxRooms;
    public int maxAttempts; //How many times should it try to place rooms before it gives up?

    public int roomMaxCells;
    public int roomMinCells;
    //What is the largest and smallest amount of cells a room can occupy?

    public void InitializeMap()
    {
        MapManager.cells = new Cell[cellsWidth, cellsHeight];
        MapManager.features = new List<Feature>();
        MapManager.map = new Tile[cellsWidth * roomWidth, cellsHeight * roomWidth];

    }

    // Update is called once per frame
    public void GenerateMap()
    {
        InitializeMap();
        CreateRoomCells();
    }

    void CreateRoomCells()
    {
        int roomSize = Random.Range(1, roomMaxCells);
        Vector2Int location = new Vector2Int(Random.Range(0,cellsWidth),Random.Range(0,cellsHeight));
        MapManager.cells[location.x, location.y].ID = 0;
        MapManager.cells[location.x, location.y].exists = true;
        MapManager.cells[location.x, location.y].type = "room";
        for (int i = 0; i < roomSize; i++)
        {
            GenerateRoom(0, location);
        }
        
        for (int i = 0; i < maxAttempts; i++)
        {
            
        }
    }

    void GenerateRoom(int count, Vector2Int cellLocation)
    {

    }
}
