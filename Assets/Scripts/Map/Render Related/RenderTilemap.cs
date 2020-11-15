using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RenderTilemap : MonoBehaviour
{
    Tilemap gridMap;
    public TileBase missingTile;
    public TileBase floorTile;
    public TileBase wallTile;
    public TileBase doorTile;
    public TileBase walkTile;
    public TileBase hullTile;

    private void Awake()
    {
        gridMap = FindObjectOfType<Tilemap>();
    }

    public void DrawMap() //ATTENTION! THIS IS A TESTING SCRIPT. IT SHOULD NOT BE USED!
    {
        for (int x = 0; x < MapManager.map.GetLength(0); x++)
        {
            for (int y = 0; y < MapManager.map.GetLength(1); y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (MapManager.map[x, y] != null)
                {
                    switch (MapManager.map[x, y].sprite)
                    {
                        case "wall":
                            gridMap.SetTile(tilePosition, wallTile);
                            break;
                        case "floor":
                            gridMap.SetTile(tilePosition, floorTile);
                            break;
                        case "door":
                            gridMap.SetTile(tilePosition, doorTile);
                            break;
                        case "walk":
                            gridMap.SetTile(tilePosition, walkTile);
                            break;
                        case "hull":
                            gridMap.SetTile(tilePosition, hullTile);
                            break;
                        default:
                            Debug.LogWarning("Map coordinates x:" + x + " y:" + y + " has no sprite associated with it! " +
                                "It is supposed to have sprite :" + MapManager.map[x,y].sprite);
                            gridMap.SetTile(tilePosition, missingTile);
                            break;
                    }
                }
                else
                {
                    //Debug.LogWarning("Map coordinates x:" + x + " y:" + y + " has no map tile!");
                }
            }
        }
    }

    public void ClearMap()
    {
        gridMap.ClearAllTiles();
    }
}
