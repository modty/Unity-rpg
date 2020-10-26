using System;
using System.Collections.Generic;
using UnityEngine;
using New;

/// <summary>
/// 角色的数据
/// </summary>
public class CharacterData:MonoBehaviour
{
    public static CharacterData Instance;
    private DataManager _dataManager;

    private void Awake()
    {
        Instance = this;
        loadData();
        Initial();
    }
    /// <summary>
    /// 角色拥有的技能
    /// </summary>
    private Dictionary<long,Spell> spells;
    /// <summary>
    /// 角色快捷键绑定
    /// </summary>
    private Dictionary<int, long> actionButtons;

    /// <summary>
    /// 消耗品<物品Uid,背包中该物品的数目>（后面会更改Value值）
    /// </summary>
    private Dictionary<long, ConsumbleInGame> consumblesInGame;


    /// <summary>
    /// 
    /// </summary>
    private Dictionary<long, EquipmentInGame> equipmentsInGame;
    private CharacterData()
    {
        loadData();
        spells=new Dictionary<long, Spell>();
        actionButtons=new Dictionary<int, long>();
        consumblesInGame=new Dictionary<long, ConsumbleInGame>();
        equipmentsInGame=new Dictionary<long, EquipmentInGame>();
        ObtainItem(1001000000000);
        ObtainItem(1001000000001);
        ObtainItem(1001000000002);
        // 添加快捷键绑定
        actionButtons.Add(0,1001000000000);
        actionButtons.Add(3,1001000000001);
        // 背包中添加物品
        
//        // 15个苹果
//        bagItems.Add(2000000000,new ConsumbleInGame(_dataManager.GetConsumable(2000000000),new int[,]{{0,0},{0,14}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(2000000000).icon),15));
//        // 21个血瓶
//        bagItems.Add(2001000000,new ConsumbleInGame(_dataManager.GetConsumable(2001000000),new int[,]{{0,1},{0,2}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(2001000000).icon),21));
//        //装备
//        bagItems.Add(1006000000,new ConsumbleInGame(_dataManager.GetConsumable(1006000000),new int[,]{{0,3}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1006000000).icon),1));
//        bagItems.Add(1006000001,new ConsumbleInGame(_dataManager.GetConsumable(1006000001),new int[,]{{0,4}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1006000001).icon),1));
//        bagItems.Add(1007000000,new ConsumbleInGame(_dataManager.GetConsumable(1007000000),new int[,]{{0,5}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1007000000).icon),1));
//        bagItems.Add(1007000001,new ConsumbleInGame(_dataManager.GetConsumable(1007000001),new int[,]{{0,7}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1007000001).icon),1));
//        bagItems.Add(1000000000,new ConsumbleInGame(_dataManager.GetConsumable(1000000000),new int[,]{{0,8}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1000000000).icon),1));
//        bagItems.Add(1001000000,new ConsumbleInGame(_dataManager.GetConsumable(1001000000),new int[,]{{0,9}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1001000000).icon),1));
//        bagItems.Add(1002000000,new ConsumbleInGame(_dataManager.GetConsumable(1002000000),new int[,]{{0,10}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1002000000).icon),1));
//        bagItems.Add(1003000000,new ConsumbleInGame(_dataManager.GetConsumable(1003000000),new int[,]{{0,11}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1003000000).icon),1));
//        bagItems.Add(1004000000,new ConsumbleInGame(_dataManager.GetConsumable(1004000000),new int[,]{{0,12}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1004000000).icon),1));
//        bagItems.Add(1005000000,new ConsumbleInGame(_dataManager.GetConsumable(1005000000),new int[,]{{0,13}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1005000000).icon),1));
//        //背包
//        bagItems.Add(1008000000,new ConsumbleInGame(_dataManager.GetConsumable(1008000000),new int[,]{{0,15},{1,16}},Utils.LoadSpriteByIO(_dataManager.GetConsumable(1008000000).icon),2));
    }

    public void loadData()
    {
        _dataManager=DataManager.Instance;
    }

    public Spell GetSpell(long uid)
    {
        return spells[uid];
    }

    public void Initial()
    {
//        SpellBook.Instance.Initial(spells);
//        ActionBar.Instance.Initial(actionButtons);
    }

    public Sprite GetIcon(long uid)
    {
        string dir = "unknown";
        Debug.Log(Utils.GetItemType(uid));
        switch (Utils.GetItemType(uid))
        {
            case 1:
                dir=spells[uid].icon;
                break;
        }

        Sprite sprite = Utils.LoadSpriteByIO(dir);
        return sprite;
    }

    public int GetMaxStackSize(long uid)
    {
        int size = 1;
        switch (Utils.GetItemType(uid))
        {
            case 0:
                size = equipmentsInGame[uid].maxStackSize;
                break;
            case 2:
                size = consumblesInGame[uid].maxStackSize;
                break;

        }
        return size;
    }
    
    /// <summary>
    /// 获得装备
    ///     如果为技能则是学习技能
    /// </summary>
    public void ObtainItem(long uid)
    {
        switch (Utils.GetItemType(uid))
        {
            case 0:
                EquipmentInGame equipment=new EquipmentInGame(DataManager.Instance.GetEquipment(uid),new int[,]{{0,3}},1);
                equipmentsInGame.Add(uid,equipment);
                break;
            case 1:
                spells.Add(uid,DataManager.Instance.GetSpell(uid));
                break;
            case 2:
                ConsumbleInGame consumble=new ConsumbleInGame(DataManager.Instance.GetConsumable(uid),new int[,]{{0,3}},1);
                consumblesInGame.Add(uid,consumble);
                break;

        }
    }

}
