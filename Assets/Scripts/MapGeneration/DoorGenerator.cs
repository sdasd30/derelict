using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGenerator : MonoBehaviour
{
    List<DoorCell> DoorCells;
    // Start is called before the first frame update
    void Start()
    {
        
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
