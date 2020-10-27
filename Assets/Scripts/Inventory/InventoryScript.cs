using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.Serialization;
using static New.Item;

public delegate void ItemCountChanged(ItemInGame itemInGame);
/// <summary>
/// 界面UI中显示背包的区域
/// </summary>
public class InventoryScript : MonoBehaviour
{
    // 物品数目发生变化时
    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;

    public static InventoryScript Instance
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
    [FormerlySerializedAs("items")] [SerializeField]
    private ItemInGame[] itemsInGame;
    /// <summary>
    /// 背包栏（5个）是否还能装备背包
    /// </summary>
    public bool CanAddBag
    {
        get { return Bags.Count < 5; }
    }

    /// <summary>
    /// 所有已经装备的背包空的格子数（不装备不计算）
    /// </summary>
    public int EmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in Bags)
            {
                count += bag.BagScript.EmptySlotCount;
            }

            return count;
        }
    }
    /// <summary>
    /// 所有已经装备的背包的格子数（不装备不计算）
    /// </summary>
    public int TotalSlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in Bags)
            {
                count += bag.BagScript.Slots.Count;
            }

            return count;
        }
    }
    /// <summary>
    /// 所有已经装备的背包中的物品数
    /// </summary>
    public int FullSlotCount
    {
        get
        {
            return TotalSlotCount - EmptySlotCount;
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
                fromSlot.Cover.enabled = true;
            }
        }
    }

    public List<Bag> Bags
    {
        get
        {
            return bags;
        }
    }

    private void Awake()
    {
        // 初始化一个背包，大小为20，并装备
//        Bag bag = (Bag)Instantiate(itemsInGame[8]);
//        bag.Initialize(20);
//        bag.Use();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
//            Bag bag = (Bag)Instantiate(itemsInGame[8]);
//            bag.Initialize(40);
//            AddItem(bag);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
//            Bag bag = (Bag)Instantiate(itemsInGame[8]);
//            bag.Initialize(20);
//            AddItem(bag);

        }
        if (Input.GetKeyDown(KeyCode.M))
        {
//            HealthPotion potion = (HealthPotion)Instantiate(itemsInGame[9]);
//            AddItem(potion);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
//            GoldNugget nugget = (GoldNugget)Instantiate(itemsInGame[11]);
//            AddItem(nugget);
//            AddItem((HealthPotion)Instantiate(itemsInGame[9]));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
//            AddItem((Armor)Instantiate(itemsInGame[0]));
//            AddItem((Armor)Instantiate(itemsInGame[1]));
//            AddItem((Armor)Instantiate(itemsInGame[2]));
//            AddItem((Armor)Instantiate(itemsInGame[3]));
//            AddItem((Armor)Instantiate(itemsInGame[4]));
//            AddItem((Armor)Instantiate(itemsInGame[5]));
//            AddItem((Armor)Instantiate(itemsInGame[6]));
//            AddItem((Armor)Instantiate(itemsInGame[7]));
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
            if (bagButton.Bag == null)
            {
                bagButton.Bag = bag;
                Bags.Add(bag);
                bag.BagButton = bagButton;
                bag.BagScript.transform.SetSiblingIndex(bagButton.BagIndex);
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
        Bags.Add(bag);
        bagButton.Bag = bag;
        bag.BagScript.transform.SetSiblingIndex(bagButton.BagIndex);
    }

    public void AddBag(Bag bag, int bagIndex)
    {
        bag.SetupScript();
        Bags.Add(bag);
        bag.BagScript.BagIndex = bagIndex;
        bag.BagButton = bagButtons[bagIndex];
        bagButtons[bagIndex].Bag = bag;
    }

    /// <summary>
    /// 从窗口栏中移除背包
    /// </summary>
    /// <param name="bag"></param>
    public void RemoveBag(Bag bag)
    {
        Bags.Remove(bag);
        Destroy(bag.BagScript.gameObject);
    }
    /// <summary>
    /// 交换两个背包
    /// </summary>
    /// <param name="oldBag"></param>
    /// <param name="newBag"></param>
    public void SwapBags(Bag oldBag, Bag newBag)
    {
        // 当新背包装备上后所有装备背包的格子数
        int newSlotCount = (TotalSlotCount - oldBag.SlotCount) + newBag.SlotCount;
        // 如果装备后的格子数比之前的多
        if (newSlotCount - FullSlotCount >= 0)
        {
            // 获取旧背包的所有物品
            List<ItemInGame> bagItems = oldBag.BagScript.GetItems();
            // 移除旧背包
            RemoveBag(oldBag);
            // 新背包按钮绑定
            newBag.BagButton = oldBag.BagButton;

            // 装备新背包
            newBag.Use();
            // 新背包物品复制
            foreach (ItemInGame item in bagItems)
            {
                if (item != newBag)// 避免旧背包中含有新背包，物品复制后会有新背包指向新背包的引用
                {
                    AddItem(item);
                }
            }

            // 把旧背包放到新背包中
            AddItem(oldBag);
            // 鼠标上物品丢下
            HandScript.Instance.Drop();
            // 将记录原来背包的引用置空
            Instance.fromSlot = null;

        }
    }

    /// <summary>
    /// 往背包中添加物品（目标位所有已经装备的背包）
    /// </summary>
    /// <param name="itemInGame">要添加的物品</param>
    public int[] AddItem(ItemInGame itemInGame)
    {
        if (itemInGame.StackCount > 0)
        {
            // 如果添加的物品有数量，尝试堆叠，如果堆叠成功，直接返回，不然放到空位上
            int[] position = PlaceInStack(itemInGame);
            if (position[1]!=-1)
            {
                return position;
            }
        }
        // 堆叠失败，直接放置
        return PlaceInEmpty(itemInGame);
    }

    /// <summary>
    /// 将物品放到空格子中（检索范围为所有已经装备的背包）
    /// </summary>
    /// <param name="itemInGame">要添加的物品</param>
    private int[] PlaceInEmpty(ItemInGame itemInGame)
    {
        int[] position = {-1, -1};
        for (int i = 0; i < Bags.Count; i++)
        {
            List<SlotScript> slots = Bags[i].BagScript.Slots;
            for (int j = 0; j < slots.Count; j++)
            {
                if (slots[j].IsEmpty)
                {
                    position[0] = i;
                    position[1] = j;
                }
            }
        }
        return position;
    }

    /// <summary>
    /// 尝试堆叠物品（检索所有已经装备的背包）
    /// </summary>
    /// <param name="itemInGame">想要堆叠的物品</param>
    /// <returns></returns>
    private int[] PlaceInStack(ItemInGame itemInGame)
    {
        int[] position = {-1,-1};
        for (int i = 0; i < Bags.Count; i++)
        {
            List<SlotScript> slots = Bags[i].BagScript.Slots;
            for (int j = 0; j < slots.Count; j++)
            {
                position[0] = i;
                if(slots[j].StackItem(itemInGame))
                    position[1]=j;
                if (position[1]!=-1)// 尝试堆叠物品，堆叠成功触发物品数量改变事件，返回堆叠成功
                {
                    OnItemCountChanged(itemInGame);
                    return position;
                }
            }
        }
        return position;
    }

    public void PlaceInSpecific(ItemInGame itemInGame, int slotIndex, int bagIndex)
    {
        bags[bagIndex].BagScript.Slots[slotIndex].AddItem(itemInGame);

    }

    
    /// <summary>
    /// 关闭所有打开的背包，打开所有背包 
    /// </summary>
    public void OpenClose()
    {
        // 检查是否有关闭的背包（透明度为alpha=0（完全透明）的背包）
//        bool closedBag = Bags.Find(x => !x.BagScript.IsOpen);

        // 遍历所有已经装备的背包
        foreach (Bag bag in Bags)
        {
//            // 如果没有关闭的背包（closedBad = false 背包全部打开），全部关闭。如果有关闭的背包（closedBag = true），没有关闭的背包将会关闭
//            if (bag.BagScript.IsOpen != closedBag)
//            {
//                bag.BagScript.OpenClose();
//            }
        }
    }

    public List<SlotScript> GetAllItems()
    {
        List<SlotScript> slots = new List<SlotScript>();

        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.BagScript.Slots)
            {
                if (!slot.IsEmpty)
                {
                    slots.Add(slot);
                }
            }
        }

        return slots;
    }


    
    /// <summary>
    /// 将已装备背包中同类型的可使用物品压入栈中
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.BagScript.Slots)
            {
                if (!slot.IsEmpty && slot.ItemInGame.GetType() == type.GetType())
                {
                    foreach (ItemInGame item in slot.Items)
                    {
                        useables.Push(item as IUseable);
                    }
                }
            }
        }

        return useables;
    }

    public IUseable GetUseable(string type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.BagScript.Slots)
            {
                if (!slot.IsEmpty && slot.ItemInGame.Name == type)
                {
                    return (slot.ItemInGame as IUseable);
                }
            }
        }

        return null;
    }

    public int GetItemCount(string type)
    {
        int itemCount = 0;

        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.BagScript.Slots)
            {
                if (!slot.IsEmpty && slot.ItemInGame.Name == type)
                {
                    itemCount += slot.Items.Count;
                }
            }
        }

        return itemCount;

    }

    public Stack<ItemInGame> GetItems(string type, int count)
    {
        Stack<ItemInGame> items = new Stack<ItemInGame>();

        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.BagScript.Slots)
            {
                if (!slot.IsEmpty && slot.ItemInGame.Name == type)
                {
                    foreach (ItemInGame item in slot.Items)
                    {
                        items.Push(item);

                        if (items.Count == count)
                        {
                            return items;
                        }
                    }
                }
            }
        }

        return items;

    }

    public void RemoveItem(ItemInGame itemInGame)
    {
        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.BagScript.Slots)
            {
                if (!slot.IsEmpty && slot.ItemInGame.Name == itemInGame.Name)
                {
                    slot.RemoveItem(itemInGame);
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 当物品数目改变，唤醒事件
    /// </summary>
    /// <param name="itemInGame"></param>
    public void OnItemCountChanged(ItemInGame itemInGame)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(itemInGame);
        }
    }
}
