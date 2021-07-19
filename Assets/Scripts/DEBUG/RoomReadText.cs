using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomReadText : MonoBehaviour
{
    MapGenerator3 MapGen;
    TextMeshProUGUI tmp;
    string ntext = "";

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    public void RenderText(int width, int height)
    {


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (MapManager.map[x, y] != null)
                {
                    //ntext += MapManager.map[x, y].roomID;
                    if (MapManager.map[x,y].sprite == "floor")
                    {
                        ntext += " ";
                    }
                    else if (MapManager.map[x, y].type == "corner")
                    {
                        ntext += MapManager.map[x,y].sprite;
                    }
                    else
                    {
                        ntext += "©";
                    }
                }
                else
                {
                    ntext += "*";
                }
            }
            ntext += "\n";
        }

        tmp.text = ntext;
    }

}
