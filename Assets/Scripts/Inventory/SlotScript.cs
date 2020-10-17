using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    /// <summary>
    /// 格子中的所有堆叠
    /// </summary>
    private ObservableStack<Item> items = new ObservableStack<Item>();

    // 格子显示的图标
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;

    /// <summary>
    /// 当前物品数量是否耗尽
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            return items.Count == 0;
        }
    }

    /// <summary>
    /// 检测物体是否达到最大上限
    /// </summary>
    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 格子中的物品类
    /// </summary>
    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return items.Peek();
            }

            return null;
        }
    }

    public Image MyIcon
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


    public int MyCount
    {
        get {return items.Count; }
    }

    /// <summary>
    /// 堆叠文本
    /// </summary>
    public Text MyStackText
    {
        get
        {
           return stackSize;
        }
    }

    private void Awake()
    {
        // 将可观察堆栈上的所有事件分配给updateSlot函数
        items.OnPop += new UpdateStackEvent(UpdateSlot);
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    /// <summary>
    /// 当格子被点击
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) // 如果没有东西需要移动
            {
                HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                InventoryScript.MyInstance.FromSlot = this;
            }
            else if (InventoryScript.MyInstance.FromSlot != null)// 如果有东西需要移动
            {
                //尝试用不同的方法把这些物品放回库存
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) ||SwapItems(InventoryScript.MyInstance.FromSlot) ||AddItems(InventoryScript.MyInstance.FromSlot.items))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
      
        }
        if (eventData.button == PointerEventData.InputButton.Right)//右键点击
        {
            UseItem();
        }
    }

    /// <summary>
    /// 将物品添加到格子中
    /// </summary>
    /// <param name="item">添加的物品类</param>
    /// <returns>是否添加成功</returns>
    public bool AddItem(Item item)
    {
        items.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    /// <summary>
    /// 添加堆叠物品
    /// </summary>
    /// <param name="newItems">对叠的物品</param>
    /// <returns></returns>
    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
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
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            items.Pop();
        }
    }

    public void Clear()
    {
        if (items.Count > 0)
        {
            items.Clear();
        }
    }

    /// <summary>
    /// 使用物品，如果在可见格子中
    /// </summary>
    public void UseItem()
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
      
    }

    /// <summary>
    /// 合并两个物体
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize)
        {
            items.Push(item);
            item.MySlot = this;
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
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
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
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            // 获取拖拽的物品
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items);

            // 清空被拖拽物品的格子
            from.items.Clear();
            // 将此格子物品添加到之前格子中
            from.AddItems(items);

            // 清空当前格子
            items.Clear();
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
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            //当前格子还能堆叠的数量
            int free = MyItem.MyStackSize - MyCount;

            for (int i = 0; i < free; i++)
            {
                AddItem(from.items.Pop());
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
        UIManager.MyInstance.UpdateStackSize(this);
    }
}
