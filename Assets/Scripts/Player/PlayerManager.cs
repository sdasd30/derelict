using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string preferedRoomType;
    
    public void PlacePlayer()
    {
        Feature room = MapManager.allFeatures[Random.Range(0, MapManager.allFeatures.Count - 1)];
        EntityPosition position = GetComponent<EntityPosition>();

        int random = Random.Range(0, room.positions.Count - 1);
        while(MapManager.map[room.positions[random].x, room.positions[random].y].occupied)
        {
            random = Random.Range(0, room.positions.Count - 1);
        }

        Debug.Log("Done!" + room.positions[random]);

        position.SetPosition(room.positions[random]);

    }

}
