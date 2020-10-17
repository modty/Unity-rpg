using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    /// <summary>
    /// 单例对象
    /// </summary>
    private static HandScript instance;

    public static HandScript MyInstance
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

    /// <summary>
    /// 当前鼠标拉取的对象
    /// </summary>
    public IMoveable MyMoveable { get; set; }

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
        // 使拉取的物体跟随鼠标
        icon.transform.position = Input.mousePosition+offset;
        DeleteItem();
    }

    /// <summary>
    /// 将拉取物体复制到鼠标位置，跟随鼠标移动
    /// </summary>
    /// <param name="moveable">可移动的对象</param>
    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }

    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
        return tmp;
    }
    
    /// <summary>
    /// 取消鼠标上显示的物品图标（放下物品）
    /// </summary>
    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
    }
    /// <summary>
    /// 从仓库中删除物品
    /// </summary>
    private void DeleteItem()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null)
        {
            if (MyMoveable is Item && InventoryScript.MyInstance.FromSlot != null)
            {
                (MyMoveable as Item).MySlot.Clear();
            }

            Drop();

            InventoryScript.MyInstance.FromSlot = null;
        }
    }
}