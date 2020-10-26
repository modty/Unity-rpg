﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 所有物品的父类
/// </summary>
public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    /// <summary>
    /// 移动和替换物品时的图标
    /// </summary>
    [SerializeField]
    private Sprite icon;

    /// <summary>
    /// 堆叠数量
    /// </summary>
    [SerializeField]
    private int stackSize;

    /// <summary>
    /// 物品的标题
    /// </summary>
    [SerializeField]
    private string title;

    private SlotScript slot;

    private CharButton charButton;

    [SerializeField]
    private int price;

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public int StackSize
    {
        get
        {
            return stackSize;
        }
    }

    public SlotScript Slot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }

    public string Title
    {
        get
        {
            return title;
        }
    }

    public CharButton CharButton
    {
        get
        {
            return charButton;
        }

        set
        {
            Slot = null;
            charButton = value;
        }
    }

    public int Price
    {
        get
        {
            return price;
        }
    }

    /// <summary>
    /// 返回特殊物品的描述
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription()
    {
        return string.Format("<color={0}>{1}</color>", "#00ff00ff", Title);
    }
    /// <summary>
    /// 从仓库中移除物体
    /// </summary>
    public void Remove()
    {
        if (Slot != null)
        {
            Slot.RemoveItem(this);

        }
    }
}

