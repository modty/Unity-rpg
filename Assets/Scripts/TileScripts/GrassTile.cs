using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassTile : Tile
{

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        return base.StartUp(position, tilemap, go);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/GrassTile")]
    public static void CreateGrassTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save GrassTile", "GrassTile", "asset", "Save GrassTile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GrassTile>(), path);
    }

#endif

}
