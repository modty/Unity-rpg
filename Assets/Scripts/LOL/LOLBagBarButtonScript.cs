
using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LOLBagBarButtonScript : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    [SerializeField] private Image icon;
    public Image Icon => icon;
    private ItemInGame _itemInGame;

    public ItemInGame ItemInGame
    {
        get => _itemInGame;
        set => SwapItem(value);
    }

    private void SwapItem(ItemInGame fromItem)
    {
        _itemInGame = fromItem;
        if (fromItem != null) ItemShow();
        else ItemUnShow();
    }
    public void openCloseInventory()
    {
        LOLInventoryScript.Instance.OpenClose(ItemInGame);
    }
    public void OnDrag(PointerEventData eventData)
    {
        LOLMesPlaneScript.Instance.CalculatePointIconPlanePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if(obj.tag.Equals("BagBarSlot"))
        {
            LOLBagBarButtonScript target = obj.GetComponent<LOLBagBarButtonScript>();
            ItemInGame temp = target.ItemInGame;
            target.ItemInGame = ItemInGame;
            ItemInGame = temp;
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
    }

    private void ItemUnShow()
    {
        Icon.enabled = false;
    }
}

