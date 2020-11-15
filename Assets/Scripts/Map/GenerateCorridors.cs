using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCorridors : MonoBehaviour
{
    List<Path> paths;
    MapGenerator2 mapGen;
    List<Feature> allFeatures;

    // Update is called once per frame
    public void BuildCorridors()
    {
        paths = GetComponent<GeneratePaths>().GetShortPath();
        mapGen = GetComponent<MapGenerator2>();
        allFeatures = MapManager.allFeatures;

        int sourceID;
        int endID;
        Feature startRoom;
        Feature endRoom;
        bool buildL;

        foreach (Path path in paths)
        {
            sourceID = path.sourceID;
            endID = path.endID;
            startRoom = allFeatures[sourceID];
            endRoom = allFeatures[endID];

            Direction buildDirection = GetInitialDirection(startRoom, endRoom);

            buildL = DetermineL(startRoom, endRoom, buildDirection);

            Vector2Int wallTile = ChooseWallTile(startRoom, buildDirection);

            if (buildL)
            {
                BuildCorridorL(startRoom, endRoom, buildDirection);
            }
            else
            {
                BuildCorridorNoL(startRoom, endRoom,buildDirection);
            }
        }
    }

    void BuildCorridorL(Feature start, Feature end, Direction bd)
    {
        Vector2Int destination = new Vector2Int(0, 0);
        Vector2Int building = new Vector2Int(0, 0);
        Vector2Int startTile = new Vector2Int(0, 0);
        for (int i = 0; i < 100; i++)
        {
            startTile = ChooseWallTile(start, bd);
            switch (bd)
            {
                case Direction.NORTH:
                    destination = new Vector2Int(startTile.x, Random.Range(end.positions[0].y + 2, end.positions[0].y + end.height - 3));
                    building = new Vector2Int(startTile.x, startTile.y + 1);
                    if (MapManager.map[destination.x, destination.y] != null && MapManager.map[destination.x, destination.y].type == "wall")
                        destination += Vector2Int.up;
                    break;
                case Direction.SOUTH:
                    destination = new Vector2Int(startTile.x, Random.Range(end.positions[0].y + 3, end.positions[0].y + end.height - 2));
                    building = new Vector2Int(startTile.x, startTile.y - 1);
                    if (MapManager.map[destination.x, destination.y] != null && MapManager.map[destination.x, destination.y].type == "wall")
                        destination += Vector2Int.down;
                    break;
                case Direction.EAST:
                    destination = new Vector2Int(Random.Range(end.positions[0].x + 2, end.positions[0].x + end.width - 3), startTile.y);
                    building = new Vector2Int(startTile.x + 1, startTile.y);
                    if (MapManager.map[destination.x, destination.y] != null && MapManager.map[destination.x, destination.y].type == "wall")
                        destination += Vector2Int.right;
                    break;
                case Direction.WEST:
                    destination = new Vector2Int(Random.Range(end.positions[0].x + 3, end.positions[0].x + end.width - 2), startTile.y);
                    building = new Vector2Int(startTile.x - 1, startTile.y);
                    if (MapManager.map[destination.x, destination.y] != null && MapManager.map[destination.x, destination.y].type == "wall")
                        destination += Vector2Int.left;
                    break;
            }

            if (CheckRouteClear(startTile, destination, bd))
            {
                break;
            }
            startTile = ChooseWallTile(start, bd);
            if (i >= 99)
            {
                //Cooridor unsucessful. Returning...
                //Debug.LogError("Something is wrong in L! What we know:\n" +
                //    "its building to: " + destination.x +"," + destination.y +"\n From: "+
                //    startTile.x + "," + startTile.y);
                return;
            }
        }

        MapManager.map[startTile.x, startTile.y].type = "door";
        MapManager.map[startTile.x, startTile.y].sprite = "door";
        MapManager.map[startTile.x, startTile.y].occupied = false;
        Build(building, destination, bd);

        startTile = destination;

        bd = GetNextDirection(destination, end);
        building = new Vector2Int(startTile.x, startTile.y);
        if (bd == Direction.NORTH)
        {
            destination = new Vector2Int(startTile.x, end.positions[0].y);
        }
        if (bd == Direction.SOUTH)
        {
            destination = new Vector2Int(startTile.x, end.positions[0].y + end.height - 1);
        }
        if (bd == Direction.EAST)
        {
            destination = new Vector2Int(end.positions[0].x, startTile.y);
        }
        if (bd == Direction.WEST)
        {
            destination = new Vector2Int(end.positions[0].x + end.width - 1, startTile.y);
        }

        if (!CheckRouteClear(building, destination, bd))
            return;

        Build(building, destination, bd);

    }

    void BuildCorridorNoL(Feature start, Feature end, Direction bd)
    {
        Vector2Int destination = new Vector2Int (0,0);
        Vector2Int building = new Vector2Int(0, 0);
        Vector2Int startTile = new Vector2Int (0,0);
        for(int count = 0; count < 100; count ++) 
        {
            startTile = ChooseWallTile(start, bd);
            if (bd == Direction.NORTH)
            {
                destination = new Vector2Int(startTile.x, end.positions[0].y);
                building = new Vector2Int(startTile.x, startTile.y + 1);
            }
            if (bd == Direction.SOUTH)
            {
                destination = new Vector2Int(startTile.x, end.positions[0].y + end.height - 1);
                building = new Vector2Int(startTile.x, startTile.y - 1);
            }
            if (bd == Direction.EAST)
            {
                destination = new Vector2Int(end.positions[0].x, startTile.y);
                building = new Vector2Int(startTile.x + 1, startTile.y);
            }
            if (bd == Direction.WEST)
            {
                destination = new Vector2Int(end.positions[0].x + end.width - 1, startTile.y);
                building = new Vector2Int(startTile.x - 1, startTile.y);
            }
            if (end.positions.Contains(destination) && !Cornered(destination, end, bd) && CheckRouteClear(startTile,destination,bd)){
                break;
            }
            if (count >= 99)
            {
                //Debug.LogError("Something is catastrophically wrong! What we know:\n" +
                //    "its building to: " + destination.x +"," + destination.y +"\n From: "+
                //    startTile.x + "," + startTile.y);
                return;
            }
        } 
        MapManager.map[startTile.x, startTile.y].type = "door";
        MapManager.map[startTile.x, startTile.y].sprite = "door";
        MapManager.map[startTile.x, startTile.y].occupied = false;

        Build(building, destination, bd);

    }

    void Build(Vector2Int building, Vector2Int destination, Direction bd)
    {
        while (building != destination)
        {
            if (MapManager.map[building.x, building.y] != null)
            {
                if (MapManager.map[building.x, building.y].type == "wall")
                {
                    MapManager.map[building.x, building.y].type = "door";
                    MapManager.map[building.x, building.y].sprite = "door";
                    MapManager.map[building.x, building.y].occupied = false;
                }
            }
            else
            {
                MapManager.map[building.x, building.y] = new Tile();
                MapManager.map[building.x, building.y].position = building;
                MapManager.map[building.x, building.y].type = "walk";
                MapManager.map[building.x, building.y].sprite = "walk";
            }

            if (bd == Direction.NORTH)
            {
                building += Vector2Int.up;
            }
            if (bd == Direction.SOUTH)
            {
                building += Vector2Int.down;
            }
            if (bd == Direction.EAST)
            {
                building += Vector2Int.right;
            }
            if (bd == Direction.WEST)
            {
                building += Vector2Int.left;
            }
        }

        if (MapManager.map[building.x, building.y] != null)
        {
            if (MapManager.map[building.x, building.y].type == "wall")
            {
                MapManager.map[building.x, building.y].type = "door";
                MapManager.map[building.x, building.y].sprite = "door";
                MapManager.map[building.x, building.y].occupied = false;
            }
        }
    }
    Direction GetInitialDirection(Feature start, Feature end)
    {

        float xdiff = Mathf.Abs(start.centerPoint.x - end.centerPoint.x);
        float ydiff = Mathf.Abs(start.centerPoint.y - end.centerPoint.y);

        if (xdiff >= ydiff)
        {
            if (start.centerPoint.x - end.centerPoint.x <= 0)
                return Direction.EAST;
            else
                return Direction.WEST;
        }

        else
        {
            if (start.centerPoint.y - end.centerPoint.y <= 0)
                return Direction.NORTH;
            else
                return Direction.SOUTH;
        }
    }

    Direction GetNextDirection(Vector2Int start, Feature end)
    {

        float xdiff = Mathf.Abs(start.x - end.centerPoint.x);
        float ydiff = Mathf.Abs(start.y - end.centerPoint.y);

        if (xdiff >= ydiff)
        {
            if (start.x - end.centerPoint.x <= 0)
                return Direction.EAST;
            else
                return Direction.WEST;
        }

        else
        {
            if (start.y - end.centerPoint.y <= 0)
                return Direction.NORTH;
            else
                return Direction.SOUTH;
        }
    }

    Vector2Int ChooseWallTile(Feature start, Direction direction)
    {
        Wall wall = start.walls[direction];
        int tile = 0;
        tile = Random.Range(1, wall.length - 1);
        return wall.position[tile];
    }

    bool DetermineL(Feature start, Feature end, Direction dir)
    {
        int startx = start.positions[0].x;
        int starty = start.positions[0].y;

        int endx = end.positions[0].x;
        int endy = end.positions[0].y;

        List<int> spanStart = new List<int>();
        List<int> spanEnd = new List<int>();


        if (dir == Direction.SOUTH || dir == Direction.NORTH)
        {
            for (int i = 0; i < start.width - 1; i++)
            {
                spanStart.Add(startx + i);
            }
            for (int i = 0; i < end.width - 1; i++)
            {
                spanEnd.Add(endx + i);
            }

            //spanStart.Remove(0);
            //spanStart.Remove(spanStart.Count - 1);

            //spanEnd.Remove(0);
            //spanEnd.Remove(spanStart.Count - 1);

            foreach (int i in spanStart)
            {
                if (spanEnd.Contains(i))
                {
                    return false;
                }
            }
        }
        else
        {
            for (int i = 0; i < start.height - 1; i++)
            {
                spanStart.Add(starty + i);
            }
            for (int i = 0; i < end.height - 1; i++)
            {
                spanEnd.Add(endy + i);
            }

            //spanStart.Remove(0);
            //spanStart.Remove(spanStart.Count - 1);

            //spanEnd.Remove(0);
            //spanEnd.Remove(spanStart.Count - 1);

            foreach (int i in spanStart)
            {
                if (spanEnd.Contains(i))
                {
                    return false;
                }
            }
        }


        return true;
    }

    bool Cornered(Vector2Int check, Feature room, Direction dir)
    {
        //Debug.Log("Corner checking! There should be 1 for every hallway at least");
        //Debug.Log("Check X: " + check.x + " Check Y: " + check.y);
        //Debug.Log("Check X Min: " + room.positions[0].x + " Check X max: " + (room.positions[0].x + room.width));
        //Debug.Log("Check Y Min: " + room.positions[0].y + " Check Y min: " + (room.positions[0].y + room.height));
        if (dir == Direction.NORTH || dir == Direction.SOUTH)
        {
            if (check.x == room.positions[0].x)
            {
                return true;
            }
            if (check.x == room.positions[0].x + room.width - 1)
            {
                return true;
            }
        }
        if (dir == Direction.EAST || dir == Direction.WEST)
        {
            if (check.y == room.positions[0].y)
            {
                return true;
            }

            if (check.y == room.positions[0].y + room.height - 1)
            {
                return true;
            }
        }
        return false;
    }

    bool CheckRouteClear(Vector2Int start, Vector2Int end, Direction dir)
    {
        //return true;
        Vector2Int increment = new Vector2Int(0,0);
        string previousTile;
        if (MapManager.map[start.x, start.y] != null)
        {
            previousTile = MapManager.map[start.x, start.y].type;
        }
        else
        {
            previousTile = "space";
        }
        switch (dir)
        {
            case Direction.NORTH:
                increment = Vector2Int.up;
                break;
            case Direction.SOUTH:
                increment = Vector2Int.down;
                break;
            case Direction.EAST:
                increment = Vector2Int.right;
                break;
            case Direction.WEST:
                increment = Vector2Int.left;
                break;
            default:
                break;
        }

        for (int i = 0; i < 9999; i++)
        {

            if (start == end)
            {
                return true;
            }
            
            start += increment;

            if (MapManager.map[start.x, start.y] == null)
            {
                previousTile = "space";
                continue;
            }

            else if (MapManager.map[start.x, start.y].type == "wall" && previousTile == "wall")
            {
                return false;
            }

            else
            {
                previousTile = MapManager.map[start.x, start.y].type;
            }
        }
        Debug.LogError("Something has gone horribly wrong! Errors incoming!");
        return false;
    }
}
