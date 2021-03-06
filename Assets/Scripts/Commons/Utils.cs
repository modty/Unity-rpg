
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using Newtonsoft.Json;
using Object = UnityEngine.Object;


/// <summary>
/// 游戏中的公共方法类,只提供静态方法
/// </summary>
public class Utils
{

    /// <summary>
    /// 物品类别
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static int GetItemType(long uid)
    {
        return (int) ((uid / 1000000000000) % 1000);
    }
    /// <summary>
    /// 物品品质
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static int GetQuality(long uid)
    {
        return (int) ((uid / 1000000000) % 1000);
    }
    /// <summary>
    /// 物品用途
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static int GetUseType(long uid)
    {
        
        return (int) ((uid / 1000000) % 1000);
    }
    /// <summary>
    /// 物品基础ID
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static int GetItemId(long uid)
    {
        return (int) (uid% 1000);
    }
    public static T LoadJSON<T>(string dir)
    {
        StreamReader streamReader = new StreamReader("Assets/"+dir);
        string str = streamReader.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(str);
    }


    /// <summary>
    /// 使用 IO 流加载图片，并返回。
    /// </summary>
    /// <param name="_url">图片地址</param>
    /// <returns></returns>
    public static Texture2D LoadTexture2DByIO(string _url)
    {
        //创建文件读取流
        FileStream _fileStream = new FileStream("Assets/"+_url, FileMode.Open, FileAccess.Read);
        _fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] _bytes = new byte[_fileStream.Length];
        _fileStream.Read(_bytes, 0, (int)_fileStream.Length);
        _fileStream.Close();
        _fileStream.Dispose();
        //创建Texture
        Texture2D _texture2D = new Texture2D(1, 1);
        _texture2D.LoadImage(_bytes);
        return _texture2D;
    }
    /// <summary>
    /// 使用 IO 流加载图片，并将图片转换成 Sprite 类型返回
    /// </summary>
    /// <param name="_url">图片地址</param>
    /// <returns></returns>
    public static Sprite LoadSpriteByIO(string _url)
    {
        Texture2D _texture2D = null;
        if ("unknown".Equals(_url))
            _texture2D= LoadTexture2DByIO("Sprites/Default.png");
        else
            _texture2D= LoadTexture2DByIO(_url);
        Sprite _sprite = Sprite.Create(_texture2D, new Rect(0, 0, _texture2D.width, _texture2D.height), new Vector2(0.5f, 0.5f));
        return _sprite;
    }
    public static Sprite SplitSpriteByIO(Texture2D _texture2D,int startX,int startY,int endX,int endY)
    {
        Sprite _sprite = Sprite.Create(_texture2D, new Rect(startX, startY, endX, endY), new Vector2(0.5f, 0.5f));
        return _sprite;
    }
    public static Sprite[] SplitSpriteByIO(string url,int XNum,int YNum)
    {
        Texture2D texture2D = LoadTexture2DByIO(url);
        int perWidth = texture2D.width / XNum;
        int perHeight = texture2D.height / YNum;
        Sprite[] sprites=new Sprite[XNum*YNum];
        for (int i = 0; i < XNum; i++)
        {
            for (int j = 0; j < YNum; j++)
            {
                sprites[i] = SplitSpriteByIO(texture2D, i, j, (i + 1) * perWidth, (j + 1) * perHeight);
            }
        }
        return sprites;
    }
    
    /// <summary>
    /// 深度克隆一个对象
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public  static  T  Clone<T>(T  obj)
    {
        Stream objectStream = new MemoryStream();
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(objectStream, obj);
        objectStream.Seek(0, SeekOrigin.Begin);
        return (T)formatter.Deserialize(objectStream);
    }

    /// <summary>
    /// 角色获得一个物品（拾取等）
    /// </summary>
    /// <param name="uid"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static void Obtain(long uid)
    {
        
    }

    public static void ItemAttributeHelper(Dictionary<string,Dictionary<string,int>> attribute,string key,long fromId,long toUid,string enterAction)
    {
        Dictionary<string,int> temp=new Dictionary<string, int>();
        AttributeChange attributeChange=new AttributeChange(fromId,toUid,temp);
        Dictionary<string,int> dictionary = attribute[key];
        int[] param={0,0,-1,1,0,0};
        if (dictionary.TryGetValue(Constants.BaseValue, out param[0]))
        {
            temp[key]=param[0];
            // 立刻恢复生命值
            EventCenter.Broadcast<AttributeChange>(enterAction+":"+attributeChange.ToUid,attributeChange);
        }
        // 消耗品是持续性消耗品
        if (dictionary.TryGetValue(Constants.Duration,out param[2]) && param[2]!=-1)
        {
            if (dictionary.TryGetValue(Constants.Frequency, out param[3]) && param[3] > 0)
            {
                param[5] = param[2] / param[3];
                if (dictionary.TryGetValue(Constants.Value, out param[1]))
                {
                    Timer _testTimer = null;
                    _testTimer = Timer.Register(
                        duration: 1f,
                        () =>
                        {
                            temp[key]=param[1];
                            EventCenter.Broadcast<AttributeChange>(enterAction+":"+attributeChange.ToUid,attributeChange);
                            param[4]++;
                            if (param[4] >= param[5])
                            {
                                _testTimer.Cancel();
                            }
                        },
                        isLooped: true);
                }
            }
                
        }
    }


}
