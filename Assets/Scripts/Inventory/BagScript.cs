using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 140
public class BagScript : MonoBehaviour
{

    /// <summary>
    /// 背包格子预制体
    /// </summary>
    [SerializeField]
    private GameObject slotPrefab;

    /// <summary>
    /// 背包画布
    /// </summary>
    private CanvasGroup canvasGroup;

    /// <summary>
    /// 背包中的所有物体
    /// </summary>
    private List<SlotScript> slots = new List<SlotScript>();

    /// <summary>
    /// 背包打开还是关闭（根据透明度alpha判断，1：打开，0：未打开），true : 背包被打开，false : 背包没有打开
    /// </summary>
    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    /// <summary>
    /// 当前背包中的所有格子
    /// </summary>
    public List<SlotScript> MySlots
    {
        get
        {
            return slots;
        }
    }
    /// <summary>
    /// 当前背包中剩余的格子数
    /// </summary>
    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (SlotScript slot in MySlots)
            {
                if (slot.IsEmpty)
                {
                    count++;
                }
            }

            return count;
        }
    }
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 获取当前背包中的所有物品
    /// </summary>
    /// <returns>所有物品的列表</returns>
    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();

        foreach (SlotScript slot in slots)
        {
            if (!slot.IsEmpty)
            {
                foreach (Item item in slot.MyItems)
                {
                    items.Add(item);
                }
            }
        }
        return items;
    }
    
    /// <summary>
    /// 为背包初始化格子
    /// </summary>
    /// <param name="slotCount">格子的数目</param>
    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.MyBag = this;
            MySlots.Add(slot);
        }
    }

    /// <summary>
    /// 往当前背包中添加物品
    /// </summary>
    /// <param name="item"></param>
    /// <returns>是否添加成功</returns>
    public bool AddItem(Item item)
    {
        foreach (SlotScript slot in MySlots)// 检查所有格子
        {
            if (slot.IsEmpty) // 有格子为空，就添加进去
            {
                slot.AddItem(item);

                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 打开、关闭背包（1、透明度转换，2、鼠标射线是否阻挡转换）
    /// </summary>
    public void OpenClose()
    {
        // 切换当前背包的透明度
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
    public void Clear()
    {
        foreach (SlotScript slot in slots)
        {
            slot.Clear();
        }
    }
}
