using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// 185
public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable,IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// 可供使用的按钮
    /// </summary>
    public IUseable MyUseable { get; set; }

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
    public Button MyButton { get; private set; }
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
        get
        {
            return count;
        }
    }
    
    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }
    
    [SerializeField]
    private Image icon;

    void Start ()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);

    }
	
    void Update () {
		
    }


    public void OnClick()
    {
        if (HandScript.MyInstance.MyMoveable == null)
        {
            if (MyUseable != null)
            {
                MyUseable.Use();
            }
            if (useables != null && useables.Count > 0)
            {
                useables.Peek().Use();
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
            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable)
            {
                SetUseable(HandScript.MyInstance.MyMoveable as IUseable); 
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
            useables = InventoryScript.MyInstance.GetUseables(useable);
            // 物品来源格子颜色变化
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            InventoryScript.MyInstance.FromSlot = null;
        }
        else
        {
            useables.Clear();
            this.MyUseable = useable;
        }
        UpdateVisual();
    }
    /// <summary>
    /// 更新快捷键的图标等数据
    /// </summary>
    public void UpdateVisual() 
    {
        MyIcon.sprite = HandScript.MyInstance.Put().MyIcon;
        MyIcon.color = Color.white;
        if (count > 1)
        {
            UIManager.MyInstance.UpdateStackSize(this);
        }else if (MyUseable is Spell)
        {
            UIManager.MyInstance.ClearStackCount(this);
        }
    }
    /// <summary>
    /// 更新物品数量（快捷栏）
    /// </summary>
    /// <param name="item"></param>
    public void UpdateItemCount(Item item)
    {
        // 如果物品可使用且使用堆栈中还有没有使用的该物品
        if (item is IUseable && useables.Count > 0)
        {
            // 弹出物品并再次验证
            if (useables.Peek().GetType() == item.GetType())
            {
                useables = InventoryScript.MyInstance.GetUseables(item as IUseable);

                count = useables.Count;

                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable tmp = null;

        if (MyUseable !=null && MyUseable is IDescribable)
        {
            tmp = (IDescribable)MyUseable;
        }
        else if (useables.Count > 0)
        {
            // UIManager.MyInstance.ShowToolitip(transform.position);
        }
        if (tmp != null)
        {
            UIManager.MyInstance.ShowToolitip(new Vector2(1,0),transform.position, tmp);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
    
}