using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LOLInventoryButtonScript:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    [SerializeField]private Image icon;
    [SerializeField] private Text num;
    public Image Icon => icon;
    private ItemInGame itemInGame;
    
    public ItemInGame ItemInGame
    {
        get => itemInGame;
        set => SwapItem(value);
    }

    private void SwapItem(ItemInGame fromItem)
    {
        itemInGame = fromItem;
        if (fromItem != null) ItemShow();
        else ItemUnShow();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemInGame != null)
        {
           LOLMesPlaneScript.Instance.ShowItemMes(string.Format("<color="+DataManager.Instance.GetQuality(ItemInGame.Uid).color+">"+ItemInGame.Name+"</color>"));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemInGame!=null)
        {
            LOLMesPlaneScript.Instance.CloseItemMes();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        LOLMesPlaneScript.Instance.CalculatePointIconPlanePosition();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj.tag.Equals("InventorySlot"))
        {
            LOLInventoryButtonScript target = obj.GetComponent<LOLInventoryButtonScript>();
            ItemInGame temp = target.ItemInGame;
            target.ItemInGame = ItemInGame;
            ItemInGame = temp;
        }
        // 拖拽物品到其他背包
        else if(obj.tag.Equals("BagBarSlot"))
        {
            
        }
        LOLMesPlaneScript.Instance.PointIconClose();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemInGame!=null)
        {
            LOLMesPlaneScript.Instance.PointIconShow(ItemInGame,GetComponent<RectTransform>().sizeDelta);
        }
    }

    private void ItemShow()
    {
        Icon.sprite = ItemInGame.Icon;
        Icon.enabled = true;
        if (ItemInGame.StackCount > 1)
        {
            num.text = ItemInGame.StackCount.ToString();
            num.enabled=true;
        }
        else
        {
            num.text = 1.ToString();
            num.enabled=false;
        }
    }

    private void ItemUnShow()
    {
        Icon.enabled = false;
        num.enabled = false;
    }
}
