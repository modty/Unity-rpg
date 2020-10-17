using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// 背包打开还是关闭
    /// </summary>
    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    public List<SlotScript> MySlots
    {
        get
        {
            return slots;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
            MySlots.Add(slot);
        }
    }

    /// <summary>
    /// 往背包中添加物品
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
    /// 打开、关闭背包
    /// </summary>
    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}
