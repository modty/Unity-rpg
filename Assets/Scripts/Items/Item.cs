using System.Collections;
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
    private string titel;

    /// <summary>
    /// 物品的品质
    /// </summary>
    [SerializeField]
    private Quality quality;

    private SlotScript slot;

    private CharButton charButton;

    [SerializeField]
    private int price;

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

    public Quality MyQuality
    {
        get
        {
            return quality;
        }
    }

    public string MyTitle
    {
        get
        {
            return titel;
        }
    }

    public CharButton MyCharButton
    {
        get
        {
            return charButton;
        }

        set
        {
            MySlot = null;
            charButton = value;
        }
    }

    public int MyPrice
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
        return string.Format("<color={0}>{1}</color>", QualityColor.MyColors[MyQuality], MyTitle);
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

