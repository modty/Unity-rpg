﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// 可供使用的按钮
    /// </summary>
    public IUseable Useable { get; set; }

    [SerializeField]
    private Text stackSize;
    /// <summary>
    /// 可使用物品栈
    /// </summary>
    private Stack<IUseable> useables = new Stack<IUseable>();

    private int count;

    /// <summary>
    /// 引用当前对象的Button
    /// </summary>
    public Button Button { get; private set; }

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
        get
        {
            return count;
        }
    }


    public Text StackText
    {
        get
        {
            return stackSize;
        }
    }

    public Stack<IUseable> Useables
    {
        get
        {
            return useables;
        }

        set
        {
            if (value.Count > 0)
            {
                Useable = value.Peek();
            }
            else
            {
                Useable = null;
            }
            
            useables = value;
        }
    }

    [SerializeField]
    private Image icon;

    void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
        InventoryScript.Instance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);

    }

    void Update()
    {

    }

    public void OnClick()
    {
        if (HandScript.Instance.Moveable == null)
        {
            if (Useable != null)
            {
                Useable.Use();
            }
            else if (Useables != null && Useables.Count > 0)
            {
                Useables.Peek().Use();
            }
        }

    }

    /// <summary>
    /// 鼠标拖动物体到快捷栏，并点击
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.Instance.Moveable != null && HandScript.Instance.Moveable is IUseable)
            {
                SetUseable(HandScript.Instance.Moveable as IUseable);
            }
        }
    }

    /// <summary>
    /// 将鼠标上的物体设置到快捷栏，并统计所有已经装备背包中该物品的总数量。
    /// </summary>
    public void SetUseable(IUseable useable)
    {
        if (useable is Item)
        {
            // 获取背包中放到快捷栏的所有该物体，并压入栈中
            Useables = InventoryScript.Instance.GetUseables(useable);
            // 物品来源格子颜色变化
            if (InventoryScript.Instance.FromSlot != null)
            {
                InventoryScript.Instance.FromSlot.Cover.enabled = false;
                InventoryScript.Instance.FromSlot.Icon.enabled = true;
                InventoryScript.Instance.FromSlot = null;
            }
 

        }
        else
        {
            Useables.Clear();
            this.Useable = useable;
        }

        count = Useables.Count;
        UpdateVisual(useable as IMoveable);
        UIManager.Instance.RefreshTooltip(Useable as IDescribable);
    }
    /// <summary>
    /// 更新快捷键的图标等数据
    /// </summary>
    public void UpdateVisual(IMoveable moveable)
    {
        if (HandScript.Instance.Moveable != null)
        {
            HandScript.Instance.Drop();
        }

        Icon.sprite = moveable.Icon;
        Icon.enabled = true;

        if (count > 1)
        {
            UIManager.Instance.UpdateStackSize(this);
        }
        else if (Useable is Spells)
        {
            UIManager.Instance.ClearStackCount(this);
        }
    }
    /// <summary>
    /// 更新物品数量（快捷栏）
    /// </summary>
    /// <param name="item"></param>
    public void UpdateItemCount(Item item)
    {
        // 如果物品可使用且使用堆栈中还有没有使用的该物品
        if (item is IUseable && Useables.Count > 0)
        {
            // 弹出物品并再次验证
            if (Useables.Peek().GetType() == item.GetType())
            {
                Useables = InventoryScript.Instance.GetUseables(item as IUseable);

                count = Useables.Count;

                UIManager.Instance.UpdateStackSize(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable tmp = null;

        if (Useable != null && Useable is IDescribable)
        {
            tmp = (IDescribable)Useable;
            //UIManager.Instance.ShowToolitip(transform.position);
        }
        else if (Useables.Count > 0)
        {
            // UIManager.Instance.ShowToolitip(transform.position);
        }
        if (tmp != null)
        {
            UIManager.Instance.ShowTooltip(new Vector2(1, 0), transform.position, tmp);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
