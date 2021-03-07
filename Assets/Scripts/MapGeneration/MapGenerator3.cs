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

    CellReadText crt;

    private void Start()
    {
        GenerateMap();
    }

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

        
        for (int y = 0; y < cellsHeight; y++)
        {
            for (int x = 0; x < cellsWidth; x++)
            {
                MapManager.cells[x,y] = new Cell();
            }
        }

        CreateRoomCells();
        crt = FindObjectOfType<CellReadText>();

        crt.RenderText(cellsWidth, cellsHeight);
    }

    void CreateRoomCells() //This is the script that generates the rooms themselves
    {

        Vector2Int location; //= new Vector2Int(Random.Range(0,cellsWidth),Random.Range(0,cellsHeight));
        int currentID = 0;
        bool failCond;

        for (int i = 0; i < maxRooms; i++)
        {
            failCond = false;
            for (int c = 0; c < maxAttempts; c++)
            {
                int roomSize = Random.Range(roomMinCells, roomMaxCells);
                location = new Vector2Int(Random.Range(0, cellsWidth), Random.Range(0, cellsHeight));
                if (!MapManager.cells[location.x, location.y].exists == true)
                {
                    GenerateRoom(0, location, currentID,roomSize);
                    break;
                }

                if (c == maxAttempts - 1)
                {
                    failCond = true;
                    break;
                }
            }
            if (failCond)
            {
                Debug.LogError("fail state");
                break;
            }
            currentID++;
        }

        Debug.Log("Done generating map. " + currentID +" rooms generated");
        /*
        foreach (Cell cel in MapManager.cells)
        {
            cel.sameRoomNeighbors = SameRoomDirections(cel, MapManager.cells);
            /*
            Debug.Log("Cell (room ID :" + cel.ID + ") at " + cel.location.x + "," +
                cel.location.y + "is connected to rooms " + cel.sameRoomNeighbors.Count);
            
        }*/
        
    }

    void GenerateRoom(int count, Vector2Int cellLocation, int ID, int maxRoom) 
        //This script makes a singular room, and assigns its values.
    {

        if (count == maxRoom)
        {
            //Debug.Log("Got myself");
            return;
        }

        MapManager.cells[cellLocation.x, cellLocation.y].ID = ID;
        MapManager.cells[cellLocation.x, cellLocation.y].exists = true;
        MapManager.cells[cellLocation.x, cellLocation.y].type = "room";
        MapManager.cells[cellLocation.x, cellLocation.y].location = cellLocation;

        Vector2Int newLoc = CheckForFreeNeighbors(cellLocation);
        if (newLoc == cellLocation)
        {
            return;
        }

        GenerateRoom(++count, newLoc, ID, maxRoom);

    }

    Vector2Int CheckForFreeNeighbors(Vector2Int cellLocation) 
        //This script looks around and checks for empty space.
    {
        Vector2Int newDirection = cellLocation;
        bool foundDirection = false;

        for (int i = 0; i < 100; i++)
        {
            int direction = Random.Range(0, 4);
            switch (direction)
            {
                case 0:
                    if (cellLocation.x + 1 < cellsWidth && //Check East
                    MapManager.cells[cellLocation.x + 1, cellLocation.y].exists == false)
                    {
                        newDirection = new Vector2Int(cellLocation.x + 1, cellLocation.y);
                        foundDirection = true;
                        break;
                    }
                    break;
                case 1:
                    if (cellLocation.x - 1 >= 0 && //Check West
                    MapManager.cells[cellLocation.x - 1, cellLocation.y].exists == false)
                    {
                        newDirection = new Vector2Int(cellLocation.x - 1, cellLocation.y);
                        foundDirection = true;
                        break;
                    }
                    break;
                case 2:
                    if (cellLocation.y - 1 >= 0 && //Check East
                    MapManager.cells[cellLocation.x, cellLocation.y - 1].exists == false)
                    {
                        newDirection = new Vector2Int(cellLocation.x, cellLocation.y - 1);
                        foundDirection = true;
                        break;
                    }
                    break;

                case 3:
                    if (cellLocation.y + 1 < cellsHeight && //Check East
                    MapManager.cells[cellLocation.x, cellLocation.y + 1].exists == false)
                    {
                        newDirection = new Vector2Int(cellLocation.x, cellLocation.y + 1);
                        foundDirection = true;
                        break;
                    }
                    break;

            }
            if (foundDirection)
            {
                break;
            }
        }
        return newDirection;
    }

    List<Direction> SameRoomDirections(Cell checkCell, Cell[,] celmap)
    //What rooms next to this room are the same room?
    {
        List<Direction> dirs = new List<Direction>();
        int indexX = checkCell.location.x;
        int indexY = checkCell.location.y;

        if (!(indexY + 1 >= cellsHeight) && 
            checkCell.ID == celmap[indexX, indexY + 1].ID) //Check North
        {
            dirs.Add(Direction.NORTH);
        }
        if (!(indexX + 1 >= cellsWidth) &&
            checkCell.ID == celmap[indexX + 1, indexY].ID) //Check East
        {
            dirs.Add(Direction.EAST);
        }
        if (!(indexY - 1 < 0) && 
            checkCell.ID == celmap[indexX, indexY - 1].ID) //Check South
        {
            dirs.Add(Direction.SOUTH);
        }
        if (!(indexX - 1 < 0) && 
            checkCell.ID == celmap[indexX - 1, indexY].ID) //Check West
        {
            dirs.Add(Direction.WEST);
        }

        if (!(indexY + 1 >= cellsHeight) && !(indexX + 1 >= cellsWidth) &&
            checkCell.ID == celmap[indexX + 1, indexY + 1].ID) //Check NE
        {
            dirs.Add(Direction.NE);
        }
        if (!(indexY - 1 < 0) && !(indexX + 1 >= cellsWidth) &&
            checkCell.ID == celmap[indexX + 1, indexY - 1].ID) //Check SE
        {
            dirs.Add(Direction.SE);
        }
        if (!(indexY - 1 < 0 ) && !(indexX - 1 < 0) &&
            checkCell.ID == celmap[indexX - 1, indexY - 1].ID) //Check SW
        {
            dirs.Add(Direction.SW);
        }
        if (!(indexY + 1 >= cellsHeight) && !(indexX - 1 < 0) &&
            checkCell.ID == celmap[indexX - 1, indexY + 1].ID) //Check NW
        {
            dirs.Add(Direction.NW);
        }

        return dirs;
    }
}

