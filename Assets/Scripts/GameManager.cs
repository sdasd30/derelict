using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    MapGenerator2 mapGen;
    RenderTilemap render;

    private void Start()
    {
        mapGen = GetComponent<MapGenerator2>();
        render = GetComponent<RenderTilemap>();

        MakeMap();
    }

    public void MakeMap()
    {
        mapGen.BuildMap();

        render.ClearMap();
        render.DrawMap();
        //GetComponent<DEBUGDrawCenterPoints>().drawPoints();
    }

    public void RebuildMap()
    {
        render.ClearMap();
        mapGen.ClearMap();

        MakeMap();
    }
}
