using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CellReadText : MonoBehaviour
{
    MapGenerator3 MapGen;
    TextMeshProUGUI tmp;
    string ntext = "";

    private void Start()
    {
        MapGen = FindObjectOfType<MapGenerator3>();
        tmp = GetComponent<TextMeshProUGUI>(); 
    }

    public void RenderText(int width, int height)
    {
        

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (MapManager.cells[x, y].exists)
                {
                    ntext += MapManager.cells[x, y].ID;
                }
                else
                {
                    ntext += "*";
                }
            }
            ntext += "\n";
        }


        /*
        int count = 0; 
        foreach (Cell cell in MapManager.cells)
        {
            if (count == width)
            {
                count = 0;
                ntext += "\n";
            }
            count++;

            if (cell.exists)
            {
                ntext += cell.ID;
            }
            else
            {
                ntext += "*";
            }

        }
        Debug.Log(ntext);
        */


        tmp.text = ntext;
    }

}
