using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 315


// 物品数目发生变化时
public delegate void ItemCountChanged(Item item);

/// <summary>
/// 界面UI中显示背包的区域
/// </summary>
public class InventoryScript : MonoBehaviour
{
    // 物品数目发生变化时
    public event ItemCountChanged itemCountChangedEvent;
    
    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }
    
    /// <summary>
    /// 拖拽物品的来源格子
    /// </summary>
    private SlotScript fromSlot;

    /// <summary>
    /// 游戏中的所有背包
    /// </summary>
    private List<Bag> bags = new List<Bag>();

    /// <summary>
    /// 游戏中背包装备栏（Bagbar）
    /// </summary>
    [SerializeField]
    private BagButton[] bagButtons;

    /// <summary>
    /// 游戏中的所有物品
    /// </summary>
    [SerializeField]
    private Item[] items;
    
    /// <summary>
    /// 背包栏（5个）是否还能装备背包
    /// </summary>
    public bool CanAddBag
    {
        get { return bags.Count < 5; }
    }

    /// <summary>
    /// 所有已经装备的背包空的格子数（不装备不计算）
    /// </summary>
    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }
            return count;
        }
    }

    /// <summary>
    /// 所有已经装备的背包的格子数（不装备不计算）
    /// </summary>
    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }

            return count;
        }
    }

    /// <summary>
    /// 所有已经装备的背包中的物品数
    /// </summary>
    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }

    /// <summary>
    /// 来源格子
    /// </summary>
    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }
        set
        {
            // 如果设置来源格子不为空，改变颜色
            fromSlot = value;

            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }

    private void Awake()
    {
        // 初始化一个背包，大小为20，并装备
        Bag bag = (Bag)Instantiate(items[8]);
        bag.Initialize(20);
        bag.Use();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            // 添加物品0（背包）
            Bag bag = (Bag)Instantiate(items[8]);
            bag.Initialize(20);
            AddItem(bag);
        }
        if (Input.GetKeyDown(KeyCode.K))//测试往背包中添加物品
        {
            Bag bag = (Bag)Instantiate(items[8]);
            bag.Initialize(8);
            AddItem(bag);

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            // 添加药瓶
            HealthPotion potion = (HealthPotion)Instantiate(items[9]);
            AddItem(potion);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddItem((Armor)Instantiate(items[0]));
            AddItem((Armor)Instantiate(items[1]));
            AddItem((Armor)Instantiate(items[2]));
            AddItem((Armor)Instantiate(items[3]));
            AddItem((Armor)Instantiate(items[4]));
            AddItem((Armor)Instantiate(items[5]));
            AddItem((Armor)Instantiate(items[6]));
            AddItem((Armor)Instantiate(items[7]));
            AddItem((Armor)Instantiate(items[10]));
        }
    
    }

    /// <summary>
    /// 往快捷栏中装备背包（鼠标右键）
    /// </summary>
    /// <param name="bag"></param>
    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                bags.Add(bag);
                bag.MyBagButton = bagButton;
                bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
                break;
            }
        }
    }
    /// <summary>
    /// 指定快捷栏装备背包（拖拽）
    /// </summary>
    /// <param name="bag"></param>
    /// <param name="bagButton"></param>
    public void AddBag(Bag bag, BagButton bagButton)
    {
        bags.Add(bag);
        bagButton.MyBag = bag;
        bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
    }
    /// <summary>
    /// 从窗口栏中移除背包
    /// </summary>
    /// <param name="bag"></param>
    public void RemoveBag(Bag bag)
    {
        bags.Remove(bag);
        Destroy(bag.MyBagScript.gameObject);
    }
    /// <summary>
    /// 交换两个背包
    /// </summary>
    /// <param name="oldBag"></param>
    /// <param name="newBag"></param>
    public void SwapBags(Bag oldBag, Bag newBag)
    {
        // 当新背包装备上后所有装备背包的格子数
        int newSlotCount = (MyTotalSlotCount - oldBag.Slots) + newBag.Slots;

        // 如果装备后的格子数比之前的多
        if (newSlotCount - MyFullSlotCount >= 0)
        {
            // 获取旧背包的所有物品
            List<Item> bagItems = oldBag.MyBagScript.GetItems();

            // 移除旧背包
            RemoveBag(oldBag);

            // 新背包按钮绑定
            newBag.MyBagButton = oldBag.MyBagButton;

            // 装备新背包
            newBag.Use();

            // 新背包物品复制
            foreach (Item item in bagItems)
            {
                if (item != newBag) // 避免旧背包中含有新背包，物品复制后会有新背包指向新背包的引用
                {
                    AddItem(item);
                }
            }

            // 把旧背包放到新背包中
            AddItem(oldBag);

            // 鼠标上物品丢下
            HandScript.MyInstance.Drop();

            // 将记录原来背包的引用置空
            MyInstance.fromSlot = null;

        }
    }
    
    /// <summary>
    /// 往背包中添加物品（目标位所有已经装备的背包）
    /// </summary>
    /// <param name="item">要添加的物品</param>
    public bool AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            // 如果添加的物品有数量，尝试堆叠，如果堆叠成功，直接返回，不然放到空位上
            if (PlaceInStack(item))
            {
                return true;
            }
        }
        // 堆叠失败，直接放置
        return PlaceInEmpty(item);
    }
    
    /// <summary>
    /// 将物品放到空格子中（检索范围为所有已经装备的背包）
    /// </summary>
    /// <param name="item">要添加的物品</param>
    private bool  PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                // 添加成功触发事件
                OnItemCountChanged(item);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 尝试堆叠物品（检索所有已经装备的背包）
    /// </summary>
    /// <param name="item">想要堆叠的物品</param>
    /// <returns></returns>
    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in bags) // 遍历所有背包
        {
            foreach (SlotScript slots in bag.MyBagScript.MySlots) // 检查当前背包的所有格子
            {
                if (slots.StackItem(item)) // 尝试堆叠物品，堆叠成功触发物品数量改变事件，返回堆叠成功
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 关闭所有打开的背包，打开所有背包 
    /// </summary>
    public void OpenClose()
    {
        // 检查是否有关闭的背包（透明度为alpha=0（完全透明）的背包）
        bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);

        // 遍历所有已经装备的背包
        foreach (Bag bag in bags)
        {
            // 如果没有关闭的背包（closedBad = false 背包全部打开），全部关闭。如果有关闭的背包（closedBag = true），没有关闭的背包将会关闭
            if (bag.MyBagScript.IsOpen != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }
    /// <summary>
    /// 将已装备背包中同类型的可使用物品压入栈中
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (Bag bag in bags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                {
                    foreach (Item item in slot.MyItems)
                    {
                        useables.Push(item as IUseable);
                    }
                }
            }
        }

        return useables;
    }
 
    
    /// <summary>
    /// 当物品数目改变，唤醒事件
    /// </summary>
    /// <param name="item"></param>
    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }
}
