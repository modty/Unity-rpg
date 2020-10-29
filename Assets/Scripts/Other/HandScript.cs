using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{

    private static HandScript instance;

    public static HandScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }

    public ItemInGame itemInGame;

    public ItemInGame ItemInGame
    {
        get => itemInGame;
        set => itemInGame = value;
    }

    /// <summary>
    /// 当前鼠标拉取的对象
    /// </summary>
    public IMoveable Moveable { get; set; }
    /// <summary>
    /// 鼠标拉取对象的图标
    /// </summary>
    private Image icon;

    /// <summary>
    /// 拉取物体距离鼠标的偏移量
    /// </summary>
    [SerializeField]
    private Vector3 offset;

    void Start ()
    {
        icon = GetComponent<Image>();	
	}
	
	void Update ()
    {
        icon.transform.position = Input.mousePosition+offset;

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && Instance.Moveable != null)
        {
            DeleteItem();
        }

      
	}


    /// <summary>
    /// 将拉取物体复制到鼠标位置，跟随鼠标移动
    /// </summary>
    /// <param name="moveable">可移动的对象</param>
    public void TakeMoveable(IMoveable moveable)
    {
        this.Moveable = moveable;
        icon.sprite = moveable.Icon;
        icon.enabled = true;
    }

    public IMoveable Put()
    {
        IMoveable tmp = Moveable;
        Moveable = null;
        icon.enabled = false;
        return tmp;
    }
    /// <summary>
    /// 取消鼠标上显示的物品图标（放下物品）
    /// </summary>
    public void Drop()
    {
        Moveable = null;
        icon.enabled = false;
        InventoryScript.Instance.FromSlot = null;
    }
    
    /// <summary>
    /// 从仓库中删除物品
    /// </summary>
    public void DeleteItem()
    {
        if (Moveable is ItemInGame)
        {
            ItemInGame itemInGame = (ItemInGame)Moveable;
//            if (itemInGame.Slot != null)
//            {
//                itemInGame.Slot.Clear();
//            }
//            else if (itemInGame.CharButton != null)
//            {
//                itemInGame.CharButton.DequipArmor();
//            }
      
        }

        Drop();

        InventoryScript.Instance.FromSlot = null;
    }
}
