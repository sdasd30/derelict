using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DoorGraph))]

public class DoorGenerator : MonoBehaviour
{
    //This script builds the doors, but does not handle the logic of where doors go. 
    //That honor goes to the DoorGraph script.

    List<DoorCell> DoorCells;

    public void Run()
    {
        GetComponent<DoorGraph>().Run();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

class DoorCell : MonoBehaviour
{
    Vector2Int Coordinates;
    Direction LinkedTo;

    public DoorCell(Vector2Int cord, Direction dir)
    {
        Coordinates = cord;
        LinkedTo = dir;
    }

}
