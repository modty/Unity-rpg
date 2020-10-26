using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text stack;

    private int count;

    public Item Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;
        }
    }

    public void Initialize(Item item, int count)
    {
        this.Item = item;
        this.image.sprite = item.Icon;
        this.title.text = string.Format("<color={0}>{1}</color>", "#00ff00ff", item.Title);
        this.count = count;

        if (count > 1)
        {
            stack.enabled = true;
        }
    }
    
    public void UpdateStackCount()
    {
        stack.text = InventoryScript.Instance.GetItemCount(Item.Title) + "/" + count.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowTooltip(new Vector2(0, 0), transform.position, Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }

}
