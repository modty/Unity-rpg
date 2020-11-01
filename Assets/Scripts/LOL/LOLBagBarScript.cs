﻿using System;
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

    /// <summary>
    /// 加载数据（模拟）
    /// </summary>
    private void Awake()
    {
        bags=new LOLBagBarButtonScript[5];
        bagDatas=new ItemInGame[5];
        isEquiped=new bool[5];
        isEquiped[0] = true;
        bagDatas[0] = new ItemInGame(DataManager.Instance.GetItem(1009000000));
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
                LOLInventoryScript.Instance.LoadInventory(bagDatas[i]);
            }
        }
    }
    
    
}