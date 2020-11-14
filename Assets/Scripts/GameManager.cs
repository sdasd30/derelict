using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    MapGenerator mapGen;
    RenderTilemap render;

    private void Start()
    {
        mapGen = GetComponent<MapGenerator>();
        render = GetComponent<RenderTilemap>();

        MakeMap();
    }

    public void MakeMap()
    {
        mapGen.InitializeMap();
        mapGen.FirstRoom();
        render.ClearMap();
        render.DrawMap();
    }
}
