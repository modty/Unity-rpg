using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    /// 点击背包按钮
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (bag != null)// 如果装备有背包
        {
            // 打开或关闭背包
            bag.MyBagScript.OpenClose();
        }
    }
}
