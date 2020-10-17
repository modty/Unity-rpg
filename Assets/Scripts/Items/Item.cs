using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有物品的父类
/// </summary>
public abstract class Item : ScriptableObject, IMoveable
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
    /// 物品的格子
    /// </summary>
    private SlotScript slot;

    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    public int MyStackSize
    {
        get
        {
            return stackSize;
        }
    }

    public SlotScript MySlot
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

    /// <summary>
    /// 从仓库中移除物体
    /// </summary>
    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}

