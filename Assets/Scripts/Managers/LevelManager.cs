using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;

// 根据图片像素创建地图
public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// 地图位置，放置被遮盖
    /// </summary>
    [SerializeField]
    private Transform map;

    /// <summary>
    /// 地图上面的所有元素
    /// </summary>
    [SerializeField]
    private Texture2D[] mapData;

    /// <summary>
    /// 地图瓷砖
    /// </summary>
    [SerializeField]
    private MapElement[] mapElements;

    /// <summary>
    /// 是用来测量瓷砖之间的距离
    /// </summary>
    [SerializeField]
    private Sprite defaultTile;

    /// <summary>
    /// Dictionay for all water tiles
    /// </summary>
    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();
    /// <summary>
    /// 所有水瓷砖
    /// </summary>
    [SerializeField]
    private SpriteAtlas waterAtlas;

    
    /// <summary>
    /// 屏幕左下角的位置
    /// </summary>
    private Vector3 WorldStartPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        }
    }


	void Start ()
    {
        GenerateMap();
	}
    

    /// <summary>
    /// 创造一个地图，根据像素图片
    /// </summary>
    private void GenerateMap()
    {
        int height = mapData[0].height;
        int width = mapData[0].width;
        for (int i = 0; i < mapData.Length; i++)// 遍历地图元素
        {
            for (int x = 0; x < mapData[i].width; x++) // 遍历所有像素点
            {
                for (int y = 0; y < mapData[i].height; y++)
                {
                    Color c = mapData[i].GetPixel(x, y); // 获取当前像素颜色

                    //检查是否有一个匹配地图上像素颜色的瓷砖
                    MapElement newElement = Array.Find(mapElements, e => e.MyColor == c);

                    if (newElement != null) // 获取到了颜色瓷砖
                    {
                        // 计算瓷砖位置
                        float xPos = WorldStartPos.x + (defaultTile.bounds.size.x * x);
                        float yPos = WorldStartPos.y + (defaultTile.bounds.size.y * y);

                        // 创建瓷砖
                        GameObject go = Instantiate(newElement.MyElementPrefab);
                        // 设置瓷砖位置
                        go.transform.position = new Vector2(xPos, yPos);
                        if (newElement.MyTileTag == "Water")
                        {
                            waterTiles.Add(new Point(x,y), go);
                        }
                        if (newElement.MyTileTag == "Tree")
                        {
                            // 将树放入地图需要设置其排序数
                            go.GetComponent<SpriteRenderer>().sortingOrder = height*2 - y*2;
                        }
                        
                        // 将瓷砖元素添加到Map中
                        go.transform.parent = map;

                    }

                }
            }
        }
        CheckWater();
    }
    /// <summary>
    /// 检测所有的水瓷砖，这样就能正确的切换图片
    /// </summary>
    private void CheckWater()
    {
       foreach (KeyValuePair<Point, GameObject> tile in waterTiles)
       {
            string composition = TileCheck(tile.Key);

            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("0");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("1");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("2");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("3");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("4");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("5");
            }
            if (composition[1] == 'W' && composition[4] == 'W' && composition[3] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("6");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("7");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("8");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("9");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("10");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("11");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("12");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("13");
            }
            if (composition[3] == 'W' && composition[5] == 'E' && composition[6] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("14");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[2] == 'E' && composition[4] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("15");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("16");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("17");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 15)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("19");
                }
            }
            if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W')
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 10)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("20");
                }

            }

       }
    }
    /// <summary>
    /// 检测每个瓷砖的相邻瓷砖
    /// </summary>
    /// <param name="currentPoint">当前所在瓷砖</param>
    /// <returns></returns>
    private string TileCheck(Point currentPoint)
    {
        string composition = string.Empty; 

        for (int x = -1; x <= 1; x++)//遍历相邻瓷砖
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0) //不检测自己
                {
                    // 如果检测到水
                    if (waterTiles.ContainsKey(new Point(currentPoint.MyX+x, currentPoint.MyY+y)))
                    {
                        composition += "W";
                    }
                    else
                    {
                        composition += "E";
                    }
                }
            }
        }
        Debug.Log(composition);
        return composition;
    }   
}

[Serializable]
public class MapElement
{
    /// <summary>
    /// 标记地图上的物品类型
    /// </summary>
    [SerializeField]
    private string tileTag;

    /// <summary>
    /// 瓷砖颜色
    /// </summary>
    [SerializeField]
    private Color color;

    /// <summary>
    /// 平铺地图的瓷砖预制件
    /// </summary>
    [SerializeField]
    private GameObject elementPrefab;

    public GameObject MyElementPrefab
    {
       get
        {
            return elementPrefab;
        }
    }

    public Color MyColor
    {
        get
        {
            return color;
        }
    }

    public string MyTileTag
    {
        get
        {
            return tileTag;
        }
    }
}
/// <summary>
/// 瓷片的世界位置
/// </summary>
public struct Point
{
    public int MyX { get; set; }
    public int MyY { get; set; }

    public Point(int x, int y)
    {
        this.MyX = x;
        this.MyY = y;
    }


}