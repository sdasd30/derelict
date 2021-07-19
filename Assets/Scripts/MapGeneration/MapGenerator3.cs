using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator3 : MonoBehaviour
{
    public int cellsWidth;
    public int cellsHeight;
    //How many cells? (This determines how many rooms can potentially exist.

    public int roomWidth;
    public int roomHeight;
    //How large are the rooms themselves? This includes walls, so it must be larger than 3x3.

    public int maxRooms;
    public int maxAttempts; //How many times should it try to place rooms before it gives up?

    public int roomMaxCells;
    public int roomMinCells;
    //What is the largest and smallest amount of cells a room can occupy?

    RoomReadText rrt;
    DoorGenerator doorGenerator;

    private void Start()
    {
        doorGenerator = GetComponent<DoorGenerator>();
        GenerateMap();
        
    }

    public void InitializeMap() //Also usable to clear the map.
    {
        MapManager.cells = new Cell[cellsWidth, cellsHeight];
        MapManager.map = new Tile[cellsWidth * roomWidth, cellsHeight * roomWidth];

    }

    public void GenerateMap()
    {
        GeneratedConnected();
        GenerateDoors();

        GetComponent<RoomReadToTile>().DrawMap(); //DEBUG
    }

    void GeneratedConnected()
    {
        int i = 0;
        while(true)
        {
            i++;
            InitializeMap();
            for (int y = 0; y < cellsHeight; y++)
            {
                for (int x = 0; x < cellsWidth; x++)
                {
                    MapManager.cells[x, y] = new Cell();
                }
            }
            Debug.Log("Generating map... (Attempt " + i + ")");


            CreateRoomCells();
            CellstoTiles();
            doorGenerator.Run();

            if (doorGenerator.GetComponent<DoorGraph>().IsConnected())
            {
                Debug.Log("Map successfully generated!");
                return;
            }
            if (i == 100)
            {
                Debug.LogError("Could not generate map after 100 attempts. Rooms are either too small, " +
                    "there are not enough, or the map is too large. Try making the rooms more connection friendly.");
                //InitializeMap();
                return;
            }
        }
    }

    void GenerateDoors()
    {
        return;
    } //TODO


    void CellstoTiles() //This script takes the cells, and converts them into rooms.
    {
        for (int y = 0; y < cellsHeight; y++)
        {
            for (int x = 0; x < cellsWidth; x++)
            {
                if (MapManager.cells[x, y].exists)
                {
                    MapTiles(MapManager.cells[x, y]);
                }
            }
        }
    }

    void MapTiles(Cell cell) //This script turns a cell's location into tiles. Both floors and walls are accounted for.
    {
        Vector2Int roomTopLeft = new Vector2Int(cell.location.x * roomWidth, cell.location.y * roomHeight);

        for (int ry = 0; ry < roomHeight; ry++)
        {
            for (int rx = 0; rx < roomWidth; rx++)
            {
                MapManager.map[roomTopLeft.x + rx, roomTopLeft.y + ry] = new Tile();
                MapManager.map[roomTopLeft.x + rx, roomTopLeft.y + ry].position = new Vector2Int(roomTopLeft.x + rx, roomTopLeft.y + ry);
                MapManager.map[roomTopLeft.x + rx, roomTopLeft.y + ry].type = "floor";
                MapManager.map[roomTopLeft.x + rx, roomTopLeft.y + ry].sprite = "floor";
                MapManager.map[roomTopLeft.x + rx, roomTopLeft.y + ry].roomID = cell.ID;
            }

        }

        if (!cell.sameRoomNeighbors.Contains(Direction.NORTH))
        {
            for (int rx = 0; rx < roomWidth; rx++)
            {
                MapManager.map[roomTopLeft.x + rx, roomTopLeft.y].type = "Nwall";
                MapManager.map[roomTopLeft.x + rx, roomTopLeft.y].sprite = "wall";
            }
        }
        if (!cell.sameRoomNeighbors.Contains(Direction.EAST))
        {

            for (int ry = 0; ry < roomHeight; ry ++)
            {
                MapManager.map[roomTopLeft.x + roomWidth - 1, roomTopLeft.y + ry].type = "Ewall";
                MapManager.map[roomTopLeft.x + roomWidth - 1, roomTopLeft.y + ry].sprite = "wall";
            }
        }
        if (!cell.sameRoomNeighbors.Contains(Direction.SOUTH))
        {
            for (int rx = 0; rx < roomWidth; rx++)
            {
                MapManager.map[roomTopLeft.x + rx, roomTopLeft.y + roomHeight - 1].type = "Swall";
                MapManager.map[roomTopLeft.x + rx, roomTopLeft.y + roomHeight - 1].sprite = "wall";
            }
        }
        if (!cell.sameRoomNeighbors.Contains(Direction.WEST))
        {
            for (int ry = 0; ry < roomHeight; ry++)
            {
                MapManager.map[roomTopLeft.x, roomTopLeft.y + ry].type = "Wwall";
                MapManager.map[roomTopLeft.x, roomTopLeft.y + ry].sprite = "wall";
            }
        }

        //Check Top Left Corner WORKS
        if (cell.sameRoomNeighbors.Contains(Direction.WEST) &&
            cell.sameRoomNeighbors.Contains(Direction.NORTH) &&
            !cell.sameRoomNeighbors.Contains(Direction.NW))
        {
            MapManager.map[roomTopLeft.x, roomTopLeft.y].type = "corner";
            MapManager.map[roomTopLeft.x, roomTopLeft.y].sprite = "wall";
        }

        //Check Top right corner WORKS
        if (cell.sameRoomNeighbors.Contains(Direction.EAST) &&
           cell.sameRoomNeighbors.Contains(Direction.NORTH) &&
            !cell.sameRoomNeighbors.Contains(Direction.NE))
        {
            MapManager.map[roomTopLeft.x + roomHeight - 1, roomTopLeft.y].type = "corner";
            MapManager.map[roomTopLeft.x + roomHeight - 1, roomTopLeft.y].sprite = "wall";
           
        }

        //Check Bottom Left WORKS
        if (cell.sameRoomNeighbors.Contains(Direction.SOUTH) &&
            cell.sameRoomNeighbors.Contains(Direction.WEST) &&
           !cell.sameRoomNeighbors.Contains(Direction.SW))
        {
            MapManager.map[roomTopLeft.x, roomTopLeft.y + roomHeight - 1].type = "corner";
            MapManager.map[roomTopLeft.x, roomTopLeft.y + roomHeight - 1].sprite = "wall";
        }

        //Checkt Bottom Right WORKS
        if (cell.sameRoomNeighbors.Contains(Direction.SOUTH) &&
           cell.sameRoomNeighbors.Contains(Direction.EAST) &&
           !cell.sameRoomNeighbors.Contains(Direction.SE))
        {
            MapManager.map[roomTopLeft.x + roomWidth - 1, roomTopLeft.y + roomHeight - 1].type = "corner";
            MapManager.map[roomTopLeft.x + roomWidth - 1, roomTopLeft.y + roomHeight - 1].sprite = "wall";
        }
    }

    void ValidDoorTile(Tile tile)
    {
        //TODO
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

        //Debug.Log("Done generating map. " + currentID +" rooms generated");
        
        foreach (Cell cel in MapManager.cells)
        {
            cel.sameRoomNeighbors = SameRoomDirections(cel, MapManager.cells);
            /*
            Debug.Log("Cell (room ID :" + cel.ID + ") at " + cel.location.x + "," +
                cel.location.y + "is connected to rooms " + cel.sameRoomNeighbors.Count);
            */
        }
        
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
            checkCell.ID == celmap[indexX, indexY + 1].ID) //Check South
        {
            dirs.Add(Direction.SOUTH);
        }
        if (!(indexX + 1 >= cellsWidth) &&
            checkCell.ID == celmap[indexX + 1, indexY].ID) //Check East
        {
            dirs.Add(Direction.EAST);
        }
        if (!(indexY - 1 < 0) && 
            checkCell.ID == celmap[indexX, indexY - 1].ID) //Check North
        {
            dirs.Add(Direction.NORTH);
        }
        if (!(indexX - 1 < 0) && 
            checkCell.ID == celmap[indexX - 1, indexY].ID) //Check West
        {
            dirs.Add(Direction.WEST);
        }

        if (!(indexY + 1 >= cellsHeight) && !(indexX + 1 >= cellsWidth) &&
            checkCell.ID == celmap[indexX + 1, indexY + 1].ID) //Check SE
        {
            dirs.Add(Direction.SE);
        }
        if (!(indexY - 1 < 0) && !(indexX + 1 >= cellsWidth) &&
            checkCell.ID == celmap[indexX + 1, indexY - 1].ID) //Check NE
        {
            dirs.Add(Direction.NE);
        }
        if (!(indexY - 1 < 0) && !(indexX - 1 < 0) &&
            checkCell.ID == celmap[indexX - 1, indexY - 1].ID) //Check NW
        {
            dirs.Add(Direction.NW);
        }
        if (!(indexY + 1 >= cellsHeight) && !(indexX - 1 < 0) &&
            checkCell.ID == celmap[indexX - 1, indexY + 1].ID) //Check SW
        {
            dirs.Add(Direction.SW);
        }

        return dirs;
    }
}

