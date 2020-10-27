﻿using System.Collections;
using System.Collections.Generic;
 using Items;
 using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// 格子中的所有堆叠
    /// </summary>
    private ObservableStack<ItemInGame> items = new ObservableStack<ItemInGame>();

    // 格子显示的图标
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image cover;

    [SerializeField]
    private Text stackSize;

    /// <summary>
    /// 当前格子属于的背包
    /// </summary>
    public BagScript Bag { get; set; }
    
    public int Index { get; set; }

    /// <summary>
    /// 当前物品数量是否耗尽
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            return Items.Count == 0;
        }
    }

    /// <summary>
    /// 检测物体是否达到最大上限
    /// </summary>
    public bool IsFull
    {
        get
        {
            if (IsEmpty || Count < ItemInGame.StackCount)
            {
                return false;
            }

            return true;
        }
    }
    /// <summary>
    /// 格子中的物品类
    /// </summary>
    public ItemInGame ItemInGame
    {
        get
        {
            if (!IsEmpty)
            {
                return Items.Peek();
            }

            return null;
        }
    }

    public Image Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }


    public int Count
    {
        get {return Items.Count; }
    }

    /// <summary>
    /// 堆叠文本
    /// </summary>
    public Text StackText
    {
        get
        {
           return stackSize;
        }
    }

    public ObservableStack<ItemInGame> Items
    {
        get
        {
            return items;
        }
    }

    public Image Cover
    {
        get
        {
            return cover;
        }
    }

    private void Awake()
    {
        // 将可观察堆栈上的所有事件分配给updateSlot函数 
        Items.OnPop += new UpdateStackEvent(UpdateSlot);
        Items.OnPush += new UpdateStackEvent(UpdateSlot);
        Items.OnClear += new UpdateStackEvent(UpdateSlot);
    }


    /// <summary>
    /// 当格子被点击
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // 鼠标左键点击
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.Instance.FromSlot == null && !IsEmpty)
            {
                // 判断鼠标上是否拖拽背包物品
                if (HandScript.Instance.Moveable != null )
                {
                    if (HandScript.Instance.Moveable is Bag)
                    {
                        // 如果有，而且当前格子中也是背包，就切换背包
                        if (ItemInGame is Bag)
                        {
                            InventoryScript.Instance.SwapBags(HandScript.Instance.Moveable as Bag, ItemInGame as Bag);
                        }
                    }
                    else if (HandScript.Instance.Moveable is Armor)
                    {
                        if (ItemInGame is Armor && (ItemInGame as Armor).ArmorType == (HandScript.Instance.Moveable as Armor).ArmorType)
                        {
                            (ItemInGame as Armor).Equip();
                            
                            HandScript.Instance.Drop();
                        }
                    }
    
                }
                // 没有拖拽就直接拿起物品
                else
                {
                    HandScript.Instance.TakeMoveable(ItemInGame as IMoveable);
                    InventoryScript.Instance.FromSlot = this;
                }
        
            }
            // 如果来源不是背包格子（FromSlot为空）、当前格子中物品已经耗尽、鼠标上的是背包（即鼠标上该物品为从装备上取下来的物品）
            else if (InventoryScript.Instance.FromSlot == null && IsEmpty)
            {
                if (HandScript.Instance.Moveable is Bag)
                {
                    // 从鼠标上获取背包
                    Bag bag = (Bag)HandScript.Instance.Moveable;
                    // 确保脱下背包的时候不会放到自己里面，以及其他背包中有足够的空间
                    if (bag.BagScript != Bag && InventoryScript.Instance.EmptySlotCount - bag.SlotCount > 0)
                    {
                        // 将拖拽物品放到这个格子中
                        AddItem(bag);
                        // 装备栏中移除背包
                        bag.BagButton.RemoveBag();
                        // 鼠标上移除
                        HandScript.Instance.Drop();
                    }
                }
                else if (HandScript.Instance.Moveable is Armor)
                {
                    Armor armor = (Armor)HandScript.Instance.Moveable;
                    CharacterPanel.Instance.MySlectedButton.DequipArmor();
                    AddItem(armor);
                    HandScript.Instance.Drop();
                }


            }
            // 如果有物品需要移动（从一个格子到另一个格子）
            else if (InventoryScript.Instance.FromSlot != null)
            {
                //尝试不同的方法对该行为进行处理
                if (PutItemBack() || MergeItems(InventoryScript.Instance.FromSlot) ||SwapItems(InventoryScript.Instance.FromSlot) ||AddItems(InventoryScript.Instance.FromSlot.Items))
                {
                    HandScript.Instance.Drop();
                    InventoryScript.Instance.FromSlot = null;
                }
            }
      
        }
        //右键点击，使用物品
        if (eventData.button == PointerEventData.InputButton.Right && HandScript.Instance.Moveable == null)
        {
            UseItem();
        }
    }

    /// <summary>
    /// 将物品添加到格子中（当前格子中）
    /// </summary>
    /// <param name="itemInGame">添加的物品类</param>
    /// <returns>是否添加成功</returns>
    public bool AddItem(ItemInGame itemInGame)
    {
        Items.Push(itemInGame);
        icon.sprite = itemInGame.Icon;
        icon.color = Color.white;
        Cover.enabled = false;
//        itemInGame.Slot = this;
        return true;
    }

    /// <summary>
    /// 添加堆叠物品
    /// </summary>
    /// <param name="newItems">对叠的物品</param>
    /// <returns></returns>
    public bool AddItems(ObservableStack<ItemInGame> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == ItemInGame.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// 移除物品
    /// </summary>
    /// <param name="itemInGame"></param>
    public void RemoveItem(ItemInGame itemInGame)
    {
        if (!IsEmpty)
        {
            InventoryScript.Instance.OnItemCountChanged(Items.Pop());
        }
    }

    public void Clear()
    {
        int initCount = Items.Count;
        Cover.enabled = false;
        if (initCount > 0)
        {
            for (int i = 0; i < initCount; i++)
            {
                InventoryScript.Instance.OnItemCountChanged(Items.Pop());
            }
        }
    }

    /// <summary>
    /// 使用物品，如果在可见格子中
    /// </summary>
    public void UseItem()
    {
        if (ItemInGame is IUseable)
        {
            (ItemInGame as IUseable).Use();
        }
        else if (ItemInGame is Armor)
        {
            (ItemInGame as Armor).Equip();
        }
      
    }

    /// <summary>
    /// 合并两个物体
    /// </summary>
    /// <param name="itemInGame"></param>
    /// <returns></returns>
    public bool StackItem(ItemInGame itemInGame)
    {
        if (!IsEmpty && itemInGame.Uid == ItemInGame.Uid && itemInGame.StackCount < ItemInGame.MaxStackSize)
        {
            ItemInGame.StackCount++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 还原物品位置
    /// </summary>
    /// <returns></returns>
    private bool PutItemBack()
    {
        Cover.enabled = false;
        if (InventoryScript.Instance.FromSlot == this)
        {
            InventoryScript.Instance.FromSlot.Icon.enabled = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 交换两个物品
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    private bool SwapItems(SlotScript from)
    {
        from.Cover.enabled = false;
        if (IsEmpty)
        {
            return false;
        }
        if (from.ItemInGame.GetType() != ItemInGame.GetType() || from.Count+Count > ItemInGame.StackCount)
        {
            // 获取拖拽的物品
            ObservableStack<ItemInGame> tmpFrom = new ObservableStack<ItemInGame>(from.Items);
            // 清空被拖拽物品的格子
            from.Items.Clear();
            // 将此格子物品添加到之前格子中
            from.AddItems(Items);

            // 清空当前格子
            Items.Clear();
            // 将拖拽过来的物品添加到此格子
            AddItems(tmpFrom);

            return true;
        }

        return false;
    }
    /// <summary>
    /// 合并含有相同物品的格子（如果拖拽时）
    /// </summary>
    /// <param name="from">拖拽过来的物品</param>
    /// <returns></returns>
    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.ItemInGame.GetType() == ItemInGame.GetType() && !IsFull && from.ItemInGame.Name == ItemInGame.Name)
        {
            //当前格子还能堆叠的数量
            int free = ItemInGame.StackCount - Count;

            for (int i = 0; i < free; i++)
            {
                AddItem(from.Items.Pop());
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// 更新格子UI
    /// </summary>
    private void UpdateSlot()
    {
        UIManager.Instance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //显示工具界面
        if (!IsEmpty)
        {
//            UIManager.Instance.ShowTooltip(new Vector2(1, 0),transform.position, ItemInGame);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
