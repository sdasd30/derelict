using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
    MapGenerator2 mapGen;
    RenderTilemap render;
    Stopwatch timer;
    private void Start()
    {
        timer = new Stopwatch();
        mapGen = GetComponent<MapGenerator2>();
        render = GetComponent<RenderTilemap>();

        MakeMap();
        FoV.Initialize();
    }

    public void MakeMap()
    {
        timer.Start();
        int attempts = 1;

        for (int i = 0; i < 15; i++)
        {
            try
            {
                mapGen.BuildMap();
                //if (!mapGen.AstarPathCheck())
                //{
                //    attempts++;
                //    continue;
                //}
                //else


                break;
            }
            catch
            {
                attempts++;
            }

            if (i == 10)
            {
                Debug.LogError("Could not build map after 10 attempts. Something is wrong. Delete this try catch and debug.");
                break;
            }
        }

        //Debug.Log("Built map after " + attempts + " attempt(s)");

        render.ClearMap();
        render.DrawMap();
        //GetComponent<DEBUGDrawCenterPoints>().drawPoints();

        FindObjectOfType<PlayerManager>().PlacePlayer();

        timer.Stop();
        Debug.Log("Making generated in " + attempts+ " attempt(s) taking "+ timer.ElapsedMilliseconds + " miliseconds");
        timer.Reset();
    }

    public void RebuildMap()
    {
        render.ClearMap();
        mapGen.ClearMap();

        MakeMap();
    }
}
