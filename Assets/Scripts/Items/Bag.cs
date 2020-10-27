﻿using System.Collections;
using System.Collections.Generic;
 using Items;
 using New;
 using UnityEngine;

[CreateAssetMenu(fileName ="Bag",menuName ="Items/Bag",order =1)]
public class Bag : ItemInGame, IUseable
{
    /// <summary>
    /// 背包有的格子数
    /// </summary>
    [SerializeField]
    private int slots;

    /// <summary>
    ///背包预制体
    /// </summary>
    [SerializeField]
    private GameObject bagPrefab;

    public BagScript BagScript { get; set; }

    /// <summary>
    /// 背包绑定的按钮
    /// </summary>
    public BagButton BagButton { get; set; }

    /// <summary>
    /// 背包所有的格子数
    /// </summary>
    public int SlotCount
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
        if (InventoryScript.Instance.CanAddBag)
        {
//            Remove();
//            BagScript = Instantiate(bagPrefab, InventoryScript.Instance.transform).GetComponent<BagScript>();
            BagScript.AddSlots(slots);

            if (BagButton == null)
            {
                InventoryScript.Instance.AddBag(this);
            }
            else
            {
                InventoryScript.Instance.AddBag(this,BagButton);
            }

            BagScript.BagIndex = BagButton.BagIndex;
        }
 
    }

    public void SetupScript()
    {
//        BagScript = Instantiate(bagPrefab, InventoryScript.Instance.transform).GetComponent<BagScript>();
        BagScript.AddSlots(slots);
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n{0} 格子背包", slots);
    }

    public Bag(Item item) : base(item)
    {
    }
}
