using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterTile : Tile
{

    [SerializeField]
    private Sprite[] waterSprites;


    [SerializeField]
    private Sprite preview;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        GameManager.MyInstance.Blocked.Add(position);
        return base.StartUp(position, tilemap, go);
    }

  
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
       
        for (int y = -1; y <= 1; y++) 
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);

                if ( HasWater(tilemap, nPos))
                {
                    tilemap.RefreshTile(nPos); 
                }
            }
        }


    }


    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
      
        base.GetTileData(location, tilemap, ref tileData);

        string composition = string.Empty;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                {

                    if (HasWater(tilemap, new Vector3Int(location.x + x, location.y + y, location.z)))
                    {
                        composition += 'W'; 
                    }
                    else
                    {
                        composition += 'E';
                    }


                }
            }
        }

        int randomVal = Random.Range(0, 100);

        if (randomVal < 15)
        {
            tileData.sprite = waterSprites[46];
        }
        else if (randomVal >= 15 && randomVal < 35)
        {

            tileData.sprite = waterSprites[48];
        }
        else
        {
            tileData.sprite = waterSprites[47];
        }


        if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[0];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'W' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[1];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[2];
        }
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'W' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[3];
        }
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[4];
        }
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[5];
        }
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'W' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[6];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = waterSprites[7];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W' && composition[5] == 'E' && composition[7] == 'W')
        {
            tileData.sprite = waterSprites[8];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = waterSprites[9];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'E' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = waterSprites[10];
        }
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[11];
        }
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[12];
        }
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[13];
        }
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[14];
        }
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[15];
        }
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[16];
        }
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = waterSprites[17];
        }
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = waterSprites[18];
        }
        else if (composition[1] == 'W' && composition[2] == 'W' && composition[4] == 'W' && composition[3] == 'E' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = waterSprites[19];
        }
        else if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = waterSprites[20];
        }
        else if (composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = waterSprites[21];
        }
        else if (composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = waterSprites[22];
        }
        else if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[23];
        }
        else if (composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[24];
        }
        else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[25];
        }
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[26];
        }
        else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[27];
        }
        else if (composition[1] == 'E' && composition[4] == 'W' && composition[3] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[28];
        }
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[6] == 'E' && composition[4] == 'W')
        {
            tileData.sprite = waterSprites[29];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[30];
        }
        else if (composition == "EWWWWEWW")
        {
            tileData.sprite = waterSprites[31];
        }
        else if (composition == "EWEWWWWE")
        {
            tileData.sprite = waterSprites[32];
        }
        else if (composition == "EWEWWWWW")
        {
            tileData.sprite = waterSprites[33];
        }
        else if (composition == "WWWWWEWW")
        {
            tileData.sprite = waterSprites[34];
        }
        else if (composition == "WWEWWWWE")
        {
            tileData.sprite = waterSprites[35];
        }
        else if (composition == "WWWWWWWE")
        {
            tileData.sprite = waterSprites[36];
        }
        else if (composition == "EWWWWWWW")
        {
            tileData.sprite = waterSprites[37];
        }
        else if (composition == "WWEWWWWW")
        {
            tileData.sprite = waterSprites[38];
        }
        else if (composition == "EWWWWWWE")
        {
            tileData.sprite = waterSprites[39];
        }
        else if (composition == "EWWWWEWE")
        {
            tileData.sprite = waterSprites[40];
        }
        else if (composition == "WWWWWEWE")
        {
            tileData.sprite = waterSprites[41];
        }
        else if (composition == "WWEWWEWW")
        {
            tileData.sprite = waterSprites[42];
        }
        else if (composition == "EWEWWEWW")
        {
            tileData.sprite = waterSprites[43];
        }
        else if (composition == "WWEWWEWE")
        {
            tileData.sprite = waterSprites[44];
        }
        else if (composition == "EWEWWEWE")
        {
            tileData.sprite = waterSprites[45];
        }
      
    }

    private bool HasWater(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/WaterTile")]
    public static void CreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Watertile", "New Watertile", "asset", "Save watertile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WaterTile>(), path);
    }

#endif
}
