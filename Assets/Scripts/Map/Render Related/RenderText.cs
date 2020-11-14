using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderText : MonoBehaviour
{
    Text screen;
    public void DrawMap()
    {
        screen = GetComponent<Text>();
        string asciiMap = "";

        for (int y = (MapManager.map.GetLength(1) - 1); y >= 0; y--)
        {
            for (int x = 0; x < MapManager.map.GetLength(0); x++)
            {
                if(x == MapManager.map.GetLength(0))
                {
                    asciiMap += "\n";
                }

                if (MapManager.map[x,y] != null)
                {
                    switch (MapManager.map[x,y].type)
                    {
                        case "wall":
                            asciiMap += "#";
                            break;
                        case "floor":
                            asciiMap += ".";
                            break;
                        //case "door":
                        //    asciiMap += "O";
                    }
                }
                else
                {
                    asciiMap += ".";
                }
            }
        }

        screen.text = asciiMap;
    }
}
