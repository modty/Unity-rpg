﻿using Items;
 using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 背包快捷栏的格子
/// </summary>
public class BagButton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 对背包对象的引用
    /// </summary>
    private Bag bag;

    private ItemInGame itemInGame;

    public ItemInGame ItemInGame
    {
        get => itemInGame;
        set
        {
            if (value==null)
            {
                equipmentSprite.enabled = false;
            }
            else
            {
                equipmentSprite.enabled = true;
                equipmentSprite.sprite = value.Icon;
            }
            itemInGame = value;
        }
    }

    private int bagIndex;
    /// <summary>
    /// 标志背包是满还是空
    /// </summary>
    [SerializeField]
    private Image emptySprite;
    [SerializeField]
    private Image equipmentSprite;

    public Bag Bag
    {
        get
        {
            return bag;
        }

        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = ItemInGame.Icon;
            }
            else
            {
            }

            bag = value;
        }
    }

    public int BagIndex
    {
        get
        {
            return bagIndex;
        }

        set
        {
            bagIndex = value;
        }
    }

    /// <summary>
    /// 点击背包
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // 点击鼠标左键
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 如果鼠标上有物体且鼠标上的物体是一个背包
            if (InventoryScript.Instance.FromSlot != null && HandScript.Instance.Moveable != null && HandScript.Instance.Moveable is Bag)
            {
                // 当前背包栏已经装备背包
                if (Bag != null)
                {
                    // 切换背包
                    InventoryScript.Instance.SwapBags(Bag, HandScript.Instance.Moveable as Bag);
                }
                // 没有装备背包
                else
                {
                    // 新建一个背包并赋值
                    Bag tmp = (Bag)HandScript.Instance.Moveable;
                    tmp.BagButton = this;
                    tmp.Use();
                    Bag = tmp;
                    // 删除鼠标上的对象
                    HandScript.Instance.Drop();
                    InventoryScript.Instance.FromSlot = null;
                }
            }
            // 如果按下LeftShift键，表示取下当前装备的背包
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                // 拿起背包
//                HandScript.Instance.TakeMoveable(Bag);
            }
            // 都不是，就打开、关闭背包
            else if (bag != null) // 如果有背包装备上
            {
                // 打开或者关闭背包
                bag.BagScript.OpenClose();
            }

        }

  
    }
    /// <summary>
    /// 从装备栏移除背包
    /// </summary>
    public void RemoveBag()
    {
        InventoryScript.Instance.RemoveBag(Bag);
        Bag.BagButton = null;

        foreach (ItemInGame item in Bag.BagScript.GetItems())
        {
            // 将移除背包中的物品放到其他背包中
            InventoryScript.Instance.AddItem(item);
        }

        Bag = null;
    }
}
