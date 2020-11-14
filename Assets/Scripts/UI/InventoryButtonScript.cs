using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButtonScript:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IEndDragHandler,IBeginDragHandler
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
           MesPlaneScript.Instance.ShowItemMes(string.Format("<color="+DataManager.Instance.GetQuality(ItemInGame.Uid).color+">"+ItemInGame.Name+"</color>"));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemInGame!=null)
        {
            MesPlaneScript.Instance.CloseItemMes();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        MesPlaneScript.Instance.CalculatePointIconPlanePosition();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if(obj==null) return;
        if (obj.tag.Equals("InventorySlot"))
        {
            InventoryButtonScript target = obj.GetComponent<InventoryButtonScript>();
            ItemInGame temp = target.ItemInGame;
            target.ItemInGame = ItemInGame;
            ItemInGame = temp;
        }
        // 拖拽物品到其他背包
        else if(obj.tag.Equals("BagBarSlot"))
        {
            
        }
        else if (obj.tag.Equals("ShortcutSlot"))
        {
            ShortCutButtonScript target = obj.GetComponent<ShortCutButtonScript>();
            ItemInGame temp = target.ItemInGame;
            target.ItemInGame = ItemInGame;
            ItemInGame = temp;
        }
        MesPlaneScript.Instance.PointIconClose();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemInGame!=null)
        {
            MesPlaneScript.Instance.PointIconShow(ItemInGame,GetComponent<RectTransform>().sizeDelta);
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

    public void ItemUse()
    {
        if (itemInGame != null)
        {
            switch (Utils.GetItemType(ItemInGame.Uid))
            {
                case 2:
                    if (itemInGame.StackCount > 0)
                    {
                        if (((ConsumableInGame) itemInGame.Item).Use())
                        {
                            itemInGame.StackCount -= 1;
                            if (itemInGame.StackCount <= 0)
                            {
                                itemInGame = null;
                                ItemUnShow();
                            }
                            else
                            {
                                num.text = itemInGame.StackCount.ToString();
                            }
                        }
                    }
                    break;
            }
            MesPlaneScript.Instance.CloseItemMes();
        }
    }
}
