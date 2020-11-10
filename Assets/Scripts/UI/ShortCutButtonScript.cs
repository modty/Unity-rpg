using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShortCutButtonScript: MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    [SerializeField]private Image icon;
    [SerializeField]private Text bindKey;
    public Image Icon => icon;
    private ItemInGame itemInGame;

    public Text BindKey
    {
        get => bindKey;
        set => bindKey = value;
    }

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
            bindKey.text = ItemInGame.StackCount.ToString();
            bindKey.enabled=true;
        }
        else
        {
            bindKey.text = 1.ToString();
            bindKey.enabled=false;
        }
    }

    private void ItemUnShow()
    {
        Icon.enabled = false;
        bindKey.enabled = false;
    }
}
