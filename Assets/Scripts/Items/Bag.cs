﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag",menuName ="Items/Bag",order =1)]
public class Bag : Item, IUseable
{
    /// <summary>
    /// 背包有的格子数
    /// </summary>
    private int slots;

    /// <summary>
    ///背包预制体
    /// </summary>
    [SerializeField]
    private GameObject bagPrefab;

    public BagScript MyBagScript { get; set; }

    public int Slots
    {
        get
        {
            return slots;
        }
    }

    /// <summary>
    /// 初始化背包
    /// </summary>
    /// <param name="slots"></param>
    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    /// <summary>
    /// 装备背包（使用背包）
    /// </summary>
    public void Use()
    {
        if (InventoryScript.MyInstance.CanAddBag)
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);

            InventoryScript.MyInstance.AddBag(this);
        }
 
    }
}
