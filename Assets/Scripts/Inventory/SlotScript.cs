using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 361

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
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
    /// 当前格子属于的背包
    /// </summary>
    public BagScript MyBag { get; set; }
    
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

    /// <summary>
    /// 
    /// </summary>
    public ObservableStack<Item> MyItems
    {
        get
        {
            return items;
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
        // 鼠标左键点击
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty)
            {
                // 判断鼠标上是否拖拽背包物品
                if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Bag)
                {
                    // 如果有，而且当前格子中也是背包，就切换背包
                    if (MyItem is Bag)
                    {
                        InventoryScript.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag);
                    }
                }
                // 没有拖拽就直接拿起物品
                else
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.MyInstance.FromSlot = this;
                }
            }
            // 如果来源不是背包格子（FromSlot为空）、当前格子中物品已经耗尽、鼠标上的是背包（即鼠标上该物品为从装备上取下来的物品）
            else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty && (HandScript.MyInstance.MyMoveable is Bag))
            {   
                // 从鼠标上获取背包
                Bag bag = (Bag)HandScript.MyInstance.MyMoveable;

                // 确保脱下背包的时候不会放到自己里面，以及其他背包中有足够的空间
                if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.Slots > 0)
                {
                    // 将拖拽物品放到这个格子中
                    AddItem(bag);
                    
                    // 装备栏中移除背包
                    bag.MyBagButton.RemoveBag();
                    // 鼠标上移除
                    HandScript.MyInstance.Drop();
                }

            }
        
            // 如果有物品需要移动（从一个格子到另一个格子）
            else if (InventoryScript.MyInstance.FromSlot != null)
            {
                //尝试不同的方法对该行为进行处理
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) ||SwapItems(InventoryScript.MyInstance.FromSlot) ||AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
        }
        //右键点击，使用物品
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    /// <summary>
    /// 将物品添加到格子中（当前格子中）
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
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public void Clear()
    {
        if (items.Count > 0)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            MyItems.Clear();
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        //显示工具界面
        if (!IsEmpty)
        {
            UIManager.MyInstance.ShowToolitip(transform.position, MyItem);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}
