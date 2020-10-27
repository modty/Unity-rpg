using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [FormerlySerializedAs("item")] [SerializeField]
    private ItemInGame itemInGame;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text stack;

    private int count;

    public ItemInGame ItemInGame
    {
        get
        {
            return itemInGame;
        }

        set
        {
            itemInGame = value;
        }
    }

    public void Initialize(ItemInGame itemInGame, int count)
    {
        this.ItemInGame = itemInGame;
        this.image.sprite = itemInGame.Icon;
        this.title.text = string.Format("<color={0}>{1}</color>", "#00ff00ff", itemInGame.Name);
        this.count = count;

        if (count > 1)
        {
            stack.enabled = true;
        }
    }
    
    public void UpdateStackCount()
    {
        stack.text = InventoryScript.Instance.GetItemCount(ItemInGame.Name) + "/" + count.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
//        UIManager.Instance.ShowTooltip(new Vector2(0, 0), transform.position, ItemInGame);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }

}
