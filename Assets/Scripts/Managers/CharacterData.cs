using System;
using System.Collections.Generic;
using Items;
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
//        Initial();
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

    public void Initial()
    {
        initialCharacter();
        bagBar.Add(0,new ItemInGame(DataManager.Instance.GetItem(1009000000)));
//        BagBarScript.Instance.Initial(4,bagBar);
        
//        ObtainItem(1009000000);
//        SpellBook.Instance.Initial(spells);
//        ActionBar.Instance.Initial(actionButtons);
    }

    /// <summary>
    /// 初始化角色数据
    /// </summary>
    public void initialCharacter()
    {
//        CharacterInGame characterInGame=new CharacterInGame(charactersInGame[3000000000]);
//        ObtainItem(characterInGame.items[0]).Use();
    }
    
    /// <summary>
    /// 获得装备
    ///     如果为技能则是学习技能
    /// </summary>
    public ItemInGame ObtainItem(long uid)
    {
        int type=Utils.GetItemType(uid);
        ItemInGame itemInGame = new ItemInGame(DataManager.Instance.GetItem(uid,type));
        InventoryScript.Instance.AddItem(itemInGame);
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
