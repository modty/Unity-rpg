using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

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
    }
    /// <summary>
    /// 角色拥有的技能
    /// </summary>
    private Dictionary<long,ItemInGame> spells;
    /// <summary>
    /// 角色快捷键绑定
    /// </summary>
    private Dictionary<int, ItemInGame> action;

    /// <summary>
    /// 消耗品<物品Uid,背包中该物品的数目>（后面会更改Value值）
    /// </summary>
    private Dictionary<long, ItemInGame> consumblesInGame;
    /// <summary>
    /// 游戏中所有角色（uid=3）,包括怪物等
    /// </summary>
    private Dictionary<long, CharacterInGame> charactersInGame;
    private Dictionary<int, ItemInGame> bagBar;


    /// <summary>
    /// 
    /// </summary>
    private Dictionary<long, ItemInGame> equipmentsInGame;
    public void loadData()
    {
        _dataManager=DataManager.Instance;
        spells=new Dictionary<long, ItemInGame>();
        action=new Dictionary<int, ItemInGame>();
        bagBar=new Dictionary<int, ItemInGame>();
        consumblesInGame=new Dictionary<long, ItemInGame>();
        charactersInGame=new Dictionary<long, CharacterInGame>();
        equipmentsInGame=new Dictionary<long, ItemInGame>();
    }

    public void Start()
    {
        InitialCharacter();
        InitialStat();
        InitialBagItems();
    }

    private void InitialCharacter()
    {
        ControlledChaState controlledChaStat=new ControlledChaState();
        controlledChaStat.Uid = 3000000000000;
        controlledChaStat.Experience = 100;
        controlledChaStat.Health=new []{2000,3128};
        controlledChaStat.Mana=new []{540,1235};
        controlledChaStat.GoldNum = 123;
        List<int> list=new List<int>();
        list.AddRange(new []{74,18,97,36,80,10,0,380});
        controlledChaStat.BaseAttribute=list;
        StatScript.Instance.ControlledControlledChaState = controlledChaStat;
        Player.Instance.ControlledChaState = controlledChaStat;
        Dictionary<int,ItemInGame> shortCuts=new Dictionary<int, ItemInGame>();
        shortCuts.Add(1,new ItemInGame(DataManager.Instance.GetItem(2005000000)));
        shortCuts.Add(2,new ItemInGame(DataManager.Instance.GetItem(3002000000)));
        shortCuts.Add(3,new ItemInGame(DataManager.Instance.GetItem(1009000000)));
        shortCuts.Add(5,new ItemInGame(DataManager.Instance.GetItem(2000000000000)));
        shortCuts.Add(6,null);
        shortCuts.Add(7,null);
        controlledChaStat.ItemShortCuts = shortCuts;
    }
    private void InitialStat()
    {
        StatScript.Instance.Initial();
        
    }
    private void InitialBagItems()
    {
        BagBarScript bagBarScript=BagBarScript.Instance;
        bagBarScript.Bags=new BagBarButtonScript[5];
        bagBarScript.BagDatas=new ItemInGame[5];
        bagBarScript.IsEquiped=new bool[5];
        bagBarScript.IsEquiped[0] = true;
        bagBarScript.IsEquiped[1] = true;
        bagBarScript.BagDatas[0] = new ItemInGame(DataManager.Instance.GetItem(1009000000));
        bagBarScript.BagDatas[1] = new ItemInGame(DataManager.Instance.GetItem(1009000000));
        bagBarScript.BagDatas[1].Capacity = 16;
        bagBarScript.BagDatas[0].ContainItems[0]=new ItemInGame(DataManager.Instance.GetItem(2005000000));
        bagBarScript.BagDatas[0].ContainItems[1]=new ItemInGame(DataManager.Instance.GetItem(3002000000));
        bagBarScript.BagDatas[0].ContainItems[5]=new ItemInGame(DataManager.Instance.GetItem(1009000000));
        
        bagBarScript.BagDatas[1].ContainItems[6]=new ItemInGame(DataManager.Instance.GetItem(2000000000000));
        bagBarScript.BagDatas[1].ContainItems[7]=new ItemInGame(DataManager.Instance.GetItem(3002000000));
        bagBarScript.BagDatas[1].ContainItems[10]=new ItemInGame(DataManager.Instance.GetItem(1009000000));
        bagBarScript.BagDatas[0].ContainItems[11]=new ItemInGame(DataManager.Instance.GetItem(2000000000000));
        bagBarScript.BagDatas[0].ContainItems[11].StackCount=10;
    }
    
    
    /// <summary>
    /// 获得装备
    ///     如果为技能则是学习技能
    /// </summary>
    public ItemInGame ObtainItem(long uid)
    {
        int type=Utils.GetItemType(uid);
        ItemInGame itemInGame = new ItemInGame(DataManager.Instance.GetItem(uid,type));
        if (itemInGame.InventoryPosition[1] != -1)
        {
            switch (type)
            {
                case 0:
                    equipmentsInGame.Add(uid,itemInGame);
                    break;
                case 1:
                    spells.Add(uid,itemInGame);
                    break;
                case 2:
                    consumblesInGame.Add(uid,itemInGame);
                    break;
            }
        }
        return itemInGame;
    }
}
