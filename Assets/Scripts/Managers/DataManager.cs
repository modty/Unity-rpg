using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{

    private static DataManager instance;
    private string UNKNOWN = "unknown";
    public static DataManager Instance =>  instance ?? (instance = new DataManager());
    private string weaponDir = "Data/Equipment.json";
    private string qualityColorDir = "Data/QualityColor.json";
    private string equipmentTypeDir = "Data/EquipmentType.json";
    private string spellDir = "Data/Spells.json";
    private string consumableDir = "Data/Consumable.json";
    private Dictionary<int, Quality> qualityColorDic; 
    private Dictionary<long,Equipment> weaponDic;
    private Dictionary<int,EquipmentType> equipmentTypeDic;
    private Dictionary<long,Spell> spellDic;
    private Dictionary<long,Consumable> consumableDic;
    
    private DataManager()
    {
        weaponDic = Utils.LoadJSON<Dictionary<long,Equipment>>(weaponDir);
        qualityColorDic = Utils.LoadJSON<Dictionary<int, Quality>>(qualityColorDir);
        equipmentTypeDic = Utils.LoadJSON< Dictionary<int,EquipmentType>>(equipmentTypeDir);
        spellDic = Utils.LoadJSON< Dictionary<long,Spell>>(spellDir);
        consumableDic = Utils.LoadJSON< Dictionary<long,Consumable>>(consumableDir);
    }
    /// <summary>
    /// 加载物品品质颜色
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public string GetItemQualityColor(long uid)
    {
        int qualityId = Utils.GetQuality(uid);
        return qualityColorDic[qualityId].color;
    }

    /// <summary>
    /// 加载物品的显示资源（物品名）
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public string GetItemName(long uid)
    {
        string res = UNKNOWN;
        switch (Utils.GetItemType(uid))
        {
            case 0:
                res=weaponDic[uid].name_cn;
                break;
            case 1:
                res = spellDic[uid].name_cn;
                break;
            case 2:
                res = consumableDic[uid].icon;
                break;
        }

        return res;
    }
    
    /// <summary>
    /// 比较两个uid是否唯一，所有比较都必须调用此方法，方便后续更改比较方法，或者通过其他方法比较等
    /// </summary>
    /// <param name="uid1"></param>
    /// <param name="uid2"></param>
    /// <returns></returns>
    public bool CompareUniqueId(long uid1,long uid2)
    {
        return uid1 == uid2;
    }

    public Sprite GetIcon(long uid)
    {
        string dir = UNKNOWN;
        switch (Utils.GetItemType(uid))
        {
            case 0:
                dir=new EquipmentInGame(weaponDic[uid]).icon;
                break;
            case 1:
                dir = new SpellInGame(spellDic[uid]).icon;
                break;
            case 2:
                dir = new ConsumableInGame(consumableDic[uid]).icon;
                break;
        }

        Sprite sprite = Utils.LoadSpriteByIO(dir);
        return sprite;
    }

    /// <summary>
    /// 获取物品的堆叠数量
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public int GetStackSize(long uid)
    {
        int stackSize = 0;
        switch (Utils.GetItemType(uid))
        {
            case 0:
                stackSize=weaponDic[uid].maxStackSize;
                break;
        }
        return stackSize;
    }
    public Item GetItem(long uid,int type)
    {
        Item item = null;
        switch (type)
        {
            case 0:
                item= weaponDic[uid];
                break;
            case 1:
                item=spellDic[uid];
                break;
            case 2:
                item=consumableDic[uid];
                break;
        }
        return item;
    }
    
    public Item GetItem(long uid)
    {
        int type = Utils.GetItemType(uid);
        Item item = null;
        switch (type)
        {
            case 0:
                item= new EquipmentInGame(weaponDic[uid]);
                break;
            case 1:
                item=new SpellInGame(spellDic[uid]);
                break;
            case 2:
                item=new ConsumableInGame(consumableDic[uid]);
                break;
        }
        return item;
    }

    public Quality GetQuality(long uid)
    {
        return qualityColorDic[Utils.GetQuality(uid)];
    }
}
