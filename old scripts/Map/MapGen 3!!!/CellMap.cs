using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMap : MonoBehaviour
{
    public static Cell[,] cells; //This is the map with information of all tiles.
    public static Feature[] features;

    public class Cell //Each cell will possibly contain a room.
    {
        public int ID; //Which room is this?
        public string type; //Is this a room or a hallway?
        public Vector2Int location; //Which cell does this cell occupy?
        public bool exists; //Does this cell currently contain a room?

    }
}
