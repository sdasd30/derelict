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
    public TileBase debugTile;

    public bool uglyDoor;

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

        if (uglyDoor)
        {
            foreach (Tile tipo in MapManager.map)
            {
                if (tipo.type == "door")
                {
                    gridMap.SetTile(new Vector3Int(tipo.position.x, tipo.position.y, 0), debugTile);
                }
            }
        }

        //foreach (Tile tile in MapManager.map)
        //{
        //    gridMap.SetTileFlags(new Vector3Int(tile.position.x, tile.position.y, 0), TileFlags.None);
        //    gridMap.SetColor(new Vector3Int(tile.position.x, tile.position.y, 0), Color.clear);
        //}
    }

    public void UpdateVisibility()
    { //THIS SCRIPT SHOULD NOT UPDATE EVERY FRAME! IT SHOULD UPDATE ONLY WHEN PLAYER GETS CONTROL.
        foreach (Tile tile in MapManager.map)
        {
            if (tile.isVisible)
            {
                gridMap.SetColor(new Vector3Int(tile.position.x, tile.position.y, 0), Color.white);
            }
            else if (tile.isExplored)
            {
                gridMap.SetColor(new Vector3Int(tile.position.x, tile.position.y, 0), new Color(.2f,.2f,.2f,1));
            }
        }
    }

    private void Update()
    {
        //UpdateVisibility();
    }

    public void ClearMap()
    {
        gridMap.ClearAllTiles();
    }
}
