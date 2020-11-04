using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class LOLBagBarScript : MonoBehaviour
{

    /// <summary>
    /// 背包显示UI板
    /// </summary>

    [SerializeField] private GameObject bagBarSlotPrefab;

    [SerializeField] private GameObject bagBarSlots;

    /// <summary>
    /// 背包装备栏
    /// </summary>
    private LOLBagBarButtonScript[] bags;

    private ItemInGame[] bagDatas;
    
    private bool[] isEquiped;

    private bool isLoadInventory;
    /// <summary>
    /// 加载数据（模拟）
    /// </summary>
    private void Awake()
    {
        bags=new LOLBagBarButtonScript[5];
        bagDatas=new ItemInGame[5];
        isEquiped=new bool[5];
        isEquiped[0] = true;
        isEquiped[1] = true;
        bagDatas[0] = new ItemInGame(DataManager.Instance.GetItem(1009000000));
        bagDatas[1] = new ItemInGame(DataManager.Instance.GetItem(1009000000));
        bagDatas[1].Capacity = 16;
        bagDatas[0].ContainItems[0]=new ItemInGame(DataManager.Instance.GetItem(2005000000));
        bagDatas[0].ContainItems[1]=new ItemInGame(DataManager.Instance.GetItem(3002000000));
        bagDatas[0].ContainItems[5]=new ItemInGame(DataManager.Instance.GetItem(1009000000));
        
        bagDatas[1].ContainItems[6]=new ItemInGame(DataManager.Instance.GetItem(2005000000));
        bagDatas[1].ContainItems[7]=new ItemInGame(DataManager.Instance.GetItem(3002000000));
        bagDatas[1].ContainItems[10]=new ItemInGame(DataManager.Instance.GetItem(1009000000));
        bagDatas[0].ContainItems[11]=new ItemInGame(DataManager.Instance.GetItem(2001000000000));
        bagDatas[0].ContainItems[11].StackCount=10;
    }

    private void Start()
    {
        LoadBagBar();
    }

    public void LoadBagBar()
    {
        for (int i = 0; i < bags.Length; i++)
        {
            GameObject obj = Instantiate(bagBarSlotPrefab, bagBarSlots.transform);
            bags[i]=obj.GetComponent<LOLBagBarButtonScript>();
            if (isEquiped[i]&&bags[i]!=null)
            {
                bags[i].Icon.sprite = bagDatas[i].Icon;
                bags[i].Icon.enabled=true;
                bags[i].ItemInGame = bagDatas[i];
                if (!isLoadInventory)
                {
                    LOLInventoryScript.Instance.LoadInventory(bagDatas[i]);
                    isLoadInventory = true;
                }
            }
        }
    }
    
    
}
