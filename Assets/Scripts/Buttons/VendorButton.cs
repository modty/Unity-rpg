using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text price;

    [SerializeField]
    private Text quantity;

    private VendorItem vendorItem;

    public void AddItem(VendorItem vendorItem)
    {
        this.vendorItem = vendorItem;

        if (vendorItem.MyQuantity > 0 ||(vendorItem.MyQuantity == 0 && vendorItem.Unlimited))
        {

            icon.sprite = vendorItem.MyItem.MyIcon;
            title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[vendorItem.MyItem.MyQuality], vendorItem.MyItem.MyTitle);

            if (!vendorItem.Unlimited)
            {
                quantity.text = vendorItem.MyQuantity.ToString();
            }
            else
            {
                quantity.text = string.Empty;
            }

            if (vendorItem.MyItem.MyPrice > 0)
            {
                price.text = "价格: " + vendorItem.MyItem.MyPrice.ToString();
            }
            else
            {
                price.text = string.Empty;
            }

            gameObject.SetActive(true);
        }

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Player.MyInstance.MyGold >= vendorItem.MyItem.MyPrice) && InventoryScript.MyInstance.AddItem(Instantiate(vendorItem.MyItem)))
        {
            SellItem();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.MyInstance.ShowTooltip(new Vector2(0, 1), transform.position, vendorItem.MyItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

    private void SellItem()
    {
        Player.MyInstance.MyGold -= vendorItem.MyItem.MyPrice;

        if (!vendorItem.Unlimited)
        {
            vendorItem.MyQuantity--;
            quantity.text = vendorItem.MyQuantity.ToString();

            if (vendorItem.MyQuantity == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
