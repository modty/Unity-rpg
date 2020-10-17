using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 物品品质枚举
/// </summary>
public enum Quality {Common, Uncommon, Rare, Epic }
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
    private string titel;

    /// <summary>
    /// 物品的品质
    /// </summary>
    [SerializeField]
    private Quality quality;

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
    /// 返回特定物品的颜色
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription()
    {
        string color = string.Empty;

        switch (quality)
        {
            case Quality.Common:
                color = "#d6d6d6";
                break;
            case Quality.Uncommon:
                color = "#00ff00ff";
                break;
            case Quality.Rare:
                color = "#0000ffff";
                break;
            case Quality.Epic:
                color = "#800080ff";
                break;
        }

        return string.Format("<color={0}>{1}</color>", color, titel);
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

