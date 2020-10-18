using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 背包快捷栏的格子
/// </summary>
// 97
public class BagButton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 对背包对象的引用
    /// </summary>
    private Bag bag;

    /// <summary>
    /// 标志背包是满还是空
    /// </summary>
    [SerializeField]
    private Sprite full, empty;

    /// <summary>
    /// 背包类
    /// </summary>
    public Bag MyBag
    {
        get
        {
            return bag;
        }

        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }

            bag = value;
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
            if (InventoryScript.MyInstance.FromSlot != null && HandScript.MyInstance.MyMoveable != null &&
                HandScript.MyInstance.MyMoveable is Bag)
            {
                // 当前背包栏已经装备背包
                if (MyBag != null)
                {
                    // 切换背包
                    InventoryScript.MyInstance.SwapBags(MyBag, HandScript.MyInstance.MyMoveable as Bag);
                }
                // 没有装备背包
                else
                {
                    // 新建一个背包并赋值
                    Bag tmp = (Bag) HandScript.MyInstance.MyMoveable;
                    tmp.MyBagButton = this;
                    tmp.Use();
                    MyBag = tmp;
                    // 删除鼠标上的对象
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
            // 如果按下LeftShift键，表示取下当前装备的背包
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                // 拿起背包
                HandScript.MyInstance.TakeMoveable(MyBag);
            }
            // 都不是，就打开、关闭背包
            else if (bag != null) // 如果有背包装备上
            {
                // 打开或者关闭背包
                bag.MyBagScript.OpenClose();
            }

        }
    }
    /// <summary>
    /// 从装备栏移除背包
    /// </summary>
    public void RemoveBag()
    {
        InventoryScript.MyInstance.RemoveBag(MyBag);
        MyBag.MyBagButton = null;

        foreach (Item item in MyBag.MyBagScript.GetItems())
        {
            // 将移除背包中的物品放到其他背包中
            InventoryScript.MyInstance.AddItem(item);
        }

        MyBag = null;
    }
}
